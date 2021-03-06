using System;
using System.Collections.Generic;
using System.Text;

namespace EST.Utilities
{
    // Credit: Mark Allibone
    // https://mallibone.com/post/xamarin-oidc-refresh

    public class Credentials
    {
        public string AccessToken { get; set; } = "";
        public string IdentityToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTimeOffset AccessTokenExpiration { get; set; }
        public string Error { get; set; } = "";
        public bool IsError => !string.IsNullOrEmpty(Error);
    }
}
