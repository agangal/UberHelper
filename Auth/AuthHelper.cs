using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using Windows.Web.Http;

namespace UberHelper
{
    using Newtonsoft.Json;

    public static class AuthHelper
    {
        /// <summary>
        /// Oauth2.0 Flow for Uber
        /// </summary>
        /// <param name="clientID">ClientID of the Uber app</param>
        /// <param name="redirect_uri">Redirect URI</param>
        /// <param name="scope">List of space delimited scopes to request from Uber</param>
        public static async Task<bool> Oauth2Flow(string clientID, string redirect_uri, string clientsecret, string scope)
        {
            WebAuthenticationResult authCodeResult = await GetAuthorizationCode(clientID, redirect_uri, scope);
            if (authCodeResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                string responseData = authCodeResult.ResponseData.ToString();
                string subResponseData = responseData.Substring(responseData.IndexOf("code"));
                String[] keyValPairs = subResponseData.Split('&');
                string authCode = keyValPairs[1];

                Dictionary<string, string> pairs = new Dictionary<string, string>
                {
                    { "client_secret", clientsecret },
                    { "client_id", clientID },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", redirect_uri },
                    { "code", authCode }
                };

                HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(pairs);

                HttpClient client = new HttpClient();
                var httpresponseMessage = await client.PostAsync(new Uri(UberAPI.UberAccessTokenUrl), formContent);
                string response = await httpresponseMessage.Content.ReadAsStringAsync();
                UberAuthModel authResponse = JsonConvert.DeserializeObject<UberAuthModel>(response);
                UberConstants.StoreSecret(UberConstants.AccessTokenUserName, authResponse.access_token);
                UberConstants.StoreSecret(UberConstants.RefreshTokenUserName, authResponse.refresh_token);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get Authorization Code from Uber.
        /// The authorization code will be used in the Oauth2.0 flow to get an access token.
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="redirect_uri"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static async Task<WebAuthenticationResult> GetAuthorizationCode(string clientID, string redirect_uri, string scope)
        {
            try
            {                
                String uberAuthUrl = String.Format(UberAPI.UberAuthorizeUrl, Uri.EscapeDataString(clientID), Uri.EscapeDataString(scope), Uri.EscapeDataString(redirect_uri));
                Uri StartUri = new Uri(uberAuthUrl);
                Uri EndUri = new Uri(redirect_uri);
                
                // Make web request
                WebAuthenticationResult WebAuthResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, StartUri, EndUri);
                return WebAuthResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
