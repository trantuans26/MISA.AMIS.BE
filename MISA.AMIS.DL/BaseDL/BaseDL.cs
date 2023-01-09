using Dapper;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        /// <summary>
        /// Check trùng mã
        /// </summary>
        /// <returns>Bản ghi có mã nhân viên trùng</returns>
        /// Modified by: TTTuan 5/1/2023
        //public bool CheckDuplicateCode(string? recordCode, Guid? recordID)
        //{
        //    // Chuẩn bị tên stored procedure
        //    string connectionString = DataContext.ConnectionString;
        //    string storedProcedureName = string.Format(AMISResources.Proc_CheckDuplicateCode, typeof(T).Name);

        //    // Chuẩn bị tham số đầu vào
        //    var parameters = new DynamicParameters();
        //    var properties = typeof(T).GetProperties();
        //    foreach (var property in properties)
        //    {
        //        var primaryKeyAttribute = (PrimaryKeyAttribute)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
        //        var codeAttribute = (CodeAttribute)Attribute.GetCustomAttribute(property, typeof(CodeAttribute));
        //        if (primaryKeyAttribute != null)
        //        {
        //            parameters.Add($"${property.Name}", recordID);
        //            break;
        //        }
        //        if (codeAttribute != null)
        //        {
        //            parameters.Add($"${property.Name}", recordCode);
        //            break;
        //        }
        //    }

        //    // Khởi tạo kết nối đến DB
        //    var mySqlConnection = new MySqlConnection(connectionString);

        //    var DuplicateCode = mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

        //    if (DuplicateCode != null)
        //        return true;
        //    return false;
        //}

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <returns></returns>
        /// Modified by: TTTuan 5/1/2023
        //public int DeleteRecordByID(Guid recordID)
        //{
        //    // Khai báo kết nối tới DB
        //    var connectionString = DataContext.ConnectionString;

        //    // Khai báo tên stored procedure
        //    var storedProcedureName = string.Format(AMISResources.Proc_DeleteByID, typeof(T).Name);

        //    // Khai báo kết quả trả về
        //    var numberOfAffectedRows = 0;

        //    // Chuẩn bị tham số đầu vào 
        //    var parameters = new DynamicParameters();
        //    var properties = typeof(T).GetProperties();
        //    foreach (var property in properties)
        //    {
        //        var primaryKeyAttribute = (PrimaryKeyAttribute)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
        //        if (primaryKeyAttribute != null)
        //        {
        //            parameters.Add($"${property.Name}", recordID);
        //            break;
        //        }
        //    }

        //    //Khởi tạo kết nối DB
        //    using (var mySqlConnection = new MySqlConnection(connectionString))
        //    {
        //        // Thực hiện gọi vào DB
        //        numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
        //    }

        //    return numberOfAffectedRows;
        //}

        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi trong bảng</returns>
        /// Modified by: TTTuan 5/1/2023
        public IEnumerable<T> GetAllRecords()
        {
            //khai báo tên stored procedure
            var storedProcedure = String.Format(AMISResources.Proc_GetAll, typeof(T).Name);

            //Khởi tạo kết nối db
            var connectionString = DataContext.ConnectionString;

            IEnumerable<T> records = new List<T>();

            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                // Thực hiện gọi vào db
                records = mySqlConnection.Query<T>(storedProcedure, commandType: System.Data.CommandType.StoredProcedure);
            }

            return records;
        }

        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>bản ghi</returns>
        /// Modified by: TTTuan 5/1/2023
        public T GetRecordByID(Guid recordID)
        {
            // Khai báo kết nối tới DB
            var connectionString = DataContext.ConnectionString;

            // Khai báo tên stored procedure
            var storedProcedureName = string.Format(AMISResources.Proc_GetByID, typeof(T).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"${typeof(T).Name}ID", recordID);

            // Khai báo kết quả trả về
            T record;

            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                // Thực hiện gọi vào DB để chạy procedure
                record = mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return record;
        }
    }
}
