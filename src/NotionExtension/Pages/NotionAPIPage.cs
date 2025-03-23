using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace NotionExtension.Pages;

internal sealed partial class NotionAPIPage : ContentPage
{
    private readonly NotionAPIForm apiForm = new();

    public override IContent[] GetContent() => [apiForm];

    public NotionAPIPage()
    {
      Name = "Edit Notion API Key";
      Icon = new IconInfo("https://notion.so/favicon.ico");
    }
}
