using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using MISA.AMIS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field
        private readonly IEmployeeDL _employeeDL;
        #endregion

        #region Constructor
        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }
        #endregion

        #region Method
        /// <summary>
        /// Validate dữ liệu đầu vào
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Đối tượng ServiceResponse mỗ tả thành công hay thất bại</returns>
        /// Created by: TTTuan (23/12/2022)
        public static ServiceResponse ValidateData<T>(T record)
        {
            var errorMessages = new List<string>();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(record);

                var isNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));
                if (isNotNullOrEmptyAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    errorMessages.Add(isNotNullOrEmptyAttribute.ErrorMessage);
                }

                var codeLengthAttribute = (CodeAttribute?)Attribute.GetCustomAttribute(property, typeof(CodeAttribute));
                if (codeLengthAttribute != null && propertyValue?.ToString()?.Length > 20)
                {
                    errorMessages.Add(codeLengthAttribute.ErrorMessage);
                }

                var emailAttribute = (EmailAttribute?)Attribute.GetCustomAttribute(property, typeof(EmailAttribute));
                if (emailAttribute != null && propertyValue != null && propertyValue.ToString().Trim().Length > 0)
                {
                    string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
                    if (!Regex.IsMatch(propertyValue.ToString(), regex, RegexOptions.IgnoreCase))
                        errorMessages.Add(emailAttribute.ErrorMessage);
                }
            }
            if (errorMessages.Count > 0)
            {
                return new ServiceResponse
                {
                    Success = (int)StatusResponse.Invalid,
                    Data = new ErrorResult
                    {
                        ErrorCode = AMISErrorCode.InvalidInput,
                        DevMsg = AMISResources.DevMsg_InvalidInput,
                        UserMsg = AMISResources.UserMsg_InvalidInput,
                        MoreInfo = errorMessages.ToArray(),
                    }
                };
            }
            
            return new ServiceResponse { Success = (int)StatusResponse.Done };
        }

        /// <summary>
        /// API Lấy danh sách thông tin nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Mã nhân viên, tên nhân viên, số điện thoại</param>
        /// <param name="pageSize">Số bản ghi muốn lấy</param>
        /// <param name="pageNumber">Số chỉ mục của trang muốn lấy</param>
        /// <returns>Danh sách thông tin nhân viên & tổng số trang và tổng số bản ghi</returns>
        public PagingResult GetEmployeesByFilter(string? keyword, int pageSize, int pageNumber)
        {
            return _employeeDL.GetEmployeesByFilter(keyword, pageSize, pageNumber);
        }
        #endregion
    }
}
