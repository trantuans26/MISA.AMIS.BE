using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MISA.AMIS.Common
{
    public class Employee
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        [Key]
        [PrimaryKey]
        public Guid? EmployeeID { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [NotEmpty("Mã nhân viên không được để trống")]
        [Required(ErrorMessage = "Mã nhân viên không được để trống")]
        [DuplicateCode]
        public string? EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [NotEmpty("Tên nhân viên không được để trống")]
        public string? EmployeeName { get; set; }

        /// <summary>
        /// ID phòng ban
        /// </summary>
        [Required(ErrorMessage = "ID đơn vị không được để trống")]
        [NotEmpty("ID đơn vị không được để trống")]
        public Guid? DepartmentID { get; set; }

        /// <summary>
        /// Chức vụ
        /// </summary>
        public string? JobPosition { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Số CCCD
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CCCD
        /// </summary>
        public DateTime IdentityDate { get; set; }

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
        public string? Phone { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        public string? Fax { get; set; }

        /// <summary>
        /// Địa chỉ Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        public string? BankNumber { get; set; }

        /// <summary>
        /// Tên tài khoản ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string? BankBranch { get; set; }

        /// <summary>
        /// Ngày tạo 
        /// </summary>
        public DateTime? CreatedDate { get; set; } 

        /// <summary>
        /// Người tạo 
        /// </summary>
        public string? CreatedBy { get; set; } 

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime? ModifiedDate { get; set; } 

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; } 

    }
}
