using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.AMIS.DL;
using NSubstitute;

namespace MISA.AMIS.BL.UnitTests
{
    public class EmployeeBLTests
    {
        /// <summary>
        /// Thêm nhân viên thành công
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_Employee_ReturnsSuccessStatus()
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

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(1);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = (int)StatusResponse.Done;

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Thêm nhân viên thất bại
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_Employee_ReturnsFailedStatus()
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

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(0);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = (int)StatusResponse.Failed;

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Validate trống mã 
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_EmptyCode_ReturnsEmptyCodeErrorMessage()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
            {
                EmployeeCode = "",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now
            };

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(1);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = new List<string>
            {
                "Mã nhân viên không được để trống"
            };

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Data?.MoreInfo, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Validate trống tên
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_EmptyName_ReturnsEmptyNameErrorMessage()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
            {
                EmployeeCode = "NV-4444",
                EmployeeName = "", // Tên để trống
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now
            };

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(1);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = new List<string>
            {
                "Tên nhân viên không được để trống"
            };

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Data?.MoreInfo, Is.EqualTo(expectedResult));
        }

         /// <summary>
        /// Validate trống ID đơn vị
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_EmptyDepartmentID_ReturnsEmptyDepartmentIDErrorMessage()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            // Không nhập vào ID đơn vị
            var e = new Employee()
            { 
                EmployeeCode = "NV-4444",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now
            };

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(1);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = new List<string>
            {
                "ID đơn vị không được để trống"
            };

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Data?.MoreInfo, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Validate email sai định dạng
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_InvalidEmail_ReturnsEmailErrorMessage()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
            {
                EmployeeCode = "NV-44444",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now,
                Email = "hihi" // Email không đúng định dạng
            };

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = new List<string>
            {
                "Email không đúng định dạng"
            };

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Data?.MoreInfo, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Validate email đúng định dạng
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_ValidEmail_ReturnsReturnsSuccessStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
            {
                EmployeeCode = "NV-44444",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now,
                Email = "trantuan@gmail.com" // Email đúng định dạng
            };

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(1);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = (int)StatusResponse.Done;


            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }


        /// <summary>
        /// Độ dài mã lớn hơn giá trị cho phép
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_InvalidCodeLength_ReturnsCodeLengthErrorMessage()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
            {
                EmployeeCode = "NV-321432143214321432132143214432143214",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now
            };

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(1);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = new List<string>
            {
                "Mã nhân viên phải nhỏ hơn hoặc bằng 20 ký tự"
            }; 

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(expectedResult, Is.EqualTo(actualResult.Data?.MoreInfo));
        }

        /// <summary>
        /// Độ dài mã lớn hơn giá trị cho phép
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_InvalidNameLength_ReturnsNameLengthErrorMessage()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
            {
                EmployeeCode = "NV-32143",
                EmployeeName = "Trần Thái TuấnTrần Thái TuấnTrần Thái TuấnTrần Thái TuấnTrần Thái TuấnTrần Thái TuấnTrần Thái TuấnTrần Thái TuấnTrần Thái TuấnTrần Thái TuấnTrần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now
            };

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(1);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = new List<string>
            {
                "Tên nhân viên phải nhỏ hơn hoặc bằng 100 ký tự"
            };

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(expectedResult, Is.EqualTo(actualResult.Data?.MoreInfo));
        }

        /// <summary>
        /// Validate số điện thoại không đúng định dạng
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_InvalidPhone_ReturnsPhoneErrorMessage()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
            {
                EmployeeCode = "NV-32143",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now,
                Phone = "asss" // Phone ko đúng định dạng
            };

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(1);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = new List<string>
            {
                "Số điện thoại không hợp lệ",
            };

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(expectedResult, Is.EqualTo(actualResult.Data?.MoreInfo));
        }

        /// <summary>
        /// Validate số điện thoại đúng định dạng
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
        [Test]
        public void InsertRecord_ValidPhone_ReturnsSuccesStatus()
        {
            // Arrange - Chuẩn bị dữ liệu đầu vào và kết quả mong muốn 
            var e = new Employee()
            {
                EmployeeCode = "NV-32143",
                EmployeeName = "Trần Thái Tuấn",
                DepartmentID = new Guid("7686595d-16d5-33b3-0080-e8e2a817c80e"),
                DepartmentCode = null,
                DepartmentName = null,
                JobPosition = null,
                DateOfBirth = DateTime.Now,
                Phone = "0378983269" // Phone đúng định dạng
            };

            var fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            fakeEmployeeDL.InsertRecord(e).Returns(1);

            var employeeBL = new EmployeeBL(fakeEmployeeDL);

            var expectedResult = (int) StatusResponse.Done;

            // Act - Gọi vào hàm cần test
            var actualResult = employeeBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(expectedResult, Is.EqualTo(actualResult.Success));
        }

        /// <summary>
        /// Mã bị trùng
        /// </summary>
        /// Modified by: TTTuan 15/1/2023
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

            var fakeBaseDL = Substitute.For<IEmployeeDL>();
            fakeBaseDL.CheckDuplicateCode(null, "NV-08449").Returns(true);

            var baseBL = new EmployeeBL(fakeBaseDL);

            var expectedResult = (int)StatusResponse.DuplicateCode;

            // Act - Gọi vào hàm cần test
            var actualResult = baseBL.InsertRecord(e);

            // Assert - Kiểm tra kết quả mong muốn và kết quả thực tế
            Assert.That(actualResult.Success, Is.EqualTo(expectedResult));
        }
    }
}
