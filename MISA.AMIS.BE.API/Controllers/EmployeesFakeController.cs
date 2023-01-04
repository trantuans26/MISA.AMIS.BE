using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace MISA.AMIS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesFakeController : ControllerBase
    {
        #region API 
        /// <summary>
        /// API Lấy thông tin 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần lấy</param>
        /// <returns>Thông tin 1 nhân viên theo ID</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpGet("{employeeID}")]
        public int GetEmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                // Chuẩn bị tên stored procedure
                string connectionString = "Server=localhost;Port=3306;Database=misaamis_development;Uid=root;Pwd=12345678;";
                string storedProcedureName = "Proc_Employee_GetByID";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("$EmployeeID", employeeID);

                // Khởi tạo kết nối đến DB
                var mySqlConnection = new MySqlConnection(connectionString);

                // Gọi vào DB để chạy stored ở trên
                var employee = mySqlConnection.Query(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                return 200;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 505;
            }
        }

        /// <summary>
        /// API Lấy danh sách thông tin nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Mã nhân viên, tên nhân viên, số điện thoại</param>
        /// <param name="pageSize">Số bản ghi muốn lấy</param>
        /// <param name="pageNumber">Số chỉ mục của trang muốn lấy</param>
        /// <returns>Danh sách thông tin nhân viên và tổng số bản ghi</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpGet("filter")]
        public int GetEmployeesByFilter(
            [FromQuery] string? keyword,
            [FromQuery] int pageSize = 20,
            [FromQuery] int pageNumber = 1
            )
        {
            try
            {
                // Chuẩn bị tên stored procedure
                string connectionString = "Server=localhost;Port=3306;Database=misaamis_development;Uid=root;Pwd=12345678;";
                string storedProcedureName = "Proc_Employee_GetByFilter";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("$Keyword", keyword); ;
                parameters.Add("$PageSize", pageSize);
                parameters.Add("$PageNumber", pageNumber);

                // Khởi tạo kết nối đến DB
                var mySqlConnection = new MySqlConnection(connectionString);

                // Gọi vào DB để chạy stored ở trên
                var employees = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                int count = employees.Read<int>().FirstOrDefault();
                var list = employees.Read<Employee>().ToList();

                // Xử lý kết quả trả về
                int totalPage = (int)Math.Ceiling((double)count / pageSize);

                return 200;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 500;
            }
        }

        /// <summary>
        /// API Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpGet("newEmployeeCode")]
        public int GetNewEmployeeCode()
        {
            try
            {
                // Chuẩn bị tên stored procedure
                string connectionString = "Server=localhost;Port=3306;Database=misaamis_development;Uid=root;Pwd=12345678;";
                string storedProcedureName = "Proc_Employee_GetNewEmployeeCode";

                // Khởi tạo kết nối đến DB
                var mySqlConnection = new MySqlConnection(connectionString);

                // Gọi vào DB để chạy stored ở trên
                var newEmployeeCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                if (newEmployeeCode != null)
                {
                    return 200;
                }
                return 404;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 500;
            }
        }

        /// <summary>
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpPost]
        public int InsertEmployee([FromBody] Employee newEmployee)
        {
            try
            {
                // Validate
                bool duplicateCode = this.CheckDuplicateCode(newEmployee.EmployeeCode, null);

                if (!ModelState.IsValid || duplicateCode == true)
                {
                    List<string> messageError = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    if (duplicateCode == true) messageError.Add("Mã nhân viên đã tồn tại trong hệ thống.");

                    return 400;
                }

                // Chuẩn bị tên stored procedure
                string connectionString = "Server=localhost;Port=3306;Database=misaamis_development;Uid=root;Pwd=12345678;";
                string storedProcedureName = "Proc_Employee_Insert";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                var newEmployeeID = Guid.NewGuid();
                parameters.Add("$EmployeeID", newEmployeeID);
                parameters.Add("$EmployeeCode", newEmployee.EmployeeCode);
                parameters.Add("$EmployeeName", newEmployee.EmployeeName);
                parameters.Add("$DepartmentID", newEmployee.DepartmentID);
                parameters.Add("$JobPosition", newEmployee.JobPosition);
                parameters.Add("$DateOfBirth", newEmployee.DateOfBirth);
                parameters.Add("$Gender", newEmployee.Gender);
                parameters.Add("$IdentityNumber", newEmployee.IdentityNumber);
                parameters.Add("$IdentityDate", newEmployee.IdentityDate);
                parameters.Add("$IdentityPlace", newEmployee.IdentityPlace);
                parameters.Add("$Address", newEmployee.Address);
                parameters.Add("$Phone", newEmployee.Phone);
                parameters.Add("$Fax", newEmployee.Fax);
                parameters.Add("$Email", newEmployee.Email);
                parameters.Add("$BankNumber", newEmployee.BankNumber);
                parameters.Add("$BankName", newEmployee.BankName);
                parameters.Add("$BankBranch", newEmployee.BankBranch);
                parameters.Add("$CreatedDate", newEmployee.CreatedDate);
                parameters.Add("$CreatedBy", newEmployee.CreatedBy);

                // Khởi tạo kết nối đến DB
                var mySqlConnection = new MySqlConnection(connectionString);

                // Gọi vào DB để chạy stored ở trên
                var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                if (numberOfAffectedRows > 0)
                {
                    return 201;
                }
                else
                {
                    return 500;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 500;
            }
        }

        /// <summary>
        /// API Sửa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần sửa</param>
        /// <param name="employee">Đối tượng nhân viên cần sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpPut("{employeeID}")]
        public int UpdateEmployeeByID([FromRoute] Guid employeeID, [FromBody] Employee employee)
        {
            try
            {
                // Validate
                bool duplicateCode = this.CheckDuplicateCode(employee.EmployeeCode, null);
                if (duplicateCode != false) duplicateCode = !this.CheckDuplicateCode(employee.EmployeeCode, employeeID);

                if (!ModelState.IsValid || duplicateCode == true)
                {
                    List<string> messageError = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    if (duplicateCode == true) messageError.Add("Mã nhân viên đã tồn tại trong hệ thống.");

                    return 400;
                }

                // Chuẩn bị tên stored procedure
                string connectionString = "Server=localhost;Port=3306;Database=misaamis_development;Uid=root;Pwd=12345678;";

                string storedProcedureName = "Proc_Employee_UpdateByID";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("$EmployeeID", employeeID);
                parameters.Add("$EmployeeCode", employee.EmployeeCode);
                parameters.Add("$EmployeeName", employee.EmployeeName);
                parameters.Add("$DepartmentID", employee.DepartmentID);
                parameters.Add("$JobPosition", employee.JobPosition);
                parameters.Add("$DateOfBirth", employee.DateOfBirth);
                parameters.Add("$Gender", employee.Gender);
                parameters.Add("$IdentityNumber", employee.IdentityNumber);
                parameters.Add("$IdentityDate", employee.IdentityDate);
                parameters.Add("$IdentityPlace", employee.IdentityPlace);
                parameters.Add("$Address", employee.Address);
                parameters.Add("$Phone", employee.Phone);
                parameters.Add("$Fax", employee.Fax);
                parameters.Add("$Email", employee.Email);
                parameters.Add("$BankNumber", employee.BankNumber);
                parameters.Add("$BankName", employee.BankName);
                parameters.Add("$BankBranch", employee.BankBranch);
                parameters.Add("$ModifiedDate", employee.ModifiedDate);
                parameters.Add("$ModifiedBy", employee.ModifiedBy);


                // Khởi tạo kết nối đến DB
                var mySqlConnection = new MySqlConnection(connectionString);

                // Gọi vào DB để chạy stored ở trên
                var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                if (numberOfAffectedRows > 0)
                {
                    return 200;
                }
                else
                {
                    return 500;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 500;
            }
        }

        /// <summary>
        /// API Xóa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpDelete("{employeeID}")]
        public int DeleteEmployee([FromRoute] Guid employeeID)
        {
            try
            {
                // Chuẩn bị tên stored procedure
                string connectionString = "Server=localhost;Port=3306;Database=misaamis_development;Uid=root;Pwd=12345678;";

                string storedProcedureName = "Proc_Employee_DeleteByID";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("$EmployeeID", employeeID);

                // Khởi tạo kết nối đến DB
                var mySqlConnection = new MySqlConnection(connectionString);

                // Gọi vào DB để chạy stored ở trên
                var numberOfAffectedRows = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                if (numberOfAffectedRows > 0)
                {
                    return 200;
                }
                else
                {
                    return 500;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 500;
            }
        }

        /// <summary>
        /// API kiểm tra mã trùng
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeID"></param>
        /// <returns> Mã nhân viên trùng </returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpGet("duplicateCode")]
        public bool CheckDuplicateCode([FromQuery] string? employeeCode, [FromQuery] Guid? employeeID)
        {
            try
            {
                // Chuẩn bị tên stored procedure
                string connectionString = "Server=localhost;Port=3306;Database=misaamis_development;Uid=root;Pwd=12345678;";
                string storedProcedureName = "Proc_Employee_DuplicateCode";

                // Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("$EmployeeID", employeeID);
                parameters.Add("$EmployeeCode", employeeCode);

                // Khởi tạo kết nối đến DB
                var mySqlConnection = new MySqlConnection(connectionString);

                var DuplicateCode = mySqlConnection.QueryFirstOrDefault<Employee>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (DuplicateCode != null) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }
        #endregion
    }
}