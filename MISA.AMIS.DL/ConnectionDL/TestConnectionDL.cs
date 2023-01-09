using Dapper;
using MISA.AMIS.Common;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DL
{
    public class TestConnectionDL : IConnectionDL
    {
        public int Execute(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {
            return 1;
        }

        public IDbConnection InitConnection(string connectionString)
        {
            return new MySqlConnection();
        }

        public T QueryFirstOrDefault<T>(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {

            return default;
        }
    }
}
