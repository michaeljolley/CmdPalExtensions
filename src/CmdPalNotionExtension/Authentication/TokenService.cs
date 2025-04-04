using System;
using System.Net;

namespace NotionExtension.Authentication;

public sealed class TokenService
{
  private readonly ICredentialVault _credentialVault;

  private readonly OAuthClient _oAuthClient;

  public event EventHandler<bool>? LoginStateChanged;

  public TokenService(ICredentialVault credentialVault, OAuthClient oAuthClient)
  {
    _credentialVault = credentialVault;
    _oAuthClient = oAuthClient;
  }

  public bool IsLoggedIn()
  {
    var accessToken = _credentialVault.GetCredentials("MyAnimeList")?.Password;
    return !string.IsNullOrEmpty(accessToken);
  }

  public string GetAccessToken()
  {
    var accessToken = _credentialVault.GetCredentials("MyAnimeList")?.Password;
    return accessToken!;
  }

  public void SaveOrOverwriteAccessToken(string accessToken)
  {
    _credentialVault.SaveCredentials("MyAnimeList", new NetworkCredential(string.Empty, accessToken).SecurePassword);
  }

  public void SaveOrOverwriteRefreshToken(string refreshToken)
  {
    _credentialVault.SaveCredentials("MyAnimeListRefresh", new NetworkCredential(string.Empty, refreshToken).SecurePassword);
  }

  public void StartLoginUser()
  {
    _oAuthClient.AccessTokenChanged += OAuthTokenEventHandler;
    OAuthClient.BeginOAuthRequest();
  }

  public void LogoutUser()
  {
    _credentialVault.RemoveAllCredentials();
    LoginStateChanged?.Invoke(this, false);
  }

  public void StartRefreshAccessToken()
  {
    var refreshToken = _credentialVault.GetCredentials("MyAnimeListRefresh")?.Password;
    if (refreshToken == null)
    {
      throw new InvalidOperationException("No refresh token found");
    }

    _oAuthClient.AccessTokenChanged += OAuthTokenEventHandler;
    _ = _oAuthClient.RefreshAccessToken(refreshToken);
  }

  public void OAuthTokenEventHandler(object? sender, OAuthEventArgs e)
  {
    if (e.Error != null)
    {
      throw e.Error;
    }

    SaveOrOverwriteAccessToken(e.AccessToken!);
    SaveOrOverwriteRefreshToken(e.RefreshToken!);
    LoginStateChanged?.Invoke(this, true);
  }
}