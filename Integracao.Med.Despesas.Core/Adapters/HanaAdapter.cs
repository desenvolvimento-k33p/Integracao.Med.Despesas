using Dapper;
using Integracao.Med.Despesas.Core.Interfaces;
using Integracao.Med.Despesas.Core.TypeHandler;
using Integracao.Med.Despesas.Domain.Configuration;
using Microsoft.Extensions.Options;
using Sap.Data.Hana;

namespace Integracao.Med.Despesas.Core.Adapters
{
    public class HanaAdapter : IHanaAdapter
    {
        private readonly string _urlConnection;

        public HanaAdapter(IOptions<Configuration> configurations)
        {

            HanaDbConnection cfgFile = configurations.Value.HanaDbConnection;
            _urlConnection = $"Server={cfgFile.Server};UserID={cfgFile.UserID};Password={cfgFile.Password};CS={cfgFile.Database}";
        }

        public async Task<T> QueryFirst<T>(string sql)
        {
            using (var _connection = new HanaConnection(_urlConnection))
            {
                try
                {
                    var result = await _connection.QueryFirstOrDefaultAsync<T>(sql);

                    return result;
                }
                catch (Exception e)
                {
                    throw new Exception("HanaAdapter QueryFirst", e);
                }
            }
        }

        public async Task<IEnumerable<T>> Query<T>(string sql)
        {
            using (var _connection = new HanaConnection(_urlConnection))
            {
                try
                {
                    SqlMapper.AddTypeHandler(new DecimalTypeHandler());
                    var result = await _connection.QueryAsync<T>(sql);
                    return result;
                }
                catch (Exception e)
                {
                    throw new Exception("HanaAdapter Query", e);
                }
            }
        }

        public async Task<List<T>> QueryList<T>(string sql)
        {
            using (var _connection = new HanaConnection(_urlConnection))
            {
                try
                {
                    SqlMapper.AddTypeHandler(new DecimalTypeHandler());
                    var result = await _connection.QueryAsync<T>(sql);

                    return result.ToList();
                }
                catch (Exception e)
                {
                    throw new Exception("HanaAdapter QueryList", e);
                }
            }
        }

        public async Task<int> Execute(string sql)
        {
            using (var _connection = new HanaConnection(_urlConnection))
            {
                try
                {
                    var result = await _connection.ExecuteAsync(sql);
                    return result;
                }

                catch (Exception e)
                {
                    throw new Exception("HanaAdapter Execute", e);
                }
            }
        }

        public object ExecuteSinc(string sql)
        {
            using (var _connection = new HanaConnection(_urlConnection))
            {
                try
                {
                    var result = _connection.ExecuteScalar(sql);
                    return result;
                }

                catch (Exception e)
                {
                    throw new Exception("HanaAdapter Execute", e);
                }
            }
        }

       
    }
}
