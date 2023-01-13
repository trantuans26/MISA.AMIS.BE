﻿using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.BL;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;

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
        #endregion
    }
}
