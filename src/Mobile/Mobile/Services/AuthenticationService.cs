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

        public async Task<bool> Login()
        {
            try
            {
                var result = await client.LoginAsync();
                if (!result.IsError)
                {
                    var credentials = result.ToCredentials();
                    var json = JsonSerializer.Serialize(credentials);
                    await SecureStorage.SetAsync("est.mobile.credentials", json);
                    return true;
                }
                else
                    throw new ApplicationException($"Login failure, {result?.Error}; {result?.ErrorDescription}");
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"AuthenticationService::Login() experienced an unexpected exception, {ex.GetType().Name}; {ex.Message}");
                throw;
            }
        }

        public async Task<bool> Logout()
        {
            try
            {
                var json = await SecureStorage.GetAsync("est.mobile.credentials");
                var credentials = JsonSerializer.Deserialize<Credentials>(json);

                var result = await client.LogoutAsync(new LogoutRequest { IdTokenHint = credentials.IdentityToken });
                if (!result.IsError)
                    SecureStorage.Remove("est.mobile.credentials");
                else
                    throw new ApplicationException($"Logout failure, {result?.Error}; {result?.ErrorDescription}");

                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"AuthenticationService::Logout() experienced an unexpected exception, {ex.GetType().Name}; {ex.Message}");
                throw;
            }
        }

        public async Task<Credentials> GetCredentials()
        {
            try
            {
                return await RefreshCredentials(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AuthenticationService::GetCredentials() experienced an unexpected exception, {ex.GetType().Name}; {ex.Message}");
                throw;
            }
        }

        public async Task<Credentials> RefreshCredentials(bool force)
        {
            try
            {
                var json = await SecureStorage.GetAsync("est.mobile.credentials");
                var credentials = JsonSerializer.Deserialize<Credentials>(json);
                if (force || (DateTimeOffset.UtcNow.AddMinutes(15) >= credentials.AccessTokenExpiration))
                {
                    var result = await client.RefreshTokenAsync(credentials.RefreshToken);
                    if (!result.IsError)
                    {
                        credentials = result.ToCredentials();
                        json = JsonSerializer.Serialize(credentials);
                        await SecureStorage.SetAsync("est.mobile.credentials", json);
                    }
                    else
                        throw new ApplicationException($"Refresh failure, {result?.Error}; {result?.ErrorDescription}");
                }

                return credentials;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"AuthenticationService::RefreshCredentials() encountered an unexpected exception, {ex.GetType().Name}; {ex.Message}");
                throw;
            }
        }

        private OidcClient client;
    }
}
