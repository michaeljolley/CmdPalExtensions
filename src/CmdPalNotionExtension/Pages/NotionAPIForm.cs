using System.IO;
using System.Text.Json.Nodes;
using Microsoft.CommandPalette.Extensions.Toolkit;
using CmdPalNotionExtension.Helpers;

namespace CmdPalNotionExtension.Pages;

internal sealed partial class NotionAPIForm : FormContent
{
  public NotionAPIForm()
  {
    TemplateJson = $$"""
                  {
                    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
                    "type": "AdaptiveCard",
                    "version": "1.5",
                    "body": [
                      {
                        "type": "Input.Text",
                        "style": "password",
                        "id": "apiKey",
                        "label": "API Key",
                        "isRequired": true,
                        "errorMessage": "API Key required"
                      }
                    ],
                    "actions": [
                      {
                        "type": "Action.Submit",
                        "title": "Save",
                        "data": {
                          "apiKey": "apiKey"
                        }
                      }
                    ]
                  }
                  """;
  }

  public override CommandResult SubmitForm(string payload)
  {
    var formInput = JsonNode.Parse(payload);
    if (formInput == null)
    {
      return CommandResult.GoHome();
    }

    // get the name and url out of the values
    var formApiKey = formInput["apiKey"] ?? string.Empty;

    // Construct a new json blob with the name and url
    var json = $$"""
                  {
                      "apiKey": "{{formApiKey}}"
                  }
                  """;

    File.WriteAllText(NotionHelper.StateJsonPath(), json);
    return CommandResult.GoHome();
  }
}
