using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRandomFactsExtension;

public partial class CmdPalRandomFactsExtensionCommandsProvider : CommandProvider
{
  private readonly CommandItem factCommandItem = new(new FactsPage());

  public CmdPalRandomFactsExtensionCommandsProvider()
  {
    DisplayName = "Random Facts";
    Icon = IconHelpers.FromRelativePath("Assets\\Square88x88Logo.png");
    Id = "com.baldbeardedbuilder.cmdpal.randomfacts";
  }

  public override ICommandItem[] TopLevelCommands() => [factCommandItem];
}
