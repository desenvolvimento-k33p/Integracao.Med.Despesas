
using Integracao.Med.Despesas.Domain.Configuration;
using Integracao.Med.Despesas.Models.Sap;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.SapRepository
{
    public class LogSapRepository : BaseRepository
    {
        public LogSapRepository(SapServicesSettings _sapServicesSettings) : base(_sapServicesSettings)
        {
            
        }
        public async Task<int> SalvarLog(LogSap log)
        {
            var sql = $"insert into \"KP_INT_VEXPENSES\" " +
                $"values({log.Id},'{log.Mensagem_erro}','{log.Request}','{log.Data.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}','{log.Response}','{log.Integrado}',{log.DocEntry},'{log.Documento}')";
            var rt = ExecuteCommand(sql);
            return rt;
        }
    }
}
