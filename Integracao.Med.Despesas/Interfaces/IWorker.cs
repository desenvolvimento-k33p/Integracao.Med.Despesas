

using Hangfire;
using Hangfire.Server;


namespace Integracao.Med.Despesas.Interfaces
{
    public interface IWorker
    {
        [DisableConcurrentExecution(1800)]
        Task<bool> ExecuteAsync(PerformContext context);
    }
}
