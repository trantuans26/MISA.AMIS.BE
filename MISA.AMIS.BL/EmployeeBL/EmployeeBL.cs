using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using MISA.AMIS.DL;
using System;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;
using Aspose.Cells;
using System.IO;

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
        /// Kiểm tra mã trùng
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeID"></param>
        /// <returns>bool kiểm tra có trùng hay không</returns>
        /// Modified by: TTTuan 5/1/2023
        public override ServiceResponse CheckDuplicateCode(Guid? employeeID, Employee employee)
        {
            var duplicateCode = _employeeDL.CheckDuplicateCode(employeeID, employee.EmployeeCode);

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

            return new ServiceResponse { Success = (int)StatusResponse.Done };
        }

        /// <summary>
        /// Binding format style cho file excel
        /// </summary>
        /// <param name="workSheet">Sheet cần binding format</param>
        /// <param name="employees">Danh sách bản ghi</param>
        /// Author:NVHThai (17/10/2022)
        private void BindingFormatForExcel(ExcelWorksheet workSheet, IEnumerable<Employee> employees)
        {
            // Lấy ra các property có attribute name là ExcelColumnNameAttribute 
            var excelColumnProperties = typeof(Employee).GetProperties().Where(p => p.GetCustomAttributes(typeof(ExcelColumnNameAttribute), true).Length > 0);

            // Lấy ra tên column cuối cùng (tính cả số thứ tự)
            var lastColumnName = (char)('A' + excelColumnProperties.Count());

            // Tạo phần tiêu đề cho file excel
            using (var range = workSheet.Cells[$"A1:{lastColumnName}1"])
            {
                range.Merge = true;
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 16;
                range.Style.Font.Name = "Arial";
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Value = AMISResources.Export_Excel_TitleName;
            }
            // Gộp các ô từ A2 đến ô cuối cùng của dòng 2
            workSheet.Cells[$"A2:{lastColumnName}2"].Merge = true;

            // Style chung cho tất cả bảng
            using (var range = workSheet.Cells[$"A3:{lastColumnName}{employees.Count() + 3}"])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Font.Name = "Times New Roman";
                range.Style.Font.Size = 11;
                range.AutoFitColumns();
            }

            // Lấy ra các property có attribute name là ExcelColumnNameAttribute và đổ vào header của table
            int columnIndex = 1;
            workSheet.Cells[3, columnIndex].Value = AMISResources.Export_Excel_No;
            columnIndex++;
            foreach (var property in excelColumnProperties)
            {
                var excelColumnName = (property.GetCustomAttributes(typeof(ExcelColumnNameAttribute), true)[0] as ExcelColumnNameAttribute).ColumnName;
                workSheet.Cells[3, columnIndex].Value = excelColumnName;
                columnIndex++;
            }
            // Style cho header của table
            using (var range = workSheet.Cells[$"A3:{lastColumnName}3"])
            {
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 10;
                range.Style.Font.Name = "Arial";
                range.Style.Font.Color.SetColor(Color.Black);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Đổ dữ liệu từ list nhân viên vào các côt tương ứng
            int rowIndex = 4;
            int stt = 1; // Số thứ tự
            foreach (var employee in employees)
            {
                columnIndex = 1;
                workSheet.Cells[rowIndex, columnIndex].Value = stt;
                columnIndex++;
                foreach (var property in excelColumnProperties)
                {
                    // Lấy ra giá trị của property
                    var propertyValue = property.GetValue(employee);
                    // Trả về đối số kiểu cơ bản của kiểu nullable đã chỉ định.
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var value = "";
                    switch (propertyType.Name)
                    {
                        case "DateTime":
                            value = (propertyValue as DateTime?)?.ToString("dd/MM/yyyy"); // Định dạng ngày tháng
                            workSheet.Cells[rowIndex, columnIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            break;
                        case "Gender":
                            value = propertyValue?.ToString() == "Male" ? "Nam" : (propertyValue?.ToString() == "Female" ? "Nữ" : "Khác");
                            break;
                        default:
                            value = propertyValue?.ToString();
                            break;
                    }
                    // Đổ dữ liệu vào cột
                    workSheet.Cells[rowIndex, columnIndex].Value = value;
                    workSheet.Column(columnIndex).AutoFit();
                    columnIndex++;
                }
                rowIndex++;
                stt++;
            }
        }

        /// <summary>
        /// Xuất file excel danh sách bản ghi
        /// </summary>
        /// <returns>File excel danh sách bản ghi</returns>
        /// Modified by: TTTuan 5/1/2023
        public Stream ExportExcel(string? keyword)
        {
            var employees = _employeeDL.ExportExcel(keyword);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add(AMISResources.Export_Excel_SheetName);
            package.Workbook.Properties.Author = AMISResources.Author;
            package.Workbook.Properties.Title = AMISResources.Export_Excel_TitleName;
            BindingFormatForExcel(workSheet, employees); // Tạo cấu trúc cho file excel
            package.Save();
            stream.Position = 0; // Đặt con trỏ về đầu file để đọc
            return package.Stream;
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
