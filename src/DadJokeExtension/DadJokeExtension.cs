using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CommandPalette.Extensions;

namespace DadJokeExtension;

[ComVisible(true)]
[Guid("16a2ba75-311c-4d5f-901f-8057c86a71c6")]
[ComDefaultInterface(typeof(IExtension))]
public sealed partial class DadJokeExtension : IExtension, IDisposable
{
    private readonly ManualResetEvent _extensionDisposedEvent;

    private readonly DadJokeExtensionCommandsProvider _provider = new();

    public DadJokeExtension(ManualResetEvent extensionDisposedEvent)
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
