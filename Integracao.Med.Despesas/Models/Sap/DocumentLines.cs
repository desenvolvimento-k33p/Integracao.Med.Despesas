namespace Integracao.Med.Despesas.Models.Sap
{
    public class DocumentLines
    {
        public string ItemCode { get; set; }
        public decimal? Price { get; set; }
        public int Usage { get; set; } // 61 prestação de contas // 31 - reembolso
        public string CostingCode { get; set; }
        public string FreeText { get; set; }

        public string? ProjectCode { get; set; }
    }
}
