using MISA.AMIS.Common.Entities;
using System;
using System.Reflection;

namespace MISA.AMIS.Common
{
    public class Employee : BaseEntity
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        [PrimaryKey]
        public Guid? EmployeeID { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [IsNotNullOrEmpty("Mã nhân viên không được để trống")]
        [MaxLength("Mã nhân viên phải nhỏ hơn hoặc bằng 20 ký tự", 20)]
        [ExcelColumnName("Mã nhân viên")]
        public string? EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [IsNotNullOrEmpty("Tên nhân viên không được để trống")]
        [MaxLength("Tên nhân viên phải nhỏ hơn hoặc bằng 100 ký tự", 100)]
        [ExcelColumnName("Tên nhân viên")]
        public string? EmployeeName { get; set; }

        /// <summary>
        /// ID phòng ban
        /// </summary>
        [IsNotNullOrEmpty("ID đơn vị không được để trống")]
        public Guid? DepartmentID { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string? DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [ExcelColumnName("Tên đơn vị")]
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Chức vụ
        /// </summary>
        [ExcelColumnName("Chức danh")]
        public string? JobPosition { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [ExcelColumnName("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [ExcelColumnName("Giới tính")]
        public Gender? Gender { get; set; }

        /// <summary>
        /// Số CCCD
        /// </summary>
        
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CCCD
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp CCCD
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Địa chỉ nhà
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        [Regex("Số điện thoại không hợp lệ", @"^\d{10}$")]
        public string? Phone { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        public string? Fax { get; set; }

        /// <summary>
        /// Địa chỉ Email
        /// </summary>
        [Regex("Email không đúng định dạng", @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$")]
        public string? Email { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        [ExcelColumnName("Số tài khoản")]
        public string? BankNumber { get; set; }

        /// <summary>
        /// Tên tài khoản ngân hàng
        /// </summary>
        [ExcelColumnName("Tên ngân hàng")]
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        [ExcelColumnName("Chi nhánh TK ngân hàng")]
        public string? BankBranch { get; set; }
    }
}
