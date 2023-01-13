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
        /// <summary>
        /// Thực thi truy vấn được tham số hoá
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        public int Execute(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {
            return cnn.Execute(storedProcedureName, parameters, null, null, commandType);
        }

        /// <summary>
        /// Khởi tạo kết nối
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>Kết nối tới database</returns>
        public IDbConnection InitConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Thực thi truy vấn dòng đơn
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>Một chuỗi dữ liệu của loại được cung cấp</returns>
        public T QueryFirstOrDefault<T>(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {
            return cnn.QueryFirstOrDefault<T>(storedProcedureName, parameters, null, null, commandType);

        }
    }
}
