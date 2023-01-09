using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.API.UnitTests
{
    public class EmployeeTests
    {
        /// <summary>
        /// Số lớn hơn giá trị cho phép
        /// </summary>
        [Test]
        public void InsertEmployee_BigLength_Returns400BadRequest()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var testEmployee = new EmployeesFakeController();

            Employee e = new Employee();
            e.EmployeeCode = "NV022222222222222222222222222222222222222222222222222022222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222";
            e.EmployeeName = "Trần Thái Tuấn";
            e.DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e");
            e.DepartmentCode = null;
            e.DepartmentName = null;
            e.JobPosition = null;
            e.DateOfBirth = DateTime.Now;
            
            int expectedResult = 400;

            // Act - Gọi vào hàm cần test
            int actualResult = testEmployee.InsertEmployee(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Mã nhân viên bị trùng
        /// </summary>
        [Test]
        public void InsertEmployee_DuplicateCode_Returns400BadRequest()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var testEmployee = new EmployeesFakeController();

            Employee e = new Employee();
            e.EmployeeCode = "NV-08449";
            e.EmployeeName = "Trần Thái Tuấn";
            e.DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e");
            e.DepartmentCode = null;
            e.DepartmentName = null;
            e.JobPosition = null;
            e.DateOfBirth = DateTime.Now;

            int expectedResult = 400;

            // Act - Gọi vào hàm cần test
            int actualResult = testEmployee.InsertEmployee(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Dữ liệu đầu vào trống
        /// </summary>
        [Test]
        public void InsertEmployee_EmptyInput_Returns400BadRequest()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var testEmployee = new EmployeesFakeController();

            Employee e = new Employee();
            e.EmployeeCode = "";
            e.EmployeeName = "";
            e.DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e");
            e.DepartmentCode = null;
            e.DepartmentName = null;
            e.JobPosition = null;
            e.DateOfBirth = DateTime.Now;

            int expectedResult = 400;

            // Act - Gọi vào hàm cần test
            int actualResult = testEmployee.InsertEmployee(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Không đúng định dạng ngày tháng
        /// </summary>
        [Test]
        public void InsertEmployee_InvalidDateType_Returns400BadRequest()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var testEmployee = new EmployeesFakeController();

            Employee e = new Employee();
            e.EmployeeCode = "";
            e.EmployeeName = "";
            e.DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e");
            e.DepartmentCode = null;
            e.DepartmentName = null;
            e.JobPosition = null;
            //e.DateOfBirth = 2022;

            int expectedResult = 400;

            // Act - Gọi vào hàm cần test
            int actualResult = testEmployee.InsertEmployee(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
