using MISA.AMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL
{
    public interface IEmployeeBL : IBaseBL<Employee>
    {
        /// <summary>
        /// Kiểm tra mã trùng
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeID"></param>
        /// <returns>bool kiểm tra có trùng hay không</returns>
        /// Modified by: TTTuan 5/1/2023
        public new ServiceResponse CheckDuplicateCode(Guid? employeeID, Employee employee);

        /// <summary>
        /// Xuất file excel danh sách bản ghi
        /// </summary>
        /// <returns>File excel danh sách bản ghi</returns>
        /// Modified by: TTTuan 5/1/2023
        public MemoryStream ExportExcel(string? keyword);

        /// <summary>
        /// API Lấy danh sách thông tin nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Mã nhân viên, tên nhân viên, số điện thoại</param>
        /// <param name="pageSize">Số bản ghi muốn lấy</param>
        /// <param name="pageNumber">Số chỉ mục của trang muốn lấy</param>
        /// <returns>Danh sách thông tin nhân viên & tổng số trang và tổng số bản ghi</returns>
        /// Created by: TTTuan (23/12/2022)
        public PagingResult GetEmployeesByFilter(string? keyword, int pageSize, int pageNumber);

        /// <summary>
        /// Validate dữ liệu đầu vào
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>Đối tượng ServiceResponse mỗ tả thành công hay thất bại</returns>
        /// Created by: TTTuan (23/12/2022)
        public new ServiceResponse ValidateData(Employee employee);
    }
}
