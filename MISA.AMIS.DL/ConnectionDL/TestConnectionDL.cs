using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DL.ConnectionDL
{
    public class TestConnectionDL : IConnectionDL
    {
        public int Execute(string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public IDbConnection InitConnection(string connectionString)
        {
            return default;
        }
    }
}
