using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Services.Extensions
{
    public class SapServicesSettings
    {
        public string SapUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CompanyDB { get; set; }
        public string ConnectionString { get; set; }
        public string EhAdiantamento { get; set; }

        public SapServicesSettings() { }

        public SapServicesSettings(string _userName,string _password, string _companyDB,string _connectionString, string _ehAdiantamento) 
        { 
            UserName = _userName;
            Password = _password;
            CompanyDB = _companyDB;
            ConnectionString = _connectionString;
            EhAdiantamento = _ehAdiantamento;
        }
    }
}
