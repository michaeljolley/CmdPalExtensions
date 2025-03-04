using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.Foundation;

namespace DadJokeExtension;

internal sealed partial class DadJokeForm : FormContent
{
	internal event TypedEventHandler<object, object?>? RefreshCommand;
  private string _dadJoke = string.Empty;

	public DadJokeForm(string dadJoke)
	{
    TemplateJson = $$"""
    {
      "type": "AdaptiveCard",
      "body": [
          {
              "type": "TextBlock",
              "size": "Medium",
              "weight": "Bolder",
              "text": "${title}"
          },
          {
              "type": "ColumnSet",
              "columns": [
                  {
                      "type": "Column",
                      "width": 50,
                      "items": [
                          {
                              "type": "TextBlock",
                              "text": "${dadJoke}"
                              "wrap": true
                          }
                      ],
                      "spacing": "None",
                      "verticalContentAlignment": "Top"
                  },
                  {
                      "type": "Column",
                      "width": "stretch",
                      "items": [
                          {
                              "type": "ActionSet",
                              "actions": [
                                  {
                                      "type": "Action.Execute",
                                      "title": "Copy",
                                      "mode": "secondary"
                                  }
                              ]
                          }
                      ]
                  }
              ]
          }
      ],
      "actions": [
        {
            "type": "Action.Submit",
            "title": "Refresh"
        }
      ],
      "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
      "version": "1.6"
    }
    """;

    _dadJoke = dadJoke;
	}

	public override string DataJson 
  { 
    get => $"{{ \"title\": \"😜 Random Dad Joke\", \"dadJoke\": \"{_dadJoke}\" }}"; 
  }

	public override CommandResult SubmitForm(string payload)
	{
		RefreshCommand?.Invoke(this, null);
    return CommandResult.KeepOpen();
	}

  public void UseJoke(string dadJoke)
	{
    _dadJoke = dadJoke;
	}
}
