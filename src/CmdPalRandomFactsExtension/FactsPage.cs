using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRandomFactsExtension;

internal sealed partial class FactsPage : ContentPage
{
  internal static readonly HttpClient Client = new();
  internal static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

  private readonly MarkdownContent _markdownContent;
  private readonly CopyTextCommand _copyContextCommand;
  private readonly CommandContextItem _copyContextMenuItem;
  private readonly RefreshCommand _refreshCommand;
  private readonly CommandContextItem _refreshContextMenuItem;

  public FactsPage()
  {
    Name = "Random Facts";
    Icon = IconHelpers.FromRelativePath("Assets\\Square88x88Logo.png");
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
    RefreshFact();

    return [_markdownContent];
  }

  public void RefreshFact()
  {
    IsLoading = true;

    var t = GetFactAsync();
    t.ConfigureAwait(false);
    var currentFact = t.Result;

    var markdown = string.Empty;

    if (string.IsNullOrEmpty(currentFact))
    {
      _copyContextCommand.Text = string.Empty;
      Commands = [_refreshContextMenuItem];
      markdown = GenerateMarkdown("Awe snap! We couldn't load a fact.");
    }
    else
    {
      _copyContextCommand.Text = currentFact;
      Commands = [_refreshContextMenuItem, _copyContextMenuItem];
      markdown = GenerateMarkdown(currentFact);
    }

    _markdownContent.Body = markdown;

    IsLoading = false;
  }

  private void HandleRefresh(object sender, object? args)
  {
    RaiseItemsChanged(1);
  }

  private static async Task<string?> GetFactAsync()
  {
    try
    {
      Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      // Make a GET request to the uselessfacts.jsph.pl API
      var response = await Client
          .GetAsync("https://uselessfacts.jsph.pl/api/v2/facts/random?language=en");
      response.EnsureSuccessStatusCode();

      // Read and deserialize the response JSON into a Fact object
      var responseBody = await response.Content.ReadAsStringAsync();

      var data = JsonSerializer.Deserialize<Fact>(responseBody, Options);

      return data?.Text;
    }
    catch (Exception e)
    {
      Console.WriteLine($"An error occurred: {e.Message}");
    }

    return string.Empty;
  }

  private static string GenerateMarkdown(string content)
  {
    return $@"# Random Fact

## {content}

";
  }
}
