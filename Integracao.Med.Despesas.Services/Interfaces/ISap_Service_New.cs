using Integracao.Med.Despesas.Models.Sap;
using Integracao.Med.Despesas.Models.Vexpenses;

public interface ISap_Service_New
{
    //Task<bool> Login();
    Task<dynamic> GetDownPayments();
    Task<dynamic> CreateItem(Report reportDTO);
    Task<dynamic> ReprocessarItem(Report reportDTO, FilaSap fila);
    Task Listar();
    Task Processar();
    Task ReProcessarFila();

}
