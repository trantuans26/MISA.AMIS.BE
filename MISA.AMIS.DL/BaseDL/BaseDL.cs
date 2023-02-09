using Dapper;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
        public bool CheckDuplicateCode(Guid? recordID, string? recordCode)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.CHECK_DUPLICATE_CODE, typeof(T).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"${typeof(T).Name}ID", recordID);
            parameters.Add($"${typeof(T).Name}Code", recordCode);

            var DuplicateCode = default(bool);

            // Khởi tạo kết nối đến DB
            using (var connection = _connectionDL.InitConnection(connectionString))
            {
                DuplicateCode = _connectionDL.QueryFirstOrDefault<bool>(connection, storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return DuplicateCode;
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

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Thực hiện gọi vào DB
                        numberOfAffectedRows = _connectionDL.ExecuteUsingTransaction(connection, storedProcedureName, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);

                        if (numberOfAffectedRows == 1)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            numberOfAffectedRows = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                        transaction.Rollback();
                        numberOfAffectedRows = 0;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return numberOfAffectedRows;
        }

        /// <summary>
        /// Xoá nhiều bản ghi
        /// </summary>
        /// <param name="recordIDs"></param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Modified by: TTTuan 5/1/2023
        public int DeleteRecordsByIDs(string recordIDs)
        {
            // Khai báo kết nối tới DB
            var connectionString = DataContext.ConnectionString;

            // Khai báo tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.DELETE_BY_IDS, typeof(T).Name);

            // Khai báo kết quả trả về
            var numberOfAffectedRows = 0;

            // Chuẩn bị tham số đầu vào 
            var parameters = new DynamicParameters();
            parameters.Add($"${typeof(T).Name}IDs", recordIDs);

            //Khởi tạo kết nối DB
            using (var connection = _connectionDL.InitConnection(connectionString))
            {

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Thực hiện gọi vào DB
                        numberOfAffectedRows = _connectionDL.ExecuteUsingTransaction(connection, storedProcedureName, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);

                        if (numberOfAffectedRows > 0)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            numberOfAffectedRows = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                        transaction.Rollback();
                        numberOfAffectedRows = 0;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
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

            // Chuẩn bị tham số đầu
            var parameters = new DynamicParameters();

            // Khai báo kết quả trả về
            var records = default(IEnumerable<T>);

            // Khởi tạo kết nối đến DB
            using (var connection = _connectionDL.InitConnection(connectionString))
            {
                // Thực hiện gọi vào db
                records = _connectionDL.Query<T>(connection, storedProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure);
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
        /// Lấy danh sách thông tin bản ghi theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Mã bản ghi, tên bản ghi, số điện thoại</param>
        /// <param name="pageSize">Số bản ghi muốn lấy</param>
        /// <param name="pageNumber">Số chỉ mục của trang muốn lấy</param>
        /// <returns>Danh sách thông tin bản ghi & tổng số trang và tổng số bản ghi</returns>
        /// Created by: TTTuan (23/12/2022)
        public PagingResult<T> GetRecordsByFilter(string? keyword, int pageSize, int pageNumber)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.GET_BY_FILTER, typeof(T).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("$Keyword", keyword); ;
            parameters.Add("$PageSize", pageSize);
            parameters.Add("$PageNumber", pageNumber);

            // Khai báo kết quả trả về
            var count = 0;
            var list = new List<T>();
            var totalPage = 0;

            // Khởi tạo kết nối đến DB
            using (var mySqlConnection = _connectionDL.InitConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                var records = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                count = records.Read<int>().FirstOrDefault();
                list = records.Read<T>().ToList();
                totalPage = (int)Math.Ceiling((double)count / pageSize);
            }

            return new PagingResult<T>
            {
                Data = list,
                TotalRecord = count,
                TotalPage = totalPage
            };
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

                var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));

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
