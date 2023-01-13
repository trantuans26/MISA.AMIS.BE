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
        #region Field
        private readonly IConnectionDL _connectionDL;
        #endregion

        #region Constructor
        public BaseDL(IConnectionDL connectionDL)
        {
            _connectionDL = connectionDL;
        }
        #endregion

        /// <summary>
        /// Kiểm tra mã trùng
        /// </summary>
        /// <param name="recordCode"></param>
        /// <param name="recordID"></param>
        /// <returns>bool kiểm tra có trùng hay không</returns>
        /// Modified by: TTTuan 5/1/2023
        public bool CheckDuplicateCode(string? recordCode, Guid? recordID)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.CHECK_DUPLICATE_CODE, typeof(T).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"${typeof(T).Name}ID", recordID);
            parameters.Add($"${typeof(T).Name}Code", recordCode);

            var DuplicateCode = default(T);

            // Khởi tạo kết nối đến DB
            using (var connection = _connectionDL.InitConnection(connectionString))
            {
                DuplicateCode = _connectionDL.QueryFirstOrDefault<T>(connection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return DuplicateCode != null;
        }

        /// <summary>
        /// Xoá 1 bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Modified by: TTTuan 5/1/2023
        public int DeleteRecordByID(Guid recordID)
        {
            // Khai báo kết nối tới DB
            var connectionString = DataContext.ConnectionString;

            // Khai báo tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.DELETE_BY_ID, typeof(T).Name);

            // Khai báo kết quả trả về
            var numberOfAffectedRows = 0;

            // Chuẩn bị tham số đầu vào 
            var parameters = new DynamicParameters();
            parameters.Add($"${typeof(T).Name}ID", recordID);

            //Khởi tạo kết nối DB
            using (var connection = _connectionDL.InitConnection(connectionString))
            {
                // Thực hiện gọi vào DB
                numberOfAffectedRows = _connectionDL.Execute(connection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return numberOfAffectedRows;
        }

        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi trong bảng</returns>
        /// Modified by: TTTuan 5/1/2023
        public IEnumerable<T> GetAllRecords()
        {
            //khai báo tên stored procedure
            var storedProcedure = String.Format(ProcedureNames.GET_ALL, typeof(T).Name);

            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Khai báo kết quả trả về
            var records = default(IEnumerable<T>);

            // Khởi tạo kết nối đến DB
            using (var connection = new MySqlConnection(connectionString))
            {
                // Thực hiện gọi vào db
                records = connection.Query<T>(storedProcedure, commandType: System.Data.CommandType.StoredProcedure);
            }

            return records;
        }

        /// <summary>
        /// API Lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// Modified by: TTTuan (23/12/2022)
        public string GetNewCode()
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.GET_NEW_CODE, typeof(T).Name);

            // Chuẩn bị tham số đầu
            var parameters = new DynamicParameters();

            // Khai báo kết quả trả về
            var newCode = default(string);

            // Khởi tạo kết nối đến DB
            using (var connection = _connectionDL.InitConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                newCode = _connectionDL.QueryFirstOrDefault<string>(connection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return newCode;
        }

        /// <summary>
        /// API lấy thông tin chi tiết của 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID của bản ghi</param>
        /// <returns>Thông tin của bản ghi theo ID</returns>
        /// Modified by: TTTuan 5/1/2023
        public T GetRecordByID(Guid recordID)
        {
            // Khai báo kết nối tới DB
            var connectionString = DataContext.ConnectionString;

            // Khai báo tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.GET_BY_ID, typeof(T).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"${typeof(T).Name}ID", recordID);

            // Khai báo kết quả trả về
            var record = default(T);

            using (var connection = _connectionDL.InitConnection(connectionString))
            {
                // Thực hiện gọi vào DB để chạy procedure
                record = _connectionDL.QueryFirstOrDefault<T>(connection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return record;
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns>Trả về số dòng bị ảnh hưởng</returns>
        /// Modified by: TTTuan 5/1/2023
        public int InsertRecord(T newRecord)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.INSERT, typeof(T).Name);

            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();

            var newID = Guid.NewGuid();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;

                object? propertyValue;

                var primaryKeyAttribute = (PrimaryKeyAttribute?) Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));

                if (primaryKeyAttribute != null)
                {
                    propertyValue = newID;
                }
                else
                {
                    propertyValue = property.GetValue(newRecord, null);
                }
                parameters.Add($"${propertyName}", propertyValue);
            }

            var numberOfAffectedRows = 0;

            // Khởi tạo kết nối đến DB
            using (var connection = _connectionDL.InitConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                numberOfAffectedRows = _connectionDL.Execute(connection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return numberOfAffectedRows;
        }

        /// <summary>
        /// Sửa một bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="record"></param>
        /// <returns>Trả về số dòng bị ảnh hưởng</returns>
        /// Modified by: TTTuan 5/1/2023
        public int UpdateRecordByID(Guid recordID, T record)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(AMISResources.Proc_UpdateByID, typeof(T).Name);

            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;

                object? propertyValue;

                var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    propertyValue = recordID;
                }
                else
                {
                    propertyValue = property.GetValue(record, null);
                }
                parameters.Add($"${propertyName}", propertyValue);
            }

            var numberOfAffectedRows = 0;

            // Khởi tạo kết nối đến DB
            using (var connection = _connectionDL.InitConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                numberOfAffectedRows = _connectionDL.Execute(connection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return numberOfAffectedRows;
        }
    }
}
