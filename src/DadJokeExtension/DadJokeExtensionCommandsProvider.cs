using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace DadJokeExtension;

public partial class DadJokeExtensionCommandsProvider : CommandProvider
{
  private readonly CommandItem jokeCommandItem = new(new DadJokePage());

  public DadJokeExtensionCommandsProvider()
  {
    DisplayName = "Random Dad Joke";
    Icon = new("😜");
  }

  public override ICommandItem[] TopLevelCommands() => [jokeCommandItem];
}
