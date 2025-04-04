using System.Collections.Generic;
using System.Security;
using Windows.Security.Credentials;

namespace NotionExtension.Authentication;

public interface ICredentialVault
{
  PasswordCredential? GetCredentials(string loginId);

  void RemoveCredentials(string loginId);

  void SaveCredentials(string loginId, SecureString? accessToken);

  IEnumerable<string> GetAllCredentials();

  void RemoveAllCredentials();
}