using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Core.Models.Response
{
    public class ServiceLayerResponse
    {
        public string SessionId { get; set; } = "";
        public string Version { get; set; } = "";
        public int SessionTimeout { get; set; } = 0;

        public ServiceLayerResponse()
        {

        }
    }
}
