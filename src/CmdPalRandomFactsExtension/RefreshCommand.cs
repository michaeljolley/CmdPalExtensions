using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.Foundation;
using CmdPalRandomFactsExtension.Properties;

namespace CmdPalRandomFactsExtension;

internal sealed partial class RefreshCommand : InvokableCommand
{
  public event TypedEventHandler<object, object>? RefreshRequested;

  public RefreshCommand()
  {
    Icon = Icons.RefreshIcon;
    Name = Resources.extension_commands_refresh;
    Id = "com.baldbeardedbuilder.cmdpal.randomfacts.refresh";
  }

  public override ICommandResult Invoke()
  {
    RefreshRequested?.Invoke(this, this);
    return CommandResult.KeepOpen();
  }
}
