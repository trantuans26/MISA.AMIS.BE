using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace MISA.AMIS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region API 
        /// <summary>
        /// API Lấy thông tin 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần lấy</param>
        /// <returns>Thông tin 1 nhân viên theo ID</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpGet("{employeeID}")]
        public IActionResult GetEmployeeByID([FromRoute] Guid employeeID)
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
                return StatusCode(StatusCodes.Status200OK, employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = "Bắt được exception khi lấy nhân viên theo ID",
                    UserMsg = "Lấy dữ liệu thất bại, vui lòng thử lại!",
                    MoreInfor = "https://openapi.com.vn/cukcuk/error-code/0",
                    TraceID = HttpContext.TraceIdentifier
                });
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
        public IActionResult GetEmployeesByFilter(
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
                parameters.Add("$Keyword", keyword);;
                parameters.Add("$PageSize", pageSize);
                parameters.Add("$PageNumber", pageNumber);

                // Khởi tạo kết nối đến DB
                var mySqlConnection = new MySqlConnection(connectionString);

                // Gọi vào DB để chạy stored ở trên
                var employees = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                
                int count = employees.Read<int>().FirstOrDefault();
                var list = employees.Read<Employee>().ToList();

                // Xử lý kết quả trả về
                int totalPage = (int) Math.Ceiling( (double) count / pageSize);

                return StatusCode(StatusCodes.Status200OK, new PagingResult
                {
                    Data = list,
                    TotalRecord = count,
                    TotalPage = totalPage
                }); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = "Bắt được exception khi lấy nhân viên theo tìm kiếm và phân trang",
                    UserMsg = "Tải dữ liệu thất bại, vui lòng thử lại!",
                    MoreInfor = "https://openapi.com.vn/cukcuk/error-code/0",
                    TraceID = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// API Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpGet("newEmployeeCode")]
        public IActionResult GetNewEmployeeCode()
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
                    return StatusCode(StatusCodes.Status200OK, newEmployeeCode);
                }
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = "Bắt được exception khi lấy mã nhân viên mới",
                    UserMsg = "Lấy mã nhân viên mới thất bại, vui lòng thử lại!",
                    MoreInfor = "https://openapi.com.vn/cukcuk/error-code/0",
                    TraceID = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpPost]
        public IActionResult InsertEmployee([FromBody] Employee newEmployee)
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

                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = AMISErrorCode.InvalidInput,
                        DevMsg = "EmployeeCode is existed, please try another code",
                        UserMsg = "Dữ liệu đầu vào không hợp lệ.",
                        MoreInfor = messageError,
                        TraceID = HttpContext.TraceIdentifier
                    });
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
                    return StatusCode(StatusCodes.Status201Created, newEmployeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        ErrorCode = AMISErrorCode.InsertFailed,
                        DevMsg = "Gọi procedure trong database thất bại",
                        UserMsg = "Thêm mới thất bại, vui lòng thử lại!",
                        MoreInfor = "https://openapi.com.vn/cukcuk/error-code/1",
                        TraceID = HttpContext.TraceIdentifier
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = "Bắt được exception khi thêm mới",
                    UserMsg = "Thêm mới thất bại, vui lòng thử lại!",
                    MoreInfor = "https://openapi.com.vn/cukcuk/error-code/0",
                    TraceID = HttpContext.TraceIdentifier
                });
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
        public IActionResult UpdateEmployeeByID([FromRoute] Guid employeeID, [FromBody] Employee employee)
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

                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = AMISErrorCode.InvalidInput,
                        DevMsg = "EmployeeCode is existed, please try another code",
                        UserMsg = "Dữ liệu đầu vào không hợp lệ.",
                        MoreInfor = messageError,
                        TraceID = HttpContext.TraceIdentifier
                    });
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
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        ErrorCode = AMISErrorCode.UpdateFailed,
                        DevMsg = "Gọi procedure trong database thất bại",
                        UserMsg = "Sửa thất bại, vui lòng thử lại!",
                        MoreInfor = "https://openapi.com.vn/cukcuk/error-code/1",
                        TraceID = HttpContext.TraceIdentifier
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = "Bắt được exception khi sửa",
                    UserMsg = "Sửa thất bại, vui lòng thử lại!",
                    MoreInfor = "https://openapi.com.vn/cukcuk/error-code/0",
                    TraceID = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// API Xóa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpDelete("{employeeID}")]
        public IActionResult DeleteEmployee([FromRoute] Guid employeeID)
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
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        ErrorCode = AMISErrorCode.DeleteFailed,
                        DevMsg = "Gọi procedure trong database thất bại",
                        UserMsg = "Xoá thất bại, vui lòng thử lại!",
                        MoreInfor = "https://openapi.com.vn/cukcuk/error-code/1",
                        TraceID = HttpContext.TraceIdentifier
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = "Bắt được exception khi xoá",
                    UserMsg = "Xoá thất bại, vui lòng thử lại!",
                    MoreInfor = "https://openapi.com.vn/cukcuk/error-code/0",
                    TraceID = HttpContext.TraceIdentifier
                });
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
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }


        }
        #endregion
    }
}
