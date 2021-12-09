using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Mobile.ServiceContracts;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Mobile.Services
{
    internal class GenericRestService : IGenericRestService
    {
        public Task Delete(string uri, string authToken = "")
        {
            throw new NotImplementedException();
        }

        public async Task<T> Get<T>(string uri, string authToken = "")
        {
            try
            {
                var client = new HttpClient();
                var result = string.Empty;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrEmpty(authToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                }

                var response = await Policy.Handle<WebException>(ex =>
                {
                    Debug.WriteLine($"GenericRestService::Get() encountered an unexpected exception, {ex.GetType().Name}; {ex.Message}");
                    return true;
                })
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async() => await client.GetAsync(uri));

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Debug.WriteLine(result);                  
                    
                    var resultObj = JsonSerializer.Deserialize<T>(result);
                    return resultObj;
                }

                throw new HttpRequestException($"GenericRestService::Get() encountered an unexpected HTTP error, {response.StatusCode}; {result}");
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"GenericRestService::Get() encountered an unexpected exception, {ex.GetType().Name}; {ex.Message}");
                throw;
            }
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
    }
}
