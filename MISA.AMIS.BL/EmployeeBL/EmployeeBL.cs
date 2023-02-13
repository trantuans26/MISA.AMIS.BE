using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using MISA.AMIS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;
using Aspose.Cells;
using System.IO;
using System.Data;
using Aspose.Cells.Drawing.Texts;

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
        /// <param name="worksheet">Sheet cần binding format</param>
        /// <param name="employees">Danh sách bản ghi</param>
        /// Modified by: TTTuan 5/1/2023
        private void ImportDataTableExcel(Worksheet worksheet, IEnumerable<Employee> employees)
        {
            // Lấy ra các property có attribute name là ExcelColumnNameAttribute 
            var excelColumnProperties = typeof(Employee).GetProperties().Where(p => p.GetCustomAttributes(typeof(ExcelColumnNameAttribute), true).Length > 0);

            // Tạo một DataTable object
            var dataTable = new DataTable(AMISResources.Export_Excel_TitleName);

            // Tạo header cho DataTable
            dataTable.Columns.Add(AMISResources.Export_Excel_No, typeof(Int32));
            foreach (var property in excelColumnProperties)
            {
                var excelColumnName = (property.GetCustomAttributes(typeof(ExcelColumnNameAttribute), true)[0] as ExcelColumnNameAttribute).ColumnName;
                dataTable.Columns.Add(excelColumnName, typeof(string));
            }

            // Tạo dòng trống cho DataTable
            var dr = dataTable.NewRow();

            // Thêm nội dung cho DataTable
            var stt = 1;
            var columnIndex = 1;
            foreach (var employee in employees)
            {
                dr = dataTable.NewRow();
                dr[0] = stt;

                columnIndex = 1;
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
                            break;
                        case "Gender":
                            value = propertyValue?.ToString() == "Male" ? "Nam" : (propertyValue?.ToString() == "Female" ? "Nữ" : "Khác");
                            break;
                        default:
                            value = propertyValue?.ToString();
                            break;
                    }

                    dr[columnIndex] = value;
                    columnIndex++;
                }
                stt++;

                // Đổ nội dung dòng vào DataTable
                dataTable.Rows.Add(dr);
            }

            // Importing the contents of DataTable to the worksheet starting from "A3" cell,
            // Where true specifies that the column names of the DataTable would be added to
            // The worksheet as a body row
            worksheet.Cells.ImportDataTable(dataTable, true, "A3");
        }

        /// <summary>
        /// Xuất file excel danh sách bản ghi
        /// </summary>
        /// <returns>File excel danh sách bản ghi</returns>
        /// Modified by: TTTuan 5/1/2023
        public MemoryStream ExportExcel(string? keyword)
        {
            /*
            Bỏ comment code bên dưới khi đã mua bản quyền cho Aspose.Cells. 
            Cần triển khai giấy phép trong cùng folder với code, ngoài ra,
            có thể thêm file bản quyền dưới dạng tài nguyên nhúng vào project.
            */
            //Aspose.Cells.License cellsLicense = new Aspose.Cells.License();
            //cellsLicense.SetLicense("Aspose.Cells.lic");

            var wb = new Workbook();

            // Tạo style mặc định cho workbook
            var style = wb.CreateStyle();
            style.Font.Name = "Times New Roman";
            style.Font.Size = 11;
            style.HorizontalAlignment = TextAlignmentType.Left; 
            style.VerticalAlignment = TextAlignmentType.Center;

            // Tạo style cho chủ để
            var styleTitle = wb.CreateStyle();
            styleTitle.Font.Size = 16;
            styleTitle.Font.IsBold = true;
            styleTitle.HorizontalAlignment = TextAlignmentType.Center;
            styleTitle.VerticalAlignment = TextAlignmentType.Center;

            // Tạo style border
            var styleBorder = wb.CreateStyle();
            styleBorder.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
            styleBorder.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
            styleBorder.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
            styleBorder.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);

            // Tạo style cho header bảng
            var styleHeader = wb.CreateStyle();
            styleHeader.Copy(styleBorder);
            styleHeader.Font.Size = 10;
            styleHeader.Font.IsBold = true;
            styleHeader.HorizontalAlignment = TextAlignmentType.Center;
            styleHeader.VerticalAlignment = TextAlignmentType.Center;
            styleHeader.Pattern = BackgroundType.Solid;
            styleHeader.ForegroundColor = System.Drawing.Color.LightGray;

            // Tạo style cho nội dung bảng
            var styleValue = wb.CreateStyle();
            styleValue.Copy(styleBorder);
            styleValue.Copy(style);

            // Tạo style cho ngày tháng
            var styleAlignCenter = wb.CreateStyle();
            styleAlignCenter.Copy(styleValue);
            styleAlignCenter.HorizontalAlignment = TextAlignmentType.Center;
            style.VerticalAlignment = TextAlignmentType.Center;

            // Thêm style mặc định cho workbook
            wb.DefaultStyle = style;

            // Lấy ra các property có attribute name là ExcelColumnNameAttribute 
            var excelColumnProperties = typeof(Employee).GetProperties().Where(p => p.GetCustomAttributes(typeof(ExcelColumnNameAttribute), true).Length > 0);

            // Số các cột = cột số thứ tự + các cột cần xuất file excel
            var columnLength = excelColumnProperties.Count() + 1;

            // Lấy ra tên column cuối cùng (tính cả số thứ tự)
            var lastColumnName = (char)('A' + excelColumnProperties.Count());

            // Thêm một worksheet đến workbook
            var worksheet = wb.Worksheets[0];
            worksheet.Name = AMISResources.Export_Excel_SheetName;

            // Tạo chủ để của worksheet
            var rangeTitle = worksheet.Cells.CreateRange("A1", $"{lastColumnName}1");
            rangeTitle.SetStyle(styleTitle);
            rangeTitle.Value = AMISResources.Export_Excel_TitleName;
            rangeTitle.Merge();

            var rangeEmpty = worksheet.Cells.CreateRange("A2", $"{lastColumnName}2");
            rangeEmpty.Merge();

            var employees = _employeeDL.ExportExcel(keyword);

            // Số dòng bắt đầu từ vị trí A4 nên + 3 
            var rowLength = 3 + employees.Count();

            // Tạo nội dung bảng
            ImportDataTableExcel(worksheet, employees);
            
            // Thiết lập style cho header bảng
            for(var i = 0; i < columnLength; i++)
            {
                worksheet.Cells[2, i].SetStyle(styleHeader);
            }

            // Thiết lập style cho nội dung bảng
            for (var i = 0; i < columnLength; i++)
            {
                for (var j = 3; j < rowLength; j++)
                {
                    if (i == 5)
                    {
                        worksheet.Cells[j, i].SetStyle(styleAlignCenter);
                    } else
                    {
                       worksheet.Cells[j, i].SetStyle(styleValue);
                    }

                }
            }

            // Tự căn chỉnh độ rộng worksheet
            worksheet.AutoFitColumns();

            // Lưu workbook đến MemoryStream
            var stream = new MemoryStream();
            wb.Save(stream, SaveFormat.Xlsx);
            stream.Position = 0;    // important!

            // Giải phóng workbook
            wb.Dispose();

            return stream;
        }
        #endregion
    }
}
