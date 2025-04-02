using System.IO;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

using CmdPalNotionExtension.Helpers;
using CmdPalNotionExtension.Pages;

namespace CmdPalNotionExtension;

public partial class CmdPalNotionExtensionCommandsProvider : CommandProvider
{
  public CmdPalNotionExtensionCommandsProvider()
  {
    DisplayName = "Notion";
    Icon = NotionHelper.FavIcon();
    Id = "com.baldbeardedbuilder.cmdpal.notion";
  }

  private readonly ICommandItem[] _commands = [];

  private readonly NotionAPIPage apiPage = new();

  public override ICommandItem[] TopLevelCommands()
  {
    return TopLevelCommandsAsync().GetAwaiter().GetResult();
  }

  public async Task<ICommandItem[]> TopLevelCommandsAsync()
  {
    var settingsPath = NotionHelper.StateJsonPath();

    // Check if the settings file exists
    if (!File.Exists(settingsPath))
    {
      return new[]
      {
        new CommandItem(apiPage)
        {
          Title = "Notion",
          Icon = NotionHelper.FavIcon(),
          Subtitle = "Enter your API key.",
        },
      };
    }

    // Read the file and parse the API key
    var state = File.ReadAllText(settingsPath);
    var jsonState = System.Text.Json.Nodes.JsonNode.Parse(state);
    var apiKey = jsonState?["apiKey"]?.ToString() ?? string.Empty;

    // Validate the API key using the Notion API
    if (string.IsNullOrWhiteSpace(apiKey) || !await NotionAPI.IsApiKeyValid(apiKey))
    {
      return new[]
      {
        new CommandItem(apiPage)
        {
            Title = "Notion",
            Icon = NotionHelper.FavIcon(),
            Subtitle = "Enter your API key.",
        },
      };
    }

    // If file exists and API key is valid, return commands
    return _commands;
  }
}