using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MISA.AMIS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// API Lấy danh sách tất cả đơn vị
        /// </summary>
        /// <returns>Danh sách thông tin tất cả đơn vị</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            try
            {
                // Chuẩn bị tên stored procedure
                string ConnectionString = "Server=localhost;Port=3306;Database=misaamis_development;Uid=root;Pwd=12345678;";
                string storedProcedureName = "Proc_Department_GetAllDepartments";

                // Khởi tạo kết nối đến DB
                var mySqlConnection = new MySqlConnection(ConnectionString);

                // Gọi vào DB để chạy stored ở trên
                var departments = mySqlConnection.Query(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                return StatusCode(StatusCodes.Status200OK, departments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = "Bắt được exception khi thêm mới",
                    UserMsg = "Lấy dữ liệu thất bại, vui lòng thử lại!",
                    MoreInfor = "https://openapi.com.vn/cukcuk/error-code/0",
                    TraceID = HttpContext.TraceIdentifier
                });
            }
        }
    }
}
