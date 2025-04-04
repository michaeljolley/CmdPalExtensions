using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotionExtension.Authentication;

public sealed class OAuthClient
{
  public event EventHandler<OAuthEventArgs>? AccessTokenChanged;

  private const string notionAPIOAuthUrl = "https://api.notion.com/v1/oauth";
  private const string client_id = "1cbd872b-594c-8053-9427-00379d9229a6";

  private static Uri CreateOAuthRequestUri()
  {
    var redirect_uri = "cmdpalnotionext://oauth_redirect_uri/";
    return new Uri($"{notionAPIOAuthUrl}/authorize?client_id={client_id}&response_type=code&owner=user&redirect_uri={redirect_uri}");
  }

  public static void BeginOAuthRequest()
  {
    var uri = CreateOAuthRequestUri();
    var options = new Windows.System.LauncherOptions();
    var browserLaunch = false;

    Task.Run(async () =>
    {
      browserLaunch = await Windows.System.Launcher.LaunchUriAsync(uri, options);

      if (browserLaunch)
      {
        Debug.WriteLine($"Uri Launched - Check browser");
      }
      else
      {
        Debug.WriteLine($"Uri Launch failed");
      }
    });
  }

  public async Task HandleOAuthRedirection(Uri response)
  {
    var queryString = response.Query;
    var queryStringCollection = System.Web.HttpUtility.ParseQueryString(queryString);

    if (queryStringCollection["error"] != null)
    {
      // Handle error
      Debug.WriteLine($"Error: {queryStringCollection["error"]}");
      return;
    }

    if (queryStringCollection["code"] == null)
    {
      Debug.WriteLine("No code found in the response.");
      return;
    }

    // Handle success
    var code = queryStringCollection["code"];
    Debug.WriteLine($"Code: {code}");

    // Use the code to get the access token
    var tokenUri = new Uri($"{notionAPIOAuthUrl}/token");

    var request = new HttpRequestMessage(HttpMethod.Post, tokenUri)
    {
      Content =
        new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code!),
                new KeyValuePair<string, string>("client_id", client_id),
                new KeyValuePair<string, string>("redirect_uri", "cmdpalmalext://oauth_redirect_uri/"),
        }),
    };

    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
    request.Content.Headers.Add("Notion-Version", "2022-06-28");
    request.Content.Headers.Add("Authorization", $"Basic {BASE64_ENCODED_ID_AND_SECRET}");

    Debug.WriteLine($"Request: {request.RequestUri}");
    Debug.WriteLine($"Content: {await request.Content.ReadAsStringAsync()}");

    using var client = new HttpClient();
    try
    {
      var responseMessage = await client.SendAsync(request);
      responseMessage.EnsureSuccessStatusCode();

      var responseContent = await responseMessage.Content.ReadAsStringAsync();
      var responseJson = JsonDocument.Parse(responseContent);

      var accessToken = responseJson.RootElement.GetProperty("access_token").GetString();
      var botId = responseJson.RootElement.GetProperty("bot_id").GetString();
      var expiresIn = responseJson.RootElement.GetProperty("expires_in").GetInt32();

      Debug.WriteLine($"Access Token: {accessToken}");
      Debug.WriteLine($"Bot Id: {botId}");

      AccessTokenChanged?.Invoke(null, new OAuthEventArgs(accessToken, botId));
    }
    catch (HttpRequestException ex)
    {
      Debug.WriteLine($"Request failed: {ex.Message}");
    }
    catch (JsonException ex)
    {
      Debug.WriteLine($"JSON parsing failed: {ex.Message}");
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"Unexpected error: {ex.Message}");
    }
  }
}