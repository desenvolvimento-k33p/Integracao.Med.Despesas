using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Models.Vexpenses
{
    public class Expense
    {
        public int payment_method_id { get; set; }
        public decimal? value { get; set; }
        public decimal? converted_value { get; set; }        
        public string title { get; set; }
        public string original_currency_iso { get; set; }
        public decimal? exchange_rate { get; set; }
        public int? user_id { get; set; }
        public bool reimbursable { get; set; }
        public Costscenter costs_center { get; set; }
        public ExpenseType expense_type { get; set; }

        public Apportionment apportionment { get; set; }

        public class Costscenter
        {
            public CostscenterData data { get; set; }
        }
        public class CostscenterData
        {
            public int id { get; set; }
            public int company_group_id { get; set; }
            public string integration_id { get; set; }
        }

        public class ExpenseTypeData
        {
            public int id { get; set; }
            public string description { get; set; }

            public string integration_id { get; set; }
        }

        public class ExpenseType
        {
            public ExpenseTypeData data { get; set; }

        }

        public class Apportionment
        {
            public ApportionmentData[] data { get; set; }
        }
        public class ApportionmentData
        {
            public int id { get; set; }
            public string integration_id { get; set; }
            public int expense_id { get; set; }
            public int reimbursable_company_id { get; set; }
            public string description { get; set; }
            public string percentage { get; set; }
            public bool on { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }
    }
}
