using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalRandomDadJokeExtension;

internal static class Icons
{
  internal static IconInfo DadJokeIcon { get; } = IconHelpers.FromRelativePath("Assets\\Square88x88Logo.png");

  internal static IconInfo RefreshIcon { get; } = new("\uE72C");
}
