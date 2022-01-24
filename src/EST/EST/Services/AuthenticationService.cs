using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Xamarin.Essentials;
using EST.ServiceContracts;
using EST.Utilities;

namespace EST.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService(ISettingsService settings)
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

            settingsService = settings;
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
                else
                {
                    credentials = await RefreshCredentials(true);
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
                settingsService.UserName = loginResult?.User?.Identity?.Name;
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
                if (logoutResult.IsError)
                {
                    Debug.WriteLine("AuthenticationService::Logout() - logout returned false");
                }

                settingsService.UserName = string.Empty;
                var storageResult = SecureStorage.Remove("est.mobile.credentials");
                if (!storageResult)
                {
                    Debug.WriteLine("AuthenticationService::Logout() - secure storage returned false");
                }

                result = true;
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
                result = JsonSerializer.Deserialize<Credentials>(json);
                if (force || (DateTimeOffset.UtcNow.AddMinutes(15) >= result.AccessTokenExpiration))
                {
                    var refreshResult = await client.RefreshTokenAsync(result.RefreshToken);
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
        private readonly ISettingsService settingsService;
    }
}
