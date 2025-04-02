using System.IO;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace CmdPalNotionExtension.Helpers;

internal sealed class NotionHelper
{
  internal static IconInfo FavIcon() => new IconInfo("https://notion.so/images/favicon.ico");

  internal static string StateJsonPath()
  {
    var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
    Directory.CreateDirectory(directory);

    return Path.Combine(directory, "notion.state.json");
  }
}