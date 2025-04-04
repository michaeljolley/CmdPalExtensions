using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CommandPalette.Extensions;

namespace CmdPalRandomFactsExtension;

[ComVisible(true)]
[Guid("BC38F10C-DBFF-43A7-9FB1-E6A10209AF76")]
[ComDefaultInterface(typeof(IExtension))]
public sealed partial class CmdPalRandomFactsExtension : IExtension, IDisposable
{
  private readonly ManualResetEvent _extensionDisposedEvent;

  private readonly CmdPalRandomFactsExtensionCommandsProvider _provider = new();

  public CmdPalRandomFactsExtension(ManualResetEvent extensionDisposedEvent)
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
