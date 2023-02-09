using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.BL;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;

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
        /// Xuất file excel danh sách bản ghi
        /// </summary>
        /// <returns>File excel danh sách bản ghi</returns>
        /// Modified by: TTTuan (5/1/2022)
        [HttpGet("export")]
        public IActionResult ExportExcel([FromQuery] string? keyword)
        {
            try
            {
                var stream = _employeeBL.ExportExcel(keyword);
                string excelName = $"{AMISResources.Export_Excel_FileName}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
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
        #endregion
    }
}
