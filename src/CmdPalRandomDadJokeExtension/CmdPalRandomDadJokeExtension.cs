using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CommandPalette.Extensions;

namespace CmdPalRandomDadJokeExtension;

[ComVisible(true)]
[Guid("16a2ba75-311c-4d5f-901f-8057c86a71c6")]
[ComDefaultInterface(typeof(IExtension))]
public sealed partial class CmdPalRandomDadJokeExtension : IExtension, IDisposable
{
  private readonly ManualResetEvent _extensionDisposedEvent;

  private readonly CmdPalRandomDadJokeExtensionCommandsProvider _provider = new();

  public CmdPalRandomDadJokeExtension(ManualResetEvent extensionDisposedEvent)
  {
    this._extensionDisposedEvent = extensionDisposedEvent;
  }

  public object? GetProvider(ProviderType providerType)
  {
    return providerType switch
    {
      ProviderType.Commands => _provider,
      _ => null,
    };
  }

  public void Dispose() => this._extensionDisposedEvent.Set();
}
