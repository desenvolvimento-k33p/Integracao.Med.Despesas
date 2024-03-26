using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Integracao.Med.Despesas.Domain;

namespace Integracao.Med.Despesas.Domain.Configuration
{
    public class Configuration
    {
        public ServiceLayer ServiceLayer { get; set; }
        public HanaDbConnection HanaDbConnection { get; set; }      

        public VexpensesServicesSettings VexpensesServicesSettings { get; set; }
        public SapServicesSettings SapServicesSettings { get; set; }     



    }

    public class VexpensesServicesSettings
    {
        public string VexpensesUrl { get; set; }
        public string VexpensesToken { get; set; }
        public string DataProcessamento { get; set; }
    }

    public class SapServicesSettings
    {
        public string SapUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string CompanyDB { get; set; }
        public string EhAdiantamento { get; set; }
        public string ConnectionString { get; set; }
    }

    public class HanaDbConnection
    {
        public string Server { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
    }

    public class ServiceLayer
    {
        public string SessionId { get; set; }
        public string Uri { get; set; }
        public string CompanyDB { get; set; }
        public string UsernameManager { get; set; }
        public string PasswordManager { get; set; }
        public string Username { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UrlFront { get; set; }
        public int Language { get; set; }
        public string ApprovalUsername { get; set; }
        public string ApprovalPassword { get; set; }
    }
   
}
