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

        public const string ClientIdUserName = "clientid";
        public const string ClientSecretUserName = "clientsecret";
        public const string AccessTokenUserName = "accesstoken";
        public const string RefreshTokenUserName = "refreshtoken";
        private const string resource = "uberhelperresource";

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

        /// <summary>
        /// Add a secret to the PasswordVault
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="secret">secret/password</param>
        public static void StoreSecret(string username, string secret)
        {
            PasswordVault vault = new PasswordVault();
            DeleteSecret(username, secret);
            vault.Add(new PasswordCredential(resource, username, secret));
        }

        /// <summary>
        /// Retrieve a secret/password for that username from the PasswordVault
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public static string ReadSecret(string username)
        {
            try
            {
                PasswordVault vault = new PasswordVault();
                PasswordCredential credential = vault.Retrieve(resource, username);
                return credential.Password;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Delete secret from the PasswordVault
        /// </summary>
        /// <param name="username"></param>
        /// <param name="secret"></param>
        public static void DeleteSecret(string username, string secret)
        {
            try
            {
                PasswordVault vault = new PasswordVault();
                vault.Remove(new PasswordCredential(resource, username, secret));
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion
    }
}
