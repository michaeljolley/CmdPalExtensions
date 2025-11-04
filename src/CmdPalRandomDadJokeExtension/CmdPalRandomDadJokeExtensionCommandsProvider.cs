using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using CmdPalRandomDadJokeExtension.Properties;

namespace CmdPalRandomDadJokeExtension;

public partial class CmdPalRandomDadJokeExtensionCommandsProvider : CommandProvider
{
  private readonly DadJokePage dadJokePage = new();

  public CmdPalRandomDadJokeExtensionCommandsProvider()
  {
    DisplayName = Resources.extension_name;
    Icon = Icons.DadJokeIcon;
    Id = "com.baldbeardedbuilder.cmdpal.randomdadjoke";
  }

  public override ICommandItem[] TopLevelCommands() => [dadJokePage.ToCommandItem()];
}
