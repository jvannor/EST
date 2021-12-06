using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient;
using System.Diagnostics;

namespace Mobile.Utilities
{
    public class AuthBrowser : IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            var result = new BrowserResult() { ResultType = BrowserResultType.Success };
            try
            {
                var webResult = await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri(options.EndUrl));
                if (string.Compare(options.EndUrl, Constants.RedirectUri) == 0)
                {
                    result.Response = ParseAuthenticatorResult(webResult);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"AuthBrowser::InvokeAsync() Exception; {ex.ToString()}");

                result.ResultType = BrowserResultType.UnknownError;
                result.Error = ex.Message;
                result.ErrorDescription = ex.ToString();
            }
            return result;
        }

        private string ParseAuthenticatorResult(WebAuthenticatorResult authenticatorResult)
        {
            string code = string.Empty;
            authenticatorResult?.Properties?.TryGetValue("code", out code);

            string scope = string.Empty;
            authenticatorResult?.Properties?.TryGetValue("scope", out scope);

            string state = string.Empty;
            authenticatorResult?.Properties?.TryGetValue("state", out state);

            string sessionState = string.Empty;
            authenticatorResult?.Properties?.TryGetValue("session_state", out sessionState);

            return $"{Constants.RedirectUri}#code={code}&scope={scope}&state={state}&session_state={sessionState}";
        }
    }
}
