using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberHelper
{
    internal class UberAPI
    {
        public const string UberAuthorizeUrl = "https://login.uber.com/oauth/v2/authorize?response_type=code&client_id={0}&scope={1}&redirect_uri={2}";
        public const string UberAccessTokenUrl = "https://login.uber.com/oauth/v2/token";
    }
}
