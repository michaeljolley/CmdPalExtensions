using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.Foundation;

namespace DadJokeExtension
{
    internal sealed partial class DadJokeForm : FormContent
    {
        internal event TypedEventHandler<object, object?>? RefreshCommand;

        public DadJokeForm(string joke)
        {
            TemplateJson = $$"""
            {
                "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
                "type": "AdaptiveCard",
                "version": "1.5",
                "body": [
                    {
                        "type": "TextBlock",
                        "size": "Medium",
                        "weight": "Bolder",
                        "text": "${{joke}}"
                    }
                ],
                "actions": [
                    {
                        "type": "Action.Submit",
                        "title": "Refresh"
                    }
                ]
            }
            """;
        }

        public override CommandResult SubmitForm(string payload)
        {
            RefreshCommand?.Invoke(this, null);
            return CommandResult.GoHome();
        }
    }
}
