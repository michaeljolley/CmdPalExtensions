using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.Foundation;

namespace CmdPalRandomDadJokeExtension;

internal sealed partial class RefreshCommand : InvokableCommand
{
  public event TypedEventHandler<object, object>? RefreshRequested;

  public RefreshCommand()
  {
    Icon = new("\uE72C");
    Name = "Refresh";
    Id = "com.baldbeardedbuilder.cmdpal.randomdadjoke.refresh";
  }

  public override ICommandResult Invoke()
  {
    RefreshRequested?.Invoke(this, this);
    return CommandResult.KeepOpen();
  }
}
