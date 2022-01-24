using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Akavache;
using EST.Models;
using EST.ServiceContracts;
using EST.Utilities;

namespace EST.Services
{
    internal class SettingsDocumentService : ServiceBase, ISettingsDocumentService
    {
        public SettingsDocumentService(IAuthenticationService authn, IBlobCache cache = null) : base(cache)
        {
            authenticationService = authn;
            httpClient = new HttpClient();
        }

        public async Task<SettingsDocument> GetSettingsDocument(string author, string subject)
        {
            var doc = await GetFromCache<SettingsDocument>(Constants.SettingsDocument);
            if (doc != null)
            {
                return doc;
            }
            else
            {
                var uri = $"{Constants.Api}{Constants.SettingsApiEndpoint}?author={HttpUtility.UrlEncode(author.ToLower())}&subject={HttpUtility.UrlEncode(subject.ToLower())}";
                var credentials = await authenticationService.GetCredentials();

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.AccessToken);
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var document = JsonSerializer.Deserialize<SettingsDocument>(body, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    Cache.InsertObject(Constants.SettingsDocument, document, DateTime.Now.AddDays(1));
                    return document;
                }
                else
                {
                    throw new HttpRequestException($"HTTP error, {response.StatusCode}");
                }
            }
        }

        public async Task UpdateSettingsDocument(SettingsDocument document)
        {
            var uri = $"{Constants.Api}{Constants.SettingsApiEndpoint}/{document.Id}";
            var credentials = await authenticationService.GetCredentials();

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.AccessToken);

            var json = JsonSerializer.Serialize(document);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PutAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                Cache.InvalidateObject<SettingsDocument>(Constants.SettingsDocument);
                Cache.InsertObject(Constants.SettingsDocument, document, DateTime.Now.AddDays(1));

                return;
            }
            else
            {
                throw new HttpRequestException($"HTTP error, {response.StatusCode}");
            }
        }

        private IAuthenticationService authenticationService;
        private HttpClient httpClient;
    }
}
