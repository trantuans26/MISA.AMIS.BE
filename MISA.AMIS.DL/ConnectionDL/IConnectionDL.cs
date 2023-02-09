
using Dapper;
using MySqlConnector;
using MISA.AMIS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.DL
{ 
    public interface IConnectionDL
    {

        /// <summary>
        /// Thực thi truy vấn được tham số hoá
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        int Execute(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType);

        /// <summary>
        /// Thực thi truy vấn được tham số hoá có transaction
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="transaction"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int ExecuteUsingTransaction(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, IDbTransaction transaction, CommandType commandType);

        /// <summary>
        /// Khởi tạo kết nối
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>Kết nối tới database</returns>
        IDbConnection InitConnection(string connectionString);

        /// <summary>
        /// Thực thi truy vấn dòng đơn
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>Một chuỗi dữ liệu của loại được cung cấp</returns>
        T QueryFirstOrDefault<T>(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType);

        /// <summary>
        /// Thực hiện một truy vấn, trả về dữ liệu được nhập dưới dạng <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>Một chuỗi dữ liệu của loại được cung cấp</returns>
        IEnumerable<T> Query<T>(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType);

        /// <summary>
        /// Thực thi một lệnh trả về nhiều tập hợp kết quả và truy cập lần lượt từng tập hợp.
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns>Một chuỗi dữ liệu của loại được cung cấp</returns>
        GridReader QueryMultiple(IDbConnection cnn, string storedProcedureName, DynamicParameters parameters, CommandType commandType);
    }
}
