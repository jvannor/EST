using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Akavache;
using EST.Models;
using EST.ServiceContracts;
using EST.Utilities;

namespace EST.Services
{
    public sealed class SettingsService : ServiceBase, ISettingsService
    {
        #region Methods

        public SettingsService(IAuthenticationService authenticationService, IBlobCache cache = null) : base(cache)
        {
            this.authenticationService = authenticationService;
            this.httpClient = new HttpClient();
        }

        public async Task<IEnumerable<ReportTemplate>> GetReportTemplates(string author = "", string subject = "")
        {
            if (string.IsNullOrEmpty(author))
            {
                author = await authenticationService.GetAuthor();
            }

            if (string.IsNullOrEmpty(subject))
            {
                subject = await authenticationService.GetSubject();
            }

            var doc = await GetSettingsDocument(author, subject);
            return doc.Templates;
        }


        public async Task<IEnumerable<string>> GetTags(string author = "", string subject = "")
        {
            if (string.IsNullOrEmpty(author))
            {
                author = await authenticationService.GetAuthor();
            }

            if (string.IsNullOrEmpty(subject))
            {
                subject = await authenticationService.GetSubject();
            }

            var doc = await GetSettingsDocument(author, subject);
            return doc.Tags;
        }

        public async Task SetReportTemplates(IEnumerable<ReportTemplate> templates, string author = "", string subject = "")
        {
            if (string.IsNullOrEmpty(author))
            {
                author = await authenticationService.GetAuthor();
            }

            if (string.IsNullOrEmpty(subject))
            {
                subject = await authenticationService.GetSubject();
            }

            var doc = await GetSettingsDocument(author, subject);
            doc.Templates = new ObservableCollection<ReportTemplate>(templates);
            await SetSettingsDocument(author, subject, doc);
        }

        public async Task SetTags(IEnumerable<string> tags, string author = "", string subject = "")
        {
            if (string.IsNullOrEmpty(author))
            {
                author = await authenticationService.GetAuthor();
            }

            if (string.IsNullOrEmpty(subject))
            {
                subject = await authenticationService.GetSubject();
            }

            var doc = await GetSettingsDocument(author, subject);
            doc.Tags = new ObservableCollection<string>(tags);

            await SetSettingsDocument(author, subject, doc);
        }

        private async Task<SettingsDocument> GetSettingsDocument(string author, string subject)
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

        private async Task SetSettingsDocument(string author, string subject, SettingsDocument settingsDocument)
        {
            var uri = $"{Constants.Api}{Constants.SettingsApiEndpoint}/{settingsDocument.Id}";
            var credentials = await authenticationService.GetCredentials();

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.AccessToken);

            var json = JsonSerializer.Serialize(settingsDocument);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PutAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                Cache.InvalidateObject<SettingsDocument>(Constants.SettingsDocument);
                Cache.InsertObject(Constants.SettingsDocument, settingsDocument, DateTime.Now.AddDays(1));

                return;
            }
            else
            {
                throw new HttpRequestException($"HTTP error, {response.StatusCode}");
            }
        }

        #endregion

        #region Fields

        IAuthenticationService authenticationService;
        HttpClient httpClient;

        #endregion

    }
}
