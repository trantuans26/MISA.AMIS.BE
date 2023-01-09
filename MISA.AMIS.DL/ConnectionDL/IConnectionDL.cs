
using Dapper;
using MISA.AMIS.Common;
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
        int Execute(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType);

        IDbConnection InitConnection(string connectionString);

        T QueryFirstOrDefault<T>(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType);
    }
}
