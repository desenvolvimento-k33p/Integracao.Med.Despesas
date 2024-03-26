using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Core.Models
{
    public class LoginPost
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginPost()
        {

        }
    }
}
