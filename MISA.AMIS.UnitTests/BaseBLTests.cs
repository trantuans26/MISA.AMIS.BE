using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Common;
using MISA.AMIS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.AMIS.DL;
using MISA.AMIS.BL.UnitTests;
using NSubstitute;

namespace MISA.AMIS.API.UnitTests
{
    public class BaseBLTests
    {
        #region Field
        private IEmployeeDL _employeeDL = new EmployeeDL(new TestConnectionDL());
        #endregion

        /// <summary>
        /// Thêm nhân viên thành công
        /// </summary>
        [Test]
        public void InsertEmployee_Employee_ReturnsSuccessStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn
            Employee e = new()
            {
                EmployeeCode = "NV-1111",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();
            fakeBaseDL.InsertRecord(e).Returns(1);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            int expectedResult = (int)StatusResponse.Done;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Validate input trống
        /// </summary>
        [Test]
        public void InsertEmployee_EmptyInput_ReturnsInvalidInputStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            Employee e = new()
            {
                EmployeeCode = "",
                EmployeeName = "",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();
            fakeBaseDL.InsertRecord(e).Returns(1);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            int expectedResult = (int) StatusResponse.Invalid;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.AreEqual(expectedResult, actualResult.Success);
        }

        /// <summary>
        /// Validate email sai định dạng
        /// </summary>
        [Test]
        public void InsertEmployee_InvalidEmail_ReturnsInvalidInputStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            Employee e = new()
            {
                EmployeeCode = "4444",
                EmployeeName = "444",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now,
                Email = "hihi"
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();
            fakeBaseDL.InsertRecord(e).Returns(1);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            int expectedResult = (int)StatusResponse.Invalid;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.AreEqual(expectedResult, actualResult.Success);
        }

        /// <summary>
        /// Số lớn hơn giá trị cho phép
        /// </summary>
        [Test]
        public void InsertEmployee_InvalidCodeLength_ReturnsInvalidInputStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            Employee e = new()
            {
                EmployeeCode = "NV022222222222222222222222222222222222222222222222222022222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();
            fakeBaseDL.InsertRecord(e).Returns(1);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            int expectedResult = (int)StatusResponse.Invalid;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.AreEqual(expectedResult, actualResult.Success);
        }

        /// <summary>
        /// Mã nhân viên bị trùng
        /// </summary>
        [Test]
        public void InsertEmployee_DuplicateCode_ReturnsDuplicateCodeStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            Employee e = new()
            {
                EmployeeCode = "NV-08449",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now
            };

            var fakeBaseDL = Substitute.For<IBaseDL<Employee>>();
            fakeBaseDL.InsertRecord(e).Returns(1);
            fakeBaseDL.CheckDuplicateCode("NV-08449", null).Returns(true);

            var baseBL = new BaseBL<Employee>(fakeBaseDL);

            ServiceResponse expectedResult = new()
            {
                Success = (int)StatusResponse.DuplicateCode,
            };

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.AreEqual(expectedResult.Success, actualResult.Success);
        }
    }
}
