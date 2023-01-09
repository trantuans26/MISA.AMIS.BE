using MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MISA.AMIS.Common;

namespace MISA.AMIS.DL
{
    public class MySqlConnectionDL : IConnectionDL
    {
        public int Execute(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {
            return cnn.Execute(storedProcedureName, parameters, null, null, commandType);
        }

        public IDbConnection InitConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public T QueryFirstOrDefault<T>(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {
            return cnn.QueryFirstOrDefault<T>(storedProcedureName, parameters, null, null, commandType);

        }
    }
}
