using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using Mobile.ServiceContracts;
using Mobile.Utilities;

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
                var logoutResult = await client.LogoutAsync(new LogoutRequest());
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
