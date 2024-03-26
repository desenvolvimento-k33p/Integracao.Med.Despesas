using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Models.Sap
{
    public class SapResponse
    {
        public int DocNum { get; set; }
        public int DocEntry { get; set; }  
        
        public Error error { get; set; }
    }

    public class SapError
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public Message message { get; set; }
    }

    public class Message
    {
        public string lang { get; set; }
        public string value { get; set; }
    }
}
