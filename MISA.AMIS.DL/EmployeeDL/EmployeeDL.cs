using Dapper;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        #region Field
        private readonly IConnectionDL _connectionDL;
        #endregion

        #region Constructor
        public EmployeeDL(IConnectionDL connectionDL)
        {
            _connectionDL = connectionDL;
        }
        #endregion
        /// <summary>
        /// API kiểm tra mã trùng
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeID"></param>
        /// <returns> Mã nhân viên trùng </returns>
        /// Created by: TTTuan (23/12/2022)
        public bool CheckDuplicateCode(string? employeeCode, Guid? employeeID)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(AMISResources.Proc_CheckDuplicateCode, typeof(Employee).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"${typeof(Employee).Name}ID", employeeID);
            parameters.Add($"${typeof(Employee).Name}Code", employeeCode);

            // Khởi tạo kết nối đến DB
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                var DuplicateCode = mySqlConnection.QueryFirstOrDefault<Employee>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (DuplicateCode != null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// API Xóa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        /// Created by: TTTuan (23/12/2022)
        public int DeleteEmployeeByID(Guid employeeID)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(AMISResources.Proc_DeleteByID, typeof(Employee).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"${typeof(Employee).Name}ID", employeeID);

            var numberOfAffectedRows = 0;
            // Khởi tạo kết nối đến DB
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return numberOfAffectedRows;
        }

        /// <summary>
        /// API Lấy danh sách thông tin nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Mã nhân viên, tên nhân viên, số điện thoại</param>
        /// <param name="pageSize">Số bản ghi muốn lấy</param>
        /// <param name="pageNumber">Số chỉ mục của trang muốn lấy</param>
        /// <returns>Danh sách thông tin nhân viên & tổng số trang và tổng số bản ghi</returns>
        /// Created by: TTTuan (23/12/2022)
        public PagingResult GetEmployeesByFilter(string? keyword, int pageSize, int pageNumber)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(AMISResources.Proc_GetByFilter, typeof(Employee).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("$Keyword", keyword); ;
            parameters.Add("$PageSize", pageSize);
            parameters.Add("$PageNumber", pageNumber);

            // Khai báo kết quả trả về
            int count;
            var list = new List<Employee>();
            int totalPage;

            // Khởi tạo kết nối đến DB
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                var employees = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                count = employees.Read<int>().FirstOrDefault();
                list = employees.Read<Employee>().ToList();
                totalPage = (int)Math.Ceiling((double)count / pageSize);
            }

            return new PagingResult
            {
                Data = list,
                TotalRecord = count,
                TotalPage = totalPage
            };
        }

        /// <summary>
        /// API Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Modified by: TTTuan (23/12/2022)
        public string GetNewEmployeeCode()
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(AMISResources.Proc_GetNewCode, typeof(Employee).Name);

            var newCode = default(string);

            // Khởi tạo kết nối đến DB
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                newCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
            }

            return newCode;
        }

        /// <summary>
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="newEmployee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: TTTuan (23/12/2022)
        public int InsertEmployee(Employee newEmployee)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(AMISResources.Proc_Insert, typeof(Employee).Name);

            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            var newID = Guid.NewGuid();
            var properties = typeof(Employee).GetProperties();
            foreach (var property in properties)
            {
                string propertyName = property.Name;
                object? propertyValue;
                var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    propertyValue = newID;
                }
                else
                {
                    propertyValue = property.GetValue(newEmployee, null);
                }
                parameters.Add($"${propertyName}", propertyValue);
            }

            var numberOfAffectedRows = 0;

            // Khởi tạo kết nối đến DB
            using (_connectionDL.InitConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                numberOfAffectedRows = _connectionDL.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return numberOfAffectedRows;
        }

        /// <summary>
        /// API Sửa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần sửa</param>
        /// <param name="employee">Đối tượng nhân viên cần sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// Created by: TTTuan (23/12/2022)
        public int UpdateEmployeeByID(Guid employeeID, Employee employee)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(AMISResources.Proc_UpdateByID, typeof(Employee).Name);

            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            var properties = typeof(Employee).GetProperties();
            foreach (var property in properties)
            {
                string propertyName = property.Name;
                object? propertyValue;
                var primaryKeyAttribute = (PrimaryKeyAttribute?)Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    propertyValue = employeeID;
                }
                else
                {
                    propertyValue = property.GetValue(employee, null);
                }
                parameters.Add($"${propertyName}", propertyValue);
            }

            var numberOfAffectedRows = 0;

            // Khởi tạo kết nối đến DB
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return numberOfAffectedRows;
        }
    }
}
