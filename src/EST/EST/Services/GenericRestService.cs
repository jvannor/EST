using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using EST.ServiceContracts;

namespace EST.Services
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

        public async Task<T> Post<T>(string uri, T data, string authToken = "")
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }

            var dataJson = JsonSerializer.Serialize<T>(data);
            var stringContent = new StringContent(dataJson);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var serviceResponse = await client.PostAsync(uri, stringContent);
            if (serviceResponse.IsSuccessStatusCode)
            {
                var responseBody = await serviceResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseBody))
                {
                    var result = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return result;
                }
                else
                {
                    throw new HttpRequestException("Null or empty response");
                }
            }

            throw new HttpRequestException($"HTTP error, {serviceResponse.StatusCode}");
        }

        public Task<R> Post<T, R>(string uri, T data, string authToken = "")
        {
            throw new NotImplementedException();
        }

        public async Task<T> Put<T>(string uri, T data, string authToken = "")
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }

            var dataJson = JsonSerializer.Serialize<T>(data);
            var stringContent = new StringContent(dataJson);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var serviceResponse = await client.PutAsync(uri, stringContent);
            if (serviceResponse.IsSuccessStatusCode)
            {
                T result = data;
                var responseBody = await serviceResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseBody))
                {
                    result = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
                return result;
            }

            throw new HttpRequestException($"HTTP error, {serviceResponse.StatusCode}");
        }

        public async Task Delete(string uri, string authToken = "")
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }

            var serviceResponse = await client.DeleteAsync(uri);
            if (serviceResponse.IsSuccessStatusCode)
            {
                return;
            }

            throw new HttpRequestException($"HTTP error, {serviceResponse.StatusCode}");
        }

        private HttpClient client = new HttpClient();
    }
}
