using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using Mobile.ServiceContracts;
using Mobile.Utilities;
using Xamarin.Essentials;
using IdentityModel.OidcClient.Browser;
using System.Text.Json;

namespace Mobile.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService()
        {
            client = new OidcClient(new OidcClientOptions()
            {
                Authority = Constants.Authority,
                Browser = new AuthBrowser(),
                ClientId = Constants.ClientId,
                PostLogoutRedirectUri = Constants.PostLogoutRedirectUri,
                RedirectUri = Constants.RedirectUri,
                Scope = Constants.Scope
            });
        }

        public async Task<bool> Authenticated()
        {
            var result = false;
            var json = await SecureStorage.GetAsync("est.mobile.credentials");
            if (!string.IsNullOrEmpty(json))
            {
                var credentials = JsonSerializer.Deserialize<Credentials>(json);
                if (DateTimeOffset.UtcNow < credentials.AccessTokenExpiration)
                {
                    result = true;
                }
            }
            return result; 
        }

        public async Task<bool> Login()
        {
            var result = false;
            var loginResult = await client.LoginAsync();
            if (!loginResult.IsError)
            {
                var credentials = loginResult.ToCredentials();
                var json = JsonSerializer.Serialize(credentials);
                await SecureStorage.SetAsync("est.mobile.credentials", json);
                result = true;
            }
            return result;
        }

        public async Task<bool> Logout()
        {
            var result = false;
            var json = await SecureStorage.GetAsync("est.mobile.credentials");
            if (!string.IsNullOrEmpty(json))
            {
                var credentials = JsonSerializer.Deserialize<Credentials>(json);
                var logoutResult = await client.LogoutAsync(new LogoutRequest { IdTokenHint = credentials.IdentityToken });
                if (!logoutResult.IsError)
                {
                    SecureStorage.Remove("est.mobile.credentials");
                    result = true;
                }
            }
            return result;
        }

        public async Task<Credentials> GetCredentials()
        {
            var result = await RefreshCredentials(false);
            return result;
        }

        public async Task<Credentials> RefreshCredentials(bool force)
        {
            Credentials result = null;
            var json = await SecureStorage.GetAsync("est.mobile.credentials");
            if (!string.IsNullOrEmpty(json))
            {
                var credentials = JsonSerializer.Deserialize<Credentials>(json);
                if (force || (DateTimeOffset.UtcNow.AddMinutes(15) >= credentials.AccessTokenExpiration))
                {
                    var refreshResult = await client.RefreshTokenAsync(credentials.RefreshToken);
                    if (!refreshResult.IsError)
                    {
                        result = refreshResult.ToCredentials();
                        json = JsonSerializer.Serialize(result);
                        await SecureStorage.SetAsync("est.mobile.credentials", json);
                    }
                }
            }
            return result;
        }

        private OidcClient client;
    }
}
