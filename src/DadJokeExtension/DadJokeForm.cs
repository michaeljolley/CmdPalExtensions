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
                "body": [],
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
