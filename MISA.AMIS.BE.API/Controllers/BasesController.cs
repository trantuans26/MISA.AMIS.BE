using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.BL;
using MISA.AMIS.Common.Resourcses;
using MISA.AMIS.Common;

namespace MISA.AMIS.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field
        private readonly IBaseBL<T> _baseBL;
        #endregion

        #region Constructor
        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }
        #endregion

        #region Method
        /// <summary>
        /// API lấy danh sách tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi trong bảng</returns>
        /// Modified by: TTTuan 5/1/2023
        [HttpGet]
        public IActionResult GetAllRecords()
        {
            try
            {   // khai báo biến hứng record
                var records = _baseBL.GetAllRecords();

                // Trả về status code và record
                return StatusCode(StatusCodes.Status200OK, records);
            }
            catch (Exception)
            {
                // Trả về status code và thông báo nếu lỗi
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
        /// API lấy thông tin chi tiết của 1 bản ghi theo ID
        /// </summary>
        /// <param name="id">ID của bản ghi</param>
        /// <returns>Thông tin của bản ghi theo ID</returns>
        /// Modified by: TTTuan 5/1/2023

        [HttpGet("{id}")]
        public IActionResult GetRecordByID([FromRoute] Guid id)
        {
            try
            {
                // khai báo biến hứng record
                var record = _baseBL.GetRecordByID(id);

                // Trả về status code và record
                return StatusCode(StatusCodes.Status200OK, record);
            }
            catch (Exception)
            {
                // Trả về status code và thông báo nếu lỗi
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                    AMISErrorCode.Exception,
                    AMISResources.DevMsg_Exception,
                    AMISResources.UserMsg_Exception,
                    AMISResources.MoreInfo_Exception,
                    HttpContext.TraceIdentifier
                    ));
            }
        }

        /// <summary>
        /// API Lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpGet("newCode")]
        public IActionResult GetNewCode()
        {
            try
            {
                var newCode = _baseBL.GetNewCode();

                // Xử lý kết quả trả về
                if (newCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, newCode);
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
        /// API Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="newRecord">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID của bản ghi vừa thêm mới</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpPost]
        public IActionResult InsertRecord([FromBody] T newRecord)
        {
            try
            {
                var result = _baseBL.InsertRecord(newRecord);
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult()
                    {
                        ErrorCode = AMISErrorCode.InsertFailed,
                        DevMsg = AMISResources.DevMsg_InsertFailed,
                        UserMsg = AMISResources.UserMsg_InsertFailed,
                        MoreInfo = "lớn hơn 2",
                        TraceID = HttpContext.TraceIdentifier
                    });
                }
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
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult()
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
        /// API Sửa 1 bản ghi theo ID
        /// </summary>
        /// <param name="id">ID của bản ghi cần sửa</param>
        /// <param name="record">Đối tượng bản ghi cần sửa</param>
        /// <returns>ID của bản ghi vừa sửa</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpPut("{id}")]
        public IActionResult UpdateRecordByID([FromRoute] Guid id, [FromBody] T record)
        {
            try
            {
                var result = _baseBL.UpdateRecordByID(id, record);

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
                    return StatusCode(StatusCodes.Status404NotFound, new
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
        /// API Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="id">ID của bản ghi cần xóa</param>
        /// <returns>ID của bản ghi vừa xóa</returns>
        /// Created by: TTTuan (23/12/2022)
        [HttpDelete("{id}")]
        public IActionResult DeleteRecordByID([FromRoute] Guid id)
        {
            try
            {
                // Xử lý kết quả trả về
                if (_baseBL.DeleteRecordByID(id) > 0)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
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
                    MoreInfo = AMISResources.MoreInfo_Exception,
                    TraceID = HttpContext.TraceIdentifier
                });
            }
        }
        #endregion
    }
}
