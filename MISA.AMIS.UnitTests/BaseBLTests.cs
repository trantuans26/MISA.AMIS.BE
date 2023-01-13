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
using NSubstitute;

namespace MISA.AMIS.API.UnitTests
{
    public class BaseBLTests
    {
        /// <summary>
        /// Thêm nhân viên thành công
        /// </summary>
        [Test]
        public void InsertRecorrd_Employee_ReturnsSuccessStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn
            var e = new Employee()
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

            var expectedResult = (int)StatusResponse.Done;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Validate input trống
        /// </summary>
        [Test]
        public void InsertRecord_EmptyInput_ReturnsInvalidInputStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
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

            var expectedResult = (int) StatusResponse.Invalid;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Validate email sai định dạng
        /// </summary>
        [Test]
        public void InsertRecord_InvalidEmail_ReturnsInvalidInputStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
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

            var expectedResult = (int)StatusResponse.Invalid;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Số lớn hơn giá trị cho phép
        /// </summary>
        [Test]
        public void InsertRecord_InvalidCodeLength_ReturnsInvalidInputStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
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

            var expectedResult = (int)StatusResponse.Invalid;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Mã nhân viên bị trùng
        /// </summary>
        [Test]
        public void InsertRecord_DuplicateCode_ReturnsDuplicateCodeStatus()
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

            var expectedResult = (int)StatusResponse.DuplicateCode;

            // Act - Gọi vào hàm cần test
            ServiceResponse actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }
    }
}
