using CmdPalRandomFactsExtension.Properties;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRandomFactsExtension;

public partial class CmdPalRandomFactsExtensionCommandsProvider : CommandProvider
{
  private readonly FactsPage factsPage = new();

  public CmdPalRandomFactsExtensionCommandsProvider()
  {
    DisplayName = Resources.extension_name;
    Icon = Icons.FactsIcon;
    Id = "com.baldbeardedbuilder.cmdpal.randomfacts";
  }

  public override ICommandItem[] TopLevelCommands() => [factsPage.ToCommandItem()];
}
