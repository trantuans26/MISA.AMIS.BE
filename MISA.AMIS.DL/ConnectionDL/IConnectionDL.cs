
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DL
{ 
    public interface IConnectionDL
    {
        int Execute(string storedProcedureName, DynamicParameters parameters, CommandType commandType);

        IDbConnection InitConnection(string connectionString);
    }
}
