using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Integracao.Med.Despesas.Models.Vexpenses
{
    public class Report
    {
        public int id { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public int? payment_method_id { get; set; }
        public string observation { get; set; }
        public string external_id { get; set; }
        public string approval_date { get; set; }
        public Data expenses { get; set; }
        public User user { get; set; }
    }

    public class Data
    {
        public IEnumerable<Expense> data { get; set; }
    }

    public class User
    {
        public UserData data { get; set; }
    }

    public class UserData
    {
        public string name { get; set; }
        public string email { get; set; }
        public string cpf { get; set; }
    }

    

   

}
