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
using System.Transactions;
using static Dapper.SqlMapper;

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
        /// Thực thi truy vấn được tham số hoá có transaction
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="transaction"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int ExecuteUsingTransaction(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, IDbTransaction transaction, CommandType commandType)
        {
            return cnn.Execute(storedProcedureName, parameters, transaction: transaction, null, commandType);
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
        /// Thực hiện một truy vấn, trả về dữ liệu được nhập dưới dạng <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>Một chuỗi dữ liệu của loại được cung cấp</returns>
        public IEnumerable<T> Query<T>(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {
            return cnn.Query<T>(storedProcedureName, parameters, null, true, null, commandType);
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

        /// <summary>
        /// Thực thi một lệnh trả về nhiều tập hợp kết quả và truy cập lần lượt từng tập hợp.
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>Một chuỗi dữ liệu của loại được cung cấp</returns>
        public GridReader QueryMultiple(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Thực thi một lệnh trả về nhiều tập hợp kết quả và truy cập lần lượt từng tập hợp.
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>Một chuỗi dữ liệu của loại được cung cấp</returns>
        //public Dapper.SqlMapper.GridReader QueryMultiple(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType)
        //{
        //    cnn.QueryMultiple(storedProcedureName, parameters, null, null, commandType);
        //}
    }
}
