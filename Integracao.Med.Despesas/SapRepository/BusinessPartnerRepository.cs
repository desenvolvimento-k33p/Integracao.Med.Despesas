using Integracao.Med.Despesas.Extensions;
using Integracao.Med.Despesas.Models.Sap;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.SapRepository
{
    public class BusinessPartnerRepository : BaseRepository
    {
        public BusinessPartnerRepository(SapServicesSettings _sapServicesSettings) : base(_sapServicesSettings)
        {
            
        }

        public async Task<BusinessPartner> GetBusinessPartner(string cpf)
        {
            var sql = $"select DISTINCT c.\"CardCode\" from CRD7 c INNER JOIN OCRD a " +
                $"ON a.\"CardCode\" = c.\"CardCode\" WHERE (IFNULL(REPLACE_REGEXPR('([-*/.])' " +
                $"IN c.\"TaxId4\" WITH '' OCCURRENCE ALL ), '') = REPLACE_REGEXPR('([-*/.])' IN '{cpf}' WITH '' OCCURRENCE ALL ) OR " +
                $"IFNULL(REPLACE_REGEXPR('([-*/.])' IN c.\"TaxId0\" WITH '' OCCURRENCE ALL ), '') = REPLACE_REGEXPR('([-*/.])' IN '{cpf}' WITH '' OCCURRENCE ALL)) AND a.\"CardType\" = 'S';"; 
            var dr = ExecuteDataReader(sql);
            var bp = new BusinessPartner();
            while (dr.Read())
            {
                bp.CardCode = dr["CardCode"].ToString();                
            }
            if (string.IsNullOrEmpty(cpf))
            {
                bp.CardCode = "";
            }
            return bp;
        }
    }
}
