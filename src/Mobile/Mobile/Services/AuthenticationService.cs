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

        public async Task<bool> SignIn()
        { 
            bool result = false;
            try
            {
                var loginResult = await client.LoginAsync(new LoginRequest());
                if (!loginResult.IsError)
                {
                    await SecureStorage.SetAsync("est.mobile.accessToken", loginResult.AccessToken);
                    await SecureStorage.SetAsync("est.mobile.identityToken", loginResult.IdentityToken);
                    await SecureStorage.SetAsync("est.mobile.refreshToken", loginResult.RefreshToken);
                }

                result = !loginResult.IsError;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"AuthenticationService::SignIn() Exception; {ex.ToString()}");
            }
            return result;
        }

        public async Task<bool> SignOut()
        {
            bool result = false;
            try
            {
                var identityToken = await SecureStorage.GetAsync("est.mobile.identityToken");
                var logoutResult = await client.LogoutAsync(new LogoutRequest() { BrowserDisplayMode = DisplayMode.Hidden, IdTokenHint = identityToken });
                result = !logoutResult.IsError;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AuthenticationService::SignIn() Exception; {ex.ToString()}");
            }
            return result;
        }

        private OidcClient client;
    }
}
