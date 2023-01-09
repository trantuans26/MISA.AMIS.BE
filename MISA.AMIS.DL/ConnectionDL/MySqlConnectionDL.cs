using MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MISA.AMIS.DL
{
    public class MySqlConnectionDL : IConnectionDL
    {
        #region Field
        private IDbConnection _mySqlConnection;
        #endregion

        #region Method
        public int Execute(string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {
            var numberOfAffectedRows = _mySqlConnection.Execute(storedProcedureName, parameters, null, null, commandType);

            return numberOfAffectedRows;
        }

        public IDbConnection InitConnection(string connectionString)
        {
            _mySqlConnection = new MySqlConnection(DataContext.ConnectionString);

            return _mySqlConnection;
        } 
        #endregion
    }
}
