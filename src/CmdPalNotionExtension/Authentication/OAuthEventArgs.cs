using System;

namespace NotionExtension.Authentication;

public sealed class OAuthEventArgs : EventArgs
{
  public string? AccessToken { get; }

  public string? BotId { get; }

  public Exception? Error { get; }

  public OAuthEventArgs(string? token, string? botId, Exception? error = null)
  {
    AccessToken = token;
    BotId = botId;
    Error = error;
  }
}