using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRandomDadJokeExtension;

internal sealed partial class DadJokePage : ContentPage
{
  internal static readonly HttpClient Client = new();
  internal static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

  private readonly MarkdownContent _markdownContent;
  private readonly CopyTextCommand _copyContextCommand;
  private readonly CommandContextItem _copyContextMenuItem;
  private readonly RefreshCommand _refreshCommand;
  private readonly CommandContextItem _refreshContextMenuItem;

  public DadJokePage()
  {
    Name = "Random Dad Joke";
    Icon = IconHelpers.FromRelativePath("Assets\\Square88x88Logo.png");
    Id = "com.baldbeardedbuilder.cmdpal.randomdadjoke";
    _markdownContent = new MarkdownContent();

    _copyContextCommand = new CopyTextCommand(string.Empty);
    _copyContextMenuItem = new CommandContextItem(_copyContextCommand);

    _refreshCommand = new RefreshCommand();
    _refreshCommand.RefreshRequested += HandleRefresh;
    _refreshContextMenuItem = new CommandContextItem(_refreshCommand);

    Commands = [_refreshContextMenuItem, _copyContextMenuItem];
  }

  public override IContent[] GetContent()
  {
    RefreshJoke();

    return [_markdownContent];
  }

  public void RefreshJoke()
  {
    IsLoading = true;

    var t = GetJokeAsync();
    t.ConfigureAwait(false);
    var currentJoke = t.Result;

    var markdown = string.Empty;

    if (string.IsNullOrEmpty(currentJoke))
    {
      _copyContextCommand.Text = string.Empty;
      Commands = [_refreshContextMenuItem];
      markdown = GenerateMarkdown("Awe snap! We couldn't load a joke.");
    }
    else
    {
      _copyContextCommand.Text = currentJoke;
      Commands = [_refreshContextMenuItem, _copyContextMenuItem];
      markdown = GenerateMarkdown(currentJoke);
    }

    _markdownContent.Body = markdown;

    IsLoading = false;
  }

  private void HandleRefresh(object sender, object? args)
  {
    RaiseItemsChanged(1);
  }

  private static async Task<string?> GetJokeAsync()
  {
    try
    {
      Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      // Make a GET request to the icanhazdadjoke API
      var response = await Client
          .GetAsync("https://icanhazdadjoke.com/");
      response.EnsureSuccessStatusCode();

      // Read and deserialize the response JSON into a DadJoke object
      var responseBody = await response.Content.ReadAsStringAsync();

      var data = JsonSerializer.Deserialize<DadJoke>(responseBody, Options);

      return data?.Joke;
    }
    catch (Exception e)
    {
      Console.WriteLine($"An error occurred: {e.Message}");
    }

    return string.Empty;
  }

  private static string GenerateMarkdown(string content)
  {
    return $@"# Random Dad Joke

## {content}

";
  }
}
