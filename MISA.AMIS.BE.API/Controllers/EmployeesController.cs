using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.API.Controllers;
using MISA.AMIS.BL;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using MISA.AMIS.DL;
using MySqlConnector;

namespace MISA.AMIS.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BasesController<Employee>
    {
        #region Field
        private readonly IEmployeeBL _employeeBL;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL) 
        {
            _employeeBL = employeeBL;
        }
        #endregion

        #region Method
        /// <summary>
        /// API Lấy danh sách thông tin nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Mã nhân viên, tên nhân viên, số điện thoại</param>
        /// <param name="pageSize">Số bản ghi muốn lấy</param>
        /// <param name="pageNumber">Số chỉ mục của trang muốn lấy</param>
        /// <returns>Danh sách thông tin nhân viên & tổng số trang và tổng số bản ghi</returns>
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
                PagingResult employeeFilter = _employeeBL.GetEmployeesByFilter(keyword, pageSize, pageNumber);

                return StatusCode(StatusCodes.Status200OK, employeeFilter); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = AMISResources.DevMsg_Exception,
                    UserMsg = AMISResources.UserMsg_Exception,
                    MoreInfo = AMISResources.MoreInfo_Exception,
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
                var newEmployeeCode = _employeeBL.GetNewEmployeeCode();

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
                    DevMsg = AMISResources.DevMsg_Exception,
                    UserMsg = AMISResources.UserMsg_Exception,
                    MoreInfo = "https://openapi.com.vn/cukcuk/error-code/0",
                    TraceID = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="newEmployee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpPost]
        public IActionResult InsertEmployee([FromBody] Employee newEmployee)
        {
            try
            {
                var result = _employeeBL.InsertEmployee(newEmployee);

                // Xử lý kết quả trả về
                if (result.Success == (int)StatusResponse.Done)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                else if (result.Success == (int)StatusResponse.Invalid || result.Success == (int)StatusResponse.DuplicateCode)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        ErrorCode = AMISErrorCode.InsertFailed,
                        DevMsg = AMISResources.DevMsg_InsertFailed,
                        UserMsg = AMISResources.UserMsg_InsertFailed,
                        MoreInfo = AMISResources.MoreInfo_InsertFailed,
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
                    DevMsg = AMISResources.DevMsg_Exception,
                    UserMsg = AMISResources.UserMsg_Exception,
                    MoreInfo = AMISResources.MoreInfo_Exception,
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
                var result = _employeeBL.UpdateEmployeeByID(employeeID, employee);

                // Xử lý kết quả trả về
                if (result.Success == (int)StatusResponse.Done)
                {
                    return StatusCode(StatusCodes.Status200OK);
                } 
                else if (result.Success == (int)StatusResponse.Invalid || result.Success == (int)StatusResponse.DuplicateCode)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        ErrorCode = AMISErrorCode.UpdateFailed,
                        DevMsg = AMISResources.DevMsg_UpdateFailed,
                        UserMsg = AMISResources.UserMsg_UpdateFailed,
                        MoreInfo = AMISResources.MoreInfo_UpdateFailed,
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
                    DevMsg = AMISResources.DevMsg_Exception,
                    UserMsg = AMISResources.UserMsg_Exception,
                    MoreInfo = AMISResources.MoreInfo_Exception,
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
        public IActionResult DeleteEmployeeByID([FromRoute] Guid employeeID)
        {
            try
            {
                // Xử lý kết quả trả về
                if (_employeeBL.DeleteEmployeeByID(employeeID) > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        ErrorCode = AMISErrorCode.DeleteFailed,
                        DevMsg = AMISResources.DevMsg_DeleteFailed,
                        UserMsg = AMISResources.UserMsg_DeleteFailed,
                        MoreInfo = AMISResources.MoreInfo_DeleteFailed,
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
                    DevMsg = AMISResources.DevMsg_Exception,
                    UserMsg = AMISResources.UserMsg_Exception,
                    MoreInfo = "https://openapi.com.vn/cukcuk/error-code/0",
                    TraceID = HttpContext.TraceIdentifier
                });
            }
        }
        #endregion
    }
}
