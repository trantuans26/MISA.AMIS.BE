using MISA.AMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DL
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// API kiểm tra mã trùng
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeID"></param>
        /// <returns> Mã nhân viên trùng </returns>
        /// Created by: TTTuan (23/12/2022)
        public bool CheckDuplicateCode(string? employeeCode, Guid? employeeID);


        /// <summary>
        /// API Xóa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        /// Created by: TTTuan (23/12/2022)
        public int DeleteEmployeeByID(Guid employeeID);

        /// <summary>
        /// API Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Modified by: TTTuan (23/12/2022)
        public string GetNewEmployeeCode();

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
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="newEmployee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        /// Created by: TTTuan (23/12/2022)
        public int InsertEmployee(Employee newEmployee);

        /// <summary>
        /// API Sửa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần sửa</param>
        /// <param name="employee">Đối tượng nhân viên cần sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        /// Created by: TTTuan (23/12/2022)
        public int UpdateEmployeeByID(Guid employeeID, Employee employee);



    }
}
