using Integracao.Med.Despesas.Core.Interfaces;
using Integracao.Med.Despesas.Domain.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Integracao.Med.Despesas.Core.Adapters
{
    public class HttpAdapter : IHttpAdapter
    {
        private readonly ILogger<HttpAdapter> _logger;

        private readonly string KMM_AUTHORIZATION_KEY = "Authorization";
        public string KMM_AUTHORIZATION_VALUE;

        public HttpAdapter(ILogger<HttpAdapter> logger, IOptions<Configuration> configurations)
        {
            _logger = logger;
            //KMM_AUTHORIZATION_VALUE = configurations.Value.KmmHttp.Key;
        }

        //public async Task<T> Call<T>(
        //    HttpMethod method,
        //    string endPoint,
        //    object obj = null,
        //    string uri = null, string token = null) where T : class
        //{
        //    try
        //    {
        //        KMM_AUTHORIZATION_VALUE = "Bearer " + token;
        //        var clientHandler = new HttpClientHandler()
        //        {
        //            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
        //        };

        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(uri);
        //            client.DefaultRequestHeaders.Accept.Clear();


        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        //            client.DefaultRequestHeaders.Add(KMM_AUTHORIZATION_KEY, KMM_AUTHORIZATION_VALUE);

        //            JsonSerializerOptions options = new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            };

        //            using (var httpRequestMensage = new HttpRequestMessage(method, endPoint))
        //            {
        //                if (obj != null)
        //                {
        //                    if (!obj.ToString().Contains("xml"))
        //                    {
        //                        var json1 = JsonSerializer.Serialize(obj);
        //                        var stringContent = new StringContent(json1, Encoding.UTF8, "application/json");

        //                        if (!endPoint.Contains("invoices/queues/read"))
        //                            httpRequestMensage.Content = stringContent;

        //                    }
        //                    else
        //                    {
        //                        var stringContent = new StringContent(obj.ToString(), Encoding.UTF8, "application/xml");
        //                        httpRequestMensage.Content = stringContent;
        //                    }
        //                }

        //                var response = await client.SendAsync(httpRequestMensage);
        //                var content = await response.Content.ReadAsStringAsync();
        //                var retDefault = default(T);
        //                var statusSuccess = $"{HttpStatusCode.Created}|{HttpStatusCode.OK}";

        //                if (!statusSuccess.Contains(response.StatusCode.ToString()))
        //                {
        //                    //_logger.LogError($"responseStatus={response.StatusCode} -  payload={JsonSerializer.Serialize(obj)}");
        //                    //var contentToObject = JsonSerializer.Deserialize<T>(content);
        //                    //if (endPoint == "/v1/orders" && method.Method == "POST")
        //                    //{
        //                    return JsonSerializer.Deserialize<T>(content);
        //                    // }
        //                    // return retDefault;
        //                }
        //                else
        //                {
        //                    if (endPoint == "/v1/orders" && method.Method == "POST")
        //                    {
        //                        return null;
        //                    }
        //                    if (string.IsNullOrEmpty(content) || (endPoint.Contains("products") && method.ToString().Equals("POST")))
        //                    {
        //                        return retDefault;
        //                    }
        //                    // return retDefault;
        //                    var contentToObject = JsonSerializer.Deserialize<T>(content);
        //                    // _logger.LogInformation($"responseStatus={response.StatusCode} - payload={JsonSerializer.Serialize(obj)}");

        //                    return contentToObject;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public async Task<HttpResponseMessage> Call<T>(
       HttpMethod method,
       string endPoint,
       object obj = null,
       string uri = null, string token = null) where T : class
        {
            try
            {
                var clientHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(uri);
                    client.DefaultRequestHeaders.Accept.Clear();


                    //if (endPoint.Contains("consultar"))
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //else
                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));


                    // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    //client.BaseAddress = new Uri(settings.Value.VexpensesUrl);
                    //client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(
                    //    new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", token);

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    using (var httpRequestMensage = new HttpRequestMessage(method, endPoint))
                    {
                        if (obj != null)
                        {
                            if (!obj.ToString().Contains("xml"))
                            {
                                var json1 = JsonSerializer.Serialize(obj);
                                var stringContent = new StringContent(json1, Encoding.UTF8, "application/json");

                                //if (!endPoint.Contains("invoices/queues/read"))
                                httpRequestMensage.Content = stringContent;

                            }
                            else
                            {
                                var stringContent = new StringContent(obj.ToString(), Encoding.UTF8, "application/xml");
                                httpRequestMensage.Content = stringContent;
                            }
                        }

                        var response = await client.SendAsync(httpRequestMensage);
                        var content = await response.Content.ReadAsStringAsync();
                        var retDefault = default(T);
                        var statusSuccess = $"{HttpStatusCode.Created}|{HttpStatusCode.OK}";

                        if (!statusSuccess.Contains(response.StatusCode.ToString()))
                        {
                            _logger.LogError($"responseStatus={response.StatusCode} -  payload={JsonSerializer.Serialize(obj)}");
                            //var contentToObject = JsonSerializer.Deserialize<T>(content);

                            return response;// retDefault;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(content) || (endPoint.Contains("products") && method.ToString().Equals("POST")))
                            {
                                return response;// retDefault;
                            }
                            // return retDefault;
                            var contentToObject = JsonSerializer.Deserialize<T>(content);
                            _logger.LogInformation($"responseStatus={response.StatusCode} - payload={JsonSerializer.Serialize(obj)}");

                            return response;// contentToObject;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> CallLoginKmm<T>(
            HttpMethod method,
            string endPoint,
            object obj = null,
            string uri = null, string apiKey = null) where T : class
        {
            try
            {

                KMM_AUTHORIZATION_VALUE = apiKey;
                var clientHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(uri);
                    client.DefaultRequestHeaders.Accept.Clear();


                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token-Time-Hours", "24");

                    client.DefaultRequestHeaders.Add("apiKey", KMM_AUTHORIZATION_VALUE);

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    using (var httpRequestMensage = new HttpRequestMessage(method, endPoint))
                    {
                        if (obj != null)
                        {
                            if (!obj.ToString().Contains("xml"))
                            {
                                var json1 = JsonSerializer.Serialize(obj);
                                var stringContent = new StringContent(json1, Encoding.UTF8, "application/json");
                                httpRequestMensage.Content = stringContent;
                            }
                            else
                            {
                                var stringContent = new StringContent(obj.ToString(), Encoding.UTF8, "application/xml");
                                httpRequestMensage.Content = stringContent;
                            }
                        }

                        var response = await client.SendAsync(httpRequestMensage);
                        var content = await response.Content.ReadAsStringAsync();
                        var retDefault = default(T);
                        var statusSuccess = $"{HttpStatusCode.Created}|{HttpStatusCode.OK}";

                        if (!statusSuccess.Contains(response.StatusCode.ToString()))
                        {
                            //_logger.LogError($"responseStatus={response.StatusCode} -  payload={JsonSerializer.Serialize(obj)}");
                            var contentToObject = JsonSerializer.Deserialize<T>(content);

                            return contentToObject;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(content))// || (endPoint.Contains("products") && method.ToString().Equals("POST")))
                            {
                                return retDefault;
                            }
                            // return retDefault;
                            var contentToObject = JsonSerializer.Deserialize<T>(content);
                            //_logger.LogInformation($"responseStatus={response.StatusCode} - payload={JsonSerializer.Serialize(obj)}");

                            return contentToObject;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RestResponse<T>> ExecuteRequestAsync<T>(RestClient client, RestRequest request)
    => await client.ExecuteAsync<T>(request);
    }
}
