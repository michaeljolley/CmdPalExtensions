using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRandomDadJokeExtension;

public partial class CmdPalRandomDadJokeExtensionCommandsProvider : CommandProvider
{
  private readonly CommandItem jokeCommandItem = new(new DadJokePage());

  public CmdPalRandomDadJokeExtensionCommandsProvider()
  {
    DisplayName = "Random Dad Joke";
    Icon = IconHelpers.FromRelativePath("Assets\\Square88x88Logo.png");
    Id = "com.baldbeardedbuilder.cmdpal.randomdadjoke";
  }

  public override ICommandItem[] TopLevelCommands() => [jokeCommandItem];
}
