using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.ServiceContracts
{
    internal interface IGenericRestService
    {
        Task<T> Get<T>(string uri, string authToken = "");

        Task<T> Post<T>(string uri, T data, string authToken = "");

        Task<R> Post<T,R>(string uri, T data, string authToken = "");

        Task<T> Put<T>(string uri, T data, string authToken = "");

        Task Delete(string uri, string authToken = "");
    }
}
