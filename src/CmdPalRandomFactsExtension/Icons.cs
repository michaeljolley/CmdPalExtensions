using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRandomFactsExtension;

internal static class Icons
{
  internal static IconInfo FactsIcon { get; } = IconHelpers.FromRelativePath("Assets\\Square88x88Logo.png");

  internal static IconInfo RefreshIcon { get; } = new("\uE72C");
}
