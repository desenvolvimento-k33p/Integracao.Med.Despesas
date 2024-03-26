
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integracao.Med.Despesas.Domain.Configuration;
//using Integracao.Med.Despesas.Services.Extensions;
using Microsoft.Extensions.Options;
using Sap.Data.Hana;

namespace Integracao.Med.Despesas.SapRepository
{
    public class BaseRepository
    {
        private readonly SapServicesSettings _sapServicesSettings;

        protected HanaConnection _connection;

        public BaseRepository(SapServicesSettings sapServicesSettings)
        {
            _sapServicesSettings = sapServicesSettings;
            _connection = new HanaConnection(_sapServicesSettings.ConnectionString);
        }

        public bool ConectaBanco()
        {
            //Verifica se a conexão está aberta
            if (_connection.State != ConnectionState.Open)
            {
                //Abre conexão
                try
                {
                    _connection.ConnectionString = _sapServicesSettings.ConnectionString;
                    _connection.Open();

                    return _connection.State == ConnectionState.Open;

                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        public HanaDataReader ExecuteDataReader(string sql)
        {
            HanaCommand cmd = null;

            if (ConectaBanco())
            {
                cmd = new HanaCommand(sql, _connection);
            }
            

            try
            {
                return cmd.ExecuteReader();
            }
            catch (HanaException e)
            {
                throw e;
            }
        }



        public int ExecuteCommand(string sql)
        {
            HanaCommand cmd = null;

            if (ConectaBanco())
            {
                cmd = new HanaCommand(sql, _connection);
            }

            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (HanaException e)
            {
                throw e;
            }

        }
    }
}
