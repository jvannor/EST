using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Utilities
{
    internal class Constants
    {
        public const string Authority = "https://identity.dev.detroitcyclecar.com:5001";
        public const string ClientId = "EST.Mobile";
        public const string PostLogoutRedirectUri = "est.mobile://signout-callback-oidc";
        public const string RedirectUri = "est.mobile://signin-oidc";
        public const string Scope = "openid profile email EST.WebApi offline_access";
    }
}
