using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Mobile.ServiceContracts;

namespace Mobile.Services
{
    internal class GenericRestService : IGenericRestService
    {
        public async Task<T> Get<T>(string uri, string authToken = "")
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }

            var serviceResponse = await client.GetAsync(uri);
            if (serviceResponse.IsSuccessStatusCode)
            {
                var responseBody = await serviceResponse.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return result;
            }

            throw new HttpRequestException($"HTTP error, {serviceResponse.StatusCode}");
        }

        public Task<T> Post<T>(string uri, T data, string authToken = "")
        {
            throw new NotImplementedException();
        }

        public Task<R> Post<T, R>(string uri, T data, string authToken = "")
        {
            throw new NotImplementedException();
        }

        public Task<T> Put<T>(string uri, T data, string authToken = "")
        {
            throw new NotImplementedException();
        }

        public Task Delete(string uri, string authToken = "")
        {
            throw new NotImplementedException();
        }

        private HttpClient client = new HttpClient();
    }
}
