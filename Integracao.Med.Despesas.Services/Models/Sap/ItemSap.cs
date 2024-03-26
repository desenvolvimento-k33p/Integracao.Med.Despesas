using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Models.Sap
{
    public class ItemSap
    {
        //[JsonIgnore]
        //public int? DocNum { get; set; }
        //[JsonIgnore]
        //public int? DocEntry { get; set; }
        public string DocDate { get; set; }
        public string TaxDate { get; set; }
        public string DocDueDate { get; set; }
        public string Comments { get; set; }
        public int? PaymentGroupCode { get; set; }
        public string JournalMemo { get; set; }
        public string CardCode { get; set; }
        public string DownPaymentType { get; set; } = "dptInvoice";
        public int BPL_IDAssignedToInvoice { get; set; } = 1;
        public int DocObjectCode { get; set; } = 18;
        public string U_cod_vexpenses { get; set; } // campo de integracao com o external_id do vexpenses
        public int SequenceCode { get; set; } = -1;
        public List<DocumentLines> DocumentLines { get; set; }
        //public DownPaymentsToDraw DownPaymentsToDraw { get; set; }

        public TaxExtensions TaxExtensions { get; set; }
        public int SequenceSerial { get; set; }
    }
}
