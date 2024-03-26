using Integracao.Med.Despesas.Models.Vexpenses;

public interface IVexpensesService
{
    Task<VexpensesResponse<IEnumerable<Report>>> GetReportsAprovadosNoDiaAnterior();
    Task<VexpensesResponse<IEnumerable<Report>>> GetReportsAbertosNoDiaAnterior();
    Task<VexpensesResponse<Report>> GetById(int id);
}
