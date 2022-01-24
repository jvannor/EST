using System;
using System.Collections.Generic;
using System.Text;

namespace EST.Utilities
{
    internal class Constants
    {
        public const string Authority = "https://identity.dev.detroitcyclecar.com:5001";
        public const string ClientId = "EST.Mobile";
        public const string PostLogoutRedirectUri = "est.mobile://signout-callback-oidc";
        public const string RedirectUri = "est.mobile://signin-oidc";
        public const string Scope = "openid profile email EST.WebApi offline_access";
        public const string Api = "https://api.dev.detroitcyclecar.com:6001";
        public const string ReportsApiEndpoint = "/api/report";
        public const string SettingsApiEndpoint = "/api/settings";
        public const string SettingsDocument = "SettingsDocument";
    }
}
