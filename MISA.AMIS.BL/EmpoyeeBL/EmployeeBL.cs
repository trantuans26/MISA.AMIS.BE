using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using MISA.AMIS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field
        private IEmployeeDL _employeeDL;
        #endregion

        #region Constructor
        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra mã trùng
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeID"></param>
        /// <returns>Trả về boolean khi trùng hoặc không</returns>
        /// Created by: TTTuan (23/12/2022)
        private bool CheckDuplicateCode(string? employeeCode, Guid? employeeID)
        {
            return _employeeDL.CheckDuplicateCode(employeeCode, employeeID);
        }

        /// <summary>
        /// Validate dữ liệu đầu vào
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>Đối tượng ServiceResponse mỗ tả thành công hay thất bại</returns>
        /// Created by: TTTuan (23/12/2022)
        static private ServiceResponse ValidateData(Employee employee)
        {
            var errorMessages = new List<string>();

            var properties = typeof(Employee).GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(employee);
                var IsNotNullOrEmptyAttribute = (IsNotNullOrEmptyAttribute?)Attribute.GetCustomAttribute(property, typeof(IsNotNullOrEmptyAttribute));
                if (IsNotNullOrEmptyAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    errorMessages.Add(IsNotNullOrEmptyAttribute.ErrorMessage);
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
        /// API Xóa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        /// Created by: TTTuan (23/12/2022)
        public int DeleteEmployeeByID(Guid employeeID)
        {
            return _employeeDL.DeleteEmployeeByID(employeeID);
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

        /// <summary>
        /// API Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Modified by: TTTuan (23/12/2022)
        public string GetNewEmployeeCode()
        {
            return _employeeDL.GetNewEmployeeCode();
        }

        /// <summary>
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="newEmployee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: TTTuan (23/12/2022)
        public ServiceResponse InsertEmployee(Employee newEmployee)
        {
            // Validate
            var validateResult = ValidateData(newEmployee);

            if (validateResult.Success == (int)StatusResponse.Done)
            {
                var duplicateCode = this.CheckDuplicateCode(newEmployee.EmployeeCode, null);

                if (duplicateCode == true)
                {
                    return new ServiceResponse
                    {
                        Success = (int)StatusResponse.DuplicateCode,
                        Data = new ErrorResult()
                        {
                            ErrorCode = AMISErrorCode.DuplicateCode,
                            DevMsg = AMISResources.DevMsg_DuplicateCode,
                            UserMsg = AMISResources.UserMsg_DuplicateCode,
                            MoreInfo = AMISResources.MoreInfo_DuplicateCode
                        }
                    };
                }

                var numberOfAffectedRows = _employeeDL.InsertEmployee(newEmployee);

                if (numberOfAffectedRows > 0)
                {
                    return new ServiceResponse
                    {
                        Success = (int)StatusResponse.Done,
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = (int)StatusResponse.Failed,
                    };
                }
            }
            else
            {
                return validateResult;
            }
        }

        /// <summary>
        /// API Sửa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần sửa</param>
        /// <param name="employee">Đối tượng nhân viên cần sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// Created by: TTTuan (23/12/2022)
        public ServiceResponse UpdateEmployeeByID(Guid employeeID, Employee employee)
        {
            // Validate
            var validateResult = ValidateData(employee);
            
            if (validateResult.Success == (int)StatusResponse.Done)
            {
                var duplicateCode = this.CheckDuplicateCode(employee.EmployeeCode, null);

                if (duplicateCode != false) duplicateCode = !this.CheckDuplicateCode(employee.EmployeeCode, employeeID);

                if (duplicateCode == true)
                {
                    return new ServiceResponse
                    {
                        Success = (int) StatusResponse.DuplicateCode,
                        Data = new ErrorResult()
                        {
                            ErrorCode = AMISErrorCode.DuplicateCode,
                            DevMsg = AMISResources.DevMsg_DuplicateCode,
                            UserMsg = AMISResources.UserMsg_DuplicateCode,
                            MoreInfo = AMISResources.MoreInfo_DuplicateCode
                        }
                    };
                }

                var numberOfAffectedRows = _employeeDL.UpdateEmployeeByID(employeeID, employee);

                if (numberOfAffectedRows > 0)
                {
                    return new ServiceResponse
                    {
                        Success = (int)StatusResponse.Done,
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = (int)StatusResponse.Failed,
                    };
                }
            } 
            else
            {
                return validateResult;
            }
        } 


        #endregion
    }
}
