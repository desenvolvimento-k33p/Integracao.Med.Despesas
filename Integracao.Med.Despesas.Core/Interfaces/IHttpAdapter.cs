using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Core.Interfaces
{
    public interface IHttpAdapter
    {
        Task<HttpResponseMessage> Call<T>(
            HttpMethod method,
            string endPoint,
            object obj = null,
            string uri = null,string token = null) where T : class;
        Task<RestResponse<T>> ExecuteRequestAsync<T>(RestClient client, RestRequest request);


        Task<T> CallLoginKmm<T>(
           HttpMethod method,
           string endPoint,
           object obj = null,
           string uri = null,string apiKey = null) where T : class;

    }
}
