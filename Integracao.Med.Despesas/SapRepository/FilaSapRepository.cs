using Integracao.Med.Despesas.Extensions;
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
    public class FilaSapRepository : BaseRepository
    {
        public FilaSapRepository(SapServicesSettings _sapServicesSettings) : base(_sapServicesSettings)
        {

        }
        public int InserirFilaSap(FilaSap fila)
        {
            var sql = $"insert into \"KP_FILA_VEXPENSES\" values({fila.Id},'{fila.Data.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}')";
            var rt = ExecuteCommand(sql);
            return rt;
        }

        public int RemoverFilaSap(FilaSap fila)
        {
            var sql = $"delete from \"KP_FILA_VEXPENSES\" where \"ID\" = {fila.Id}";
            var rt = ExecuteCommand(sql);
            return rt;
        }

        public IEnumerable<FilaSap> GetNaoIntegrados(DateTime data)
        {
            var sql = $"select \"ID\" from \"KP_FILA_VEXPENSES\" where \"DATA\" = '{data.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}'";
            var dr = ExecuteDataReader(sql);
            var fila = new List<FilaSap>();
            while (dr.Read())
            {
                fila.Add(new FilaSap()
                {
                    Id = Convert.ToInt32(dr["ID"].ToString()),
                    //Data = Convert.ToDateTime(dr["DATA"])
                });
            }
            return fila;
        }
           
    }
}
