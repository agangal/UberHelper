using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Storage;

namespace UberHelper
{
    internal class UberAPI
    {
        public const string UberAuthorizeUrl = "https://login.uber.com/oauth/v2/authorize?response_type=code&client_id={0}&scope={1}&redirect_uri={2}";
        public const string UberAccessTokenUrl = "https://login.uber.com/oauth/v2/token";
    }

    internal class UberConstants
    {
        #region secrets

        private const string ClientIdUserName = "clientId";
        private const string AccessTokenUserName = "accesstoken";
        private const string RefreshTokenUserName = "refreshtoken";
        private const string UberHelperResource = "uberhelperresource";

        public static string ClientId
        {
            get
            {
                return (string)ApplicationData.Current.LocalSettings.Values[ClientIdUserName];
            }
            set
            {
                if (value != null)
                {
                    ApplicationData.Current.LocalSettings.Values[ClientIdUserName] = value;
                }
            }
        }
        public static void StoreSecret(string resource, string username, string secret)
        {
            PasswordVault vault = new PasswordVault();
            vault.Add(new PasswordCredential(resource, username, secret));
        }

        public static string ReadSecret(string resource, string username)
        {
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = vault.Retrieve(resource, username);
            return credential.Password;
        }

        public static void DeleteSecret(string resource, string username, string secret)
        {
            PasswordVault vault = new PasswordVault();
            vault.Remove(new PasswordCredential(resource, username, secret));
        }

        #endregion
    }
}
