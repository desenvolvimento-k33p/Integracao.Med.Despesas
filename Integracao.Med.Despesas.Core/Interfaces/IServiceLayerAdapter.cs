using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Core.Interfaces
{
    public interface IServiceLayerAdapter
    {
        Task<HttpResponseMessage> Call<T>(string endPoint, HttpMethod method, object obj, string uri = null, string sessionId = null) where T : class;
        Task<CookieContainer> Login();
    }
}
