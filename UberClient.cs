using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberHelper
{
    public class UberClient
    {        
        private string redirect_uri;
        private string[] scope;

        #region constructor
        /// <summary>
        /// Creates a new client to interact with Uber's API
        /// </summary>
        /// <param name="clientid"></param>
        /// <param name="clientsecret"></param>
        /// <param name="redirect_uri"></param>
        /// <param name="scope"></param>
        public UberClient(string clientid, string clientsecret, string redirect_uri, string[] scope)
        {
            UberConstants.ClientId = clientid;
            UberConstants.StoreSecret(UberConstants.ClientSecretUserName, clientsecret);
            this.redirect_uri = redirect_uri;
            this.scope = scope;
        }
        #endregion

        #region public methods

        public async void SignIn()
        {
            await AuthHelper.Oauth2Flow(UberConstants.ClientId, redirect_uri, UberConstants.ReadSecret(UberConstants.ClientSecretUserName), string.Join(" ", scope));
        }

        private string GetUserProfile()
        {
            throw new NotImplementedException();
        }

        private string GetProducts()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
