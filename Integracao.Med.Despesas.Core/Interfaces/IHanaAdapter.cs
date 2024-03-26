using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Core.Interfaces
{
    public interface IHanaAdapter
    {
        Task<T> QueryFirst<T>(string sql);
        Task<IEnumerable<T>> Query<T>(string sql);

        Task<List<T>> QueryList<T>(string sql);
        Task<int> Execute(string sql);

        object ExecuteSinc(string sql);
    }
}
