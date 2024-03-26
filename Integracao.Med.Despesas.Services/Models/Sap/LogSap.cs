namespace Integracao.Med.Despesas.Models.Sap
{
    public class LogSap
    {
        public int Id { get; set; }
        public string Mensagem_erro { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime Data { get; set; }
        public int DocEntry { get; set; }
        public string Documento { get; set; }
        public string Integrado { get; set; }
    }
}
