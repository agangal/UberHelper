using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Storage;

namespace UberHelper
{
    public static class AuthHelper
    {
        public static async Task<bool> GetAccessToken(string clientID, string redirect_uri, string scope)
        {
            string access_token = null;
            try
            {                
                String uberAuthUrl = String.Format(UberAPI.UberAuthorizeUrl, Uri.EscapeDataString(clientID), Uri.EscapeDataString(scope), Uri.EscapeDataString(redirect_uri));
                Uri StartUri = new Uri(uberAuthUrl);
                Uri EndUri = new Uri(redirect_uri);
                
                // Make web request
                WebAuthenticationResult WebAuthResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, StartUri, EndUri);
                if (WebAuthResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    string responseData = WebAuthResult.ResponseData.ToString();
                    string subResponseData = responseData.Substring(responseData.IndexOf("access_token"));
                    String[] keyValPairs = subResponseData.Split('&');                   
                    string token_type = null;
                    for (int i = 0; i < keyValPairs.Length; i++)
                    {
                        String[] splits = keyValPairs[i].Split('=');
                        switch (splits[0])
                        {
                            case "access_token":
                                access_token = splits[1];
                                break;
                            case "token_type":
                                token_type = splits[1];
                                break;
                        }
                    }                    
                    ApplicationData.Current.LocalSettings.Values["Tokens"] = access_token;
                }
                else if (WebAuthResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    return (false);
                }
                else
                {
                   return (false);
                }

            }
            catch (Exception ex)
            {
                return (false);
            }
            return (true);
        }

    }
}
