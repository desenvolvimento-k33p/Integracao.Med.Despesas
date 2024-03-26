using Integracao.Med.Despesas.Extensions;
using Integracao.Med.Despesas.Models.Sap;
using Integracao.Med.Despesas.Models.Vexpenses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.SapRepository
{
    public class SapRepository : BaseRepository
    {
        public SapRepository(SapServicesSettings _sapServicesSettings) : base(_sapServicesSettings)
        {
            
        }
        public async Task<string> GetItemCode(string u_cod_vexpenses, string prccode)
        {
            var sql = $"select \"ItemCode\" from OITM where \"U_cod_vexpenses\" = '{u_cod_vexpenses}' and " +
                 $"\"U_CategoriaItem\" = (select \"U_VAR_GroupMask\" from OPRC WHERE \"PrcCode\" ='{prccode}')";
            var dr = ExecuteDataReader(sql);
            string itemCode = null;
            while (dr.Read())
            {
                itemCode = dr["ItemCode"].ToString();                
            }
            return itemCode;
        }

        public async Task<dynamic> GetDocEntryAdiantamento(string u_cod_vexpenses)
        {
            var sql = $"select \"DocEntry\", \"DocNum\" from ODPO where \"U_cod_vexpenses\" = '{u_cod_vexpenses}'";
            var dr = ExecuteDataReader(sql);
            int docEntry = 0;
            string docNum = "";
            while (dr.Read())
            {
                docEntry = Convert.ToInt32(dr["DocEntry"]);
                docNum = Convert.ToString(dr["DocNum"]);
            }
            return new { docEntry, docNum };
        }

       
    }
}
