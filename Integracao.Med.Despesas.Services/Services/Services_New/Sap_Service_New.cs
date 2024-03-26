using AutoMapper;
using Integracao.Med.Despesas.Core.Adapters;
using Integracao.Med.Despesas.Core.Interfaces;
using Integracao.Med.Despesas.Domain.Configuration;
using Integracao.Med.Despesas.Domain.Logger;
using Integracao.Med.Despesas.Infra.Interfaces;
using Integracao.Med.Despesas.Models;
using Integracao.Med.Despesas.Models.Sap;
using Integracao.Med.Despesas.Models.Vexpenses;
using Integracao.Med.Despesas.SapRepository;
using Integracao.Med.Despesas.Services.Extensions;
using Integracao.Med.Despesas.Services.SQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;


namespace Integracao.Med.Despesas.Services.Services_New
{

    public class Sap_Service_New : Service_New, ISap_Service_New
    {
        private readonly ILogger<Sap_Service_New> _logger;
        private readonly HttpClient _httpClient;
        public readonly FilaSapRepository _filaSapRepository;
        public readonly LogSapRepository _logSapRepository;
        private readonly BusinessPartnerRepository _businessPartnerRepository;
        private readonly SapRepository.SapRepository _sapRepository;
        private readonly IVexpenses_Service_New _vexpensesService;
        private readonly IHanaAdapter _hanaAdapter;
        private readonly IHttpAdapter _httpAdapter;
        //private readonly ILoggerRepository _logger;
        private readonly IServiceLayerAdapter _serviceLayerAdapter;
        private readonly IMapper _mapper;
        private readonly ServiceLayer _serviceLayerHttp;
        private readonly IOptions<Configuration> _configuration;
        private readonly IOptions<Configuration> _settings;
        private readonly Domain.Configuration.SapServicesSettings _SapServicesSettings;
        public Sap_Service_New(IHanaAdapter hanaAdapter,
                           IOptions<Configuration> configurations,
                           IHttpAdapter httpAdapter,
                           IServiceLayerAdapter serviceLayerAdapter,
                           IMapper mapper,
                           //ILoggerRepository logger,
                           ILogger<Sap_Service_New> logger,
                          // ILogger logger,
                          IVexpenses_Service_New vexpensesService)
        {
            _hanaAdapter = hanaAdapter;
            _httpAdapter = httpAdapter;
            //_logger = logger;
            _mapper = mapper;
            _serviceLayerAdapter = serviceLayerAdapter;
            _logger = logger;
            _settings = configurations;
            _serviceLayerHttp = configurations.Value.ServiceLayer;
            _vexpensesService = vexpensesService;
            _SapServicesSettings = configurations.Value.SapServicesSettings;
            _filaSapRepository = new FilaSapRepository(_SapServicesSettings);
            _businessPartnerRepository = new BusinessPartnerRepository(_SapServicesSettings);
            _sapRepository = new SapRepository.SapRepository(_SapServicesSettings);
            _logSapRepository = new LogSapRepository(_SapServicesSettings);
        }




        public async Task<dynamic> GetDownPayments()
        {
            var response = await _httpClient.GetAsync("DownPayments");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<dynamic>(response);
        }

        public async Task<dynamic> CreateItem(Report reportDTO)
        {


            //inserir na Fila
            _logger.LogInformation($"---------- Vexpenses de número {reportDTO.id} ");

            var fila = new FilaSap()
            {
                Id = reportDTO.id,
                Data = DateTime.Now.AddDays(-1)
            };

            _filaSapRepository.InserirFilaSap(fila);

            _logger.LogInformation($"Inserido na fila de processamento ");

            return await ProcessarItem(reportDTO, fila);
        }

        public async Task<dynamic> ProcessarItem(Report reportDTO, FilaSap fila)
        {
            string moeda = "";
            double valRembolso = 0, val = 0;

            var itemSap = new ItemSap();

            itemSap.DocDate = DateTime.Now.ToString("yyyy-MM-dd");

            var dtVenc = await _retornaDtVenc();
            // data corrente
            itemSap.DocDueDate = Convert.ToDateTime(dtVenc).ToString("yyyy-MM-dd");// DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");    // data corrente + 1



            itemSap.TaxDate = reportDTO.approval_date.Substring(0, 10);

            if (reportDTO.expenses.data.Any(it => it.expense_type.data.integration_id == _settings.Value.SapServicesSettings.EhAdiantamento))// dentro do expense_type - > integration_id -> pegar o valor            
                itemSap.DocObjectCode = 204;

            itemSap.PaymentGroupCode = reportDTO.payment_method_id;
            itemSap.JournalMemo = reportDTO.observation;
            itemSap.U_cod_vexpenses = reportDTO.id.ToString(); // campo de controle do que foi ou n integrado
            itemSap.SequenceSerial = reportDTO.id;
            itemSap.DocumentLines = new List<DocumentLines>();
            itemSap.TaxExtensions = new TaxExtensions();

            _logger.LogInformation("GetBusinessPartner {cpf}", reportDTO.user.data.cpf);

            var bp = await _businessPartnerRepository.GetBusinessPartner(reportDTO.user.data.cpf);

            _logger.LogInformation("CardCode {CardCode}", bp.CardCode);

            TaxExtensions tax = new TaxExtensions();

            //bool reembolso = false;
            //bool prest = false;

            foreach (var item in reportDTO.expenses.data)
            {
                itemSap.CardCode = bp.CardCode;

                _logger.LogInformation("GetItemCode {expense_type_id} {expense_name} {cod_expense} {cost_center}",
                    item.expense_type.data.id,
                    item.expense_type.data.description,
                    item.expense_type.data.integration_id,
                    item.costs_center.data.integration_id);

                var _itemCode = await _sapRepository.GetItemCode(item.expense_type.data.integration_id,
                    item.costs_center.data.integration_id);
                //_logger.LogInformation("ItemCodeSap {_itemCode}", _itemCode);

                var i = new DocumentLines()
                {
                    ItemCode = _itemCode,
                    Price = item.original_currency_iso == "BRL" ? item.value : item.converted_value,
                    CostingCode = item.costs_center.data.integration_id,
                    FreeText = item.title,
                    ProjectCode = item.apportionment != null ? item.apportionment.data[0].integration_id : "",
                    Usage = item.reimbursable == true ? 31 : 61
                };

                //if (item.reimbursable) { reembolso = true; }
                //else { prest = true; }

                moeda = item.original_currency_iso;
                if (item.reimbursable) { valRembolso = valRembolso + Convert.ToDouble(item.value); }
                else { val = val + Convert.ToDouble(item.value); }

                itemSap.DocumentLines.Add(i);
            }

            itemSap.Comments = $"Relatório VExpenses Nº {reportDTO.id}, aprovado em {reportDTO.approval_date}, " +
                $"Moeda {moeda}, Valor Reembolsável {valRembolso}, Valor NÂO Reembolsável {val}";

            //if (!reembolso && prest) { tax.MainUsage = 61; } else { tax.MainUsage = 31; };

            itemSap.TaxExtensions = tax;

            var itemContent = ObterConteudo(itemSap);
            var response = await _serviceLayerAdapter.Call<HttpResponseMessage>(
                    "PurchaseInvoices", HttpMethod.Post, itemSap, _serviceLayerHttp.Uri);


            var sapResponse = await DeserializarObjetoResponse<SapResponse>(response);

            string json = JsonSerializer.Serialize(itemSap);

            _logger.LogInformation(json);
            //remover da Fila se retorna 201
            if (response.IsSuccessStatusCode)
            {
                _filaSapRepository.RemoverFilaSap(fila);

                var desc = itemSap.DocObjectCode == 204 ? "DownPayments" : "PurchaseInvoices";
                _logger.LogInformation($"Vexpenses de número {reportDTO.id} Criado {desc} de número documento:{sapResponse.DocEntry} - {DateTimeOffset.Now}");
            }
            else
            {
                var jsonResponse = await DeserializarObjetoResponse<SapError>(response);
                _logger.LogError(jsonResponse.error.message.value);

                if (jsonResponse.error.code == -1116)
                {
                    _filaSapRepository.RemoverFilaSap(fila);
                }

            }

            //salvar log
            var log = new LogSap()
            {
                Id = reportDTO.id,
                Mensagem_erro = response.StatusCode.ToString(),
                Request = JsonSerializer.Serialize(reportDTO),
                Response = JsonSerializer.Serialize(response.Content.ReadAsStringAsync()),
                Data = DateTime.Now,
                DocEntry = sapResponse.DocEntry,
                Documento = sapResponse.DocNum.ToString(),
                Integrado = response.IsSuccessStatusCode ? "S" : "N"
            };
            await _logSapRepository.SalvarLog(log);
            return response;
        }
        public async Task<dynamic> ReprocessarItem(Report reportDTO, FilaSap fila)
        {
            _logger.LogInformation($"Tentativa de integração Report {reportDTO.id} - {DateTimeOffset.Now}");
            return await ProcessarItem(reportDTO, fila);

        }
        public async Task Processar()
        {

            bool b = await _validaDatas();

            if (b)
            {
                _logger.LogInformation("Iniciada integração às: {time}", DateTimeOffset.Now);

                var dtAprov = await _retornaDtAprov();

                var response = await _vexpensesService.GetReportsAprovadosNoDiaAnterior(dtAprov);

                //if (dtAprov != "01/01/0001 00:00:00")
                //{
                //    var groupData = response.Data.Where(x => Convert.ToDateTime(x.approval_date).Date == Convert.ToDateTime(dtAprov));
                //    foreach (var item in groupData)
                //    {
                //        //se ja existir doc pula
                //        int ret = await _verificaSeExiste(item);
                //        if (ret > 0)
                //            continue;
                //        //DateTime dt = Convert.ToDateTime(item.approval_date).Date;
                //        var responseIntegration = await CreateItem(item);
                //    }
                //}
                //else
                foreach (var item in response.Data)
                {
                    //se ja existir doc pula
                    int ret = await _verificaSeExiste(item);
                    if (ret > 0)
                        continue;
                    var responseIntegration = await CreateItem(item);
                }
            }

            _logger.LogInformation("Fim do processamento: {time}", DateTimeOffset.Now);
        }

        public async Task Listar()
        {
            var response = await _vexpensesService.GetReportsAbertosNoDiaAnterior();

            foreach (var item in response.Data)
            {
                foreach (var item2 in item.expenses.data)
                {
                    _logger.LogInformation("CPF;{cpf};Item;{item};{item2};{item3}", item.user.data.cpf, item2.expense_type.data.integration_id, item2.expense_type.data.description, item2.costs_center.data.integration_id);
                }
            }
            _logger.LogInformation("Fim do processamento: {time}", DateTimeOffset.Now);
        }

        public async Task ReProcessarFila()
        {
            _logger.LogInformation("Reprocessamento da fila às: {time}", DateTimeOffset.Now);

            var fila = _filaSapRepository.GetNaoIntegrados(DateTime.Now.AddDays(-2));

            foreach (var item in fila)
            {
                _logger.LogInformation("Reprocessamento Vexpenses Report {id}", item.Id);
                var report = await _vexpensesService.GetById(item.Id);
                var responseIntegration = await ReprocessarItem(report.Data, item);
            }
            _logger.LogInformation("Fim do reprocessamento da fila: {time}", DateTimeOffset.Now);
        }

        private async Task<bool> _validaDatas()
        {
            bool ret = true;

            var query = SQLSupport.GetConsultas("validaDatas");
            query = String.Format(query, _settings.Value.SapServicesSettings.CompanyDB);

            var retorno = _hanaAdapter.ExecuteSinc(query);
            DateTime data = Convert.ToDateTime(retorno);

            if (DateTime.Now >= data && DateTime.Today == data.Date)
                ret = true;
            else
                ret = false;



            return ret;
        }

        private async Task<int> _verificaSeExiste(Report dados)
        {

            var query = SQLSupport.GetConsultas("verificaSeExiste");
            query = String.Format(query, _settings.Value.SapServicesSettings.CompanyDB, dados.id);

            var retorno = _hanaAdapter.ExecuteSinc(query);
            var data = Convert.ToInt32(retorno.ToString());

            return data;
        }

        private async Task<string> _retornaDtVenc()
        {
            var query = SQLSupport.GetConsultas("retornaDtVenc");
            query = String.Format(query, _settings.Value.SapServicesSettings.CompanyDB);

            var retorno = _hanaAdapter.ExecuteSinc(query);
            var data = retorno.ToString();


            return data;
        }

        private async Task<string> _retornaDtAprov()
        {
            var query = SQLSupport.GetConsultas("retornaDtAprov");
            query = String.Format(query, _settings.Value.SapServicesSettings.CompanyDB);

            var retorno = _hanaAdapter.ExecuteSinc(query);
            var data = retorno.ToString();


            return data;
        }

    }
}