using System.Net.Http;
using System.Threading.Tasks;

namespace NotionExtension.Helpers;

internal static partial class NotionAPI
{
  const string _notionAPIUri = "https://api.notion.com/v1";

  /// <summary>
  /// Checks if the API key is valid by making a simple request to the Notion API
  /// </summary>
  /// <param name="apiKey">Notion API Key</param>
  /// <returns>Bool signifying if the request was successful.</returns>
  internal static async Task<bool> IsApiKeyValid(string apiKey)
  {

    using HttpClient client = new HttpClient();
    {
      try
      {
        // Make a simple request to verify the API key
        var response = await client.GetAsync($"{_notionAPIUri}/search");

        // If the response status code is 200, the API key is valid
        if (response.IsSuccessStatusCode)
        {
          return true;
        }

        // Optionally, handle other status codes and log errors
        return false;
      }
      catch
      {
        // If any exception occurs (e.g., network error), consider the API key invalid
        return false;
      }
    }
  }
}
