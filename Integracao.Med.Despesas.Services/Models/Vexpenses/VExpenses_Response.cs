using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Services.Models.Vexpenses
{
   

    public class VExpenses_Response
    {
        public string request { get; set; }
        public string method { get; set; }
        public bool success { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public object Data { get; set; }

    }
}
