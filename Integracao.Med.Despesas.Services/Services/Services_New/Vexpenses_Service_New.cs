using AutoMapper;
using Integracao.Med.Despesas.Core.Interfaces;
using Integracao.Med.Despesas.Domain.Configuration;
using Integracao.Med.Despesas.Infra.Interfaces;
using Integracao.Med.Despesas.Models.Vexpenses;
using Integracao.Med.Despesas.Services.Extensions;
using Integracao.Med.Despesas.Services.Models.Vexpenses;
using Integracao.Med.Despesas.Services.Services_New;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using SapServicesSettings = Integracao.Med.Despesas.Domain.Configuration.SapServicesSettings;
using VexpensesServicesSettings = Integracao.Med.Despesas.Domain.Configuration.VexpensesServicesSettings;

namespace Integracao.Med.Despesas.Services
{
    public interface IVexpenses_Service_New
    {
        Task<VexpensesResponse<IEnumerable<Report>>> GetReportsAprovadosNoDiaAnterior(string dtAprov);
        Task<VexpensesResponse<IEnumerable<Report>>> GetReportsAbertosNoDiaAnterior();
        Task<VexpensesResponse<Report>> GetById(int id);
    }
    public class Vexpenses_Service_New : Service_New, IVexpenses_Service_New
    {
      
        private readonly HttpClient _httpClient;
        private readonly ILogger<Vexpenses_Service_New> _logger;
        private readonly IHanaAdapter _hanaAdapter;
        private readonly IHttpAdapter _httpAdapter;
       // private readonly ILoggerRepository _logger;
        private readonly IServiceLayerAdapter _serviceLayerAdapter;
        private readonly IMapper _mapper;
        private readonly ServiceLayer _serviceLayerHttp;
        private readonly SapServicesSettings _SapServicesSettings;
        private readonly VexpensesServicesSettings _VexpensesServicesSettings;
        private readonly IOptions<Configuration> _settings;

        public Vexpenses_Service_New(IHanaAdapter hanaAdapter,
                        IOptions<Configuration> configurations,
                        IHttpAdapter httpAdapter,
                        IServiceLayerAdapter serviceLayerAdapter,
                        IMapper mapper,
                        //ILoggerRepository logger
                        ILogger<Vexpenses_Service_New> logger
                      // ILogger logger,
                      )
        {
            _hanaAdapter = hanaAdapter;
            _httpAdapter = httpAdapter;
            _SapServicesSettings = configurations.Value.SapServicesSettings;
            _VexpensesServicesSettings = configurations.Value.VexpensesServicesSettings;
            _mapper = mapper;
            _serviceLayerAdapter = serviceLayerAdapter;
            _logger = logger;
            _serviceLayerHttp = configurations.Value.ServiceLayer;
           
        }

        public async Task<VexpensesResponse<IEnumerable<Report>>> GetReportsAprovadosNoDiaAnterior(string dtAprov)
        {
            var approval_date = dtAprov == "01/01/0001 00:00:00" ?                  
                 DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) :
                 Convert.ToDateTime(dtAprov).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
               

            HttpResponseMessage response = null;


            // esta eh a chamada oficial
            //response = await _httpAdapter.Call<HttpResponseMessage>(HttpMethod.Get, $"reports/status/APROVADO?include=expenses,user,expenses.apportionment,expenses.expense_type,advance,expenses.costs_center&searchFields=approval_date:{approval_date}", null, _VexpensesServicesSettings.VexpensesUrl, _VexpensesServicesSettings.VexpensesToken);

            // esta eh a chamada para buscar todos aprovados, sem filtro           
            //response = await _httpAdapter.Call<HttpResponseMessage>(HttpMethod.Get, $"reports/status/APROVADO?include=expenses,user,expenses.apportionment,expenses.expense_type,advance,expenses.costs_center", null, _VexpensesServicesSettings.VexpensesUrl, _VexpensesServicesSettings.VexpensesToken);

            //chamada pegando data de aprov como filtro
            response = await _httpAdapter.Call<HttpResponseMessage>(HttpMethod.Get, 
                $"reports/status/APROVADO?include=expenses,user,expenses.apportionment,expenses.expense_type,advance,expenses.costs_center&search=approval_date%3A{approval_date}&searchFields=approval_date%3A%3E%3D&searchJoin=and",
                                                                    null, _VexpensesServicesSettings.VexpensesUrl, _VexpensesServicesSettings.VexpensesToken);


            TratarErrosResponse(response);
            var rt = await DeserializarObjetoResponse<VexpensesResponse<IEnumerable<Report>>>(response);

             _logger.LogInformation("Lista de {num} Reports criados em: {time}", rt.Data.Count(), approval_date);

            return rt;// t;
        }

        public async Task<VexpensesResponse<IEnumerable<Report>>> GetReportsAbertosNoDiaAnterior()
        {
            var created_at = "2023-04-01";//DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); //default D-1
            var response = await _httpClient.GetAsync($"reports/status/ABERTO?include=expenses,user,expenses.apportionment,expenses.expense_type,advance,expenses.costs_center&searchFields=created_at>:{created_at}");

            //var response = await _httpClient.GetAsync($"reports/3440026?include=expenses,user,expenses.apportionment,expenses.expense_type,advance,expenses.costs_center");

            TratarErrosResponse(response);
            var rt = await DeserializarObjetoResponse<VexpensesResponse<IEnumerable<Report>>>(response);

            _logger.LogInformation("Lista de {num} Reports criados em: {time}", rt.Data.Count(), created_at);

            return rt;
        }
        public async Task<VexpensesResponse<Report>> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"reports/{id}?include=expenses,user,expenses.apportionment,expenses.expense_type,advance,expenses.costs_center");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VexpensesResponse<Report>>(response);
        }
    }
}
