using System.Collections.Generic;

namespace Integracao.Med.Despesas.Models.Vexpenses
{
    public class VexpensesResponse<T> where T : class
    {
        public string request { get; set; }
        public string method { get; set; }
        public bool success { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public T Data { get; set; }

    }

}