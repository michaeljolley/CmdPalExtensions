using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CommandPalette.Extensions;

namespace CmdPalNotionExtension;

[ComVisible(true)]
[Guid("D04193F6-842F-4AF4-924A-58A0CC7227F4")]
[ComDefaultInterface(typeof(IExtension))]
public sealed partial class CmdPalNotionExtension : IExtension, IDisposable
{
    private readonly ManualResetEvent _extensionDisposedEvent;

    private readonly CmdPalNotionExtensionCommandsProvider _provider = new();

    public CmdPalNotionExtension(ManualResetEvent extensionDisposedEvent)
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
