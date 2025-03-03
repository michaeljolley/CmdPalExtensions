using System.Net.Http;
using System.Text.Json;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace DadJokeExtension;

public partial class DadJokeExtensionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
	  internal static readonly HttpClient Client = new();
	  internal static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

	public DadJokeExtensionCommandsProvider()
    {
        DisplayName = "Random Dad Joke";
        Icon = new("😜");
        _commands = [
            new CommandItem(new DadJokePage()) { Title = DisplayName, Icon = Icon },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
