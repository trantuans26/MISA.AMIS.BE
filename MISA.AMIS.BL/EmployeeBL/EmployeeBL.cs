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
        /// <param name="workSheet">Sheet cần binding format</param>
        /// <param name="employees">Danh sách bản ghi</param>
        /// Author:NVHThai (17/10/2022)
        private void BindingFormatForExcel(Worksheet workSheet, IEnumerable<Employee> employees)
        {
        }

        /// <summary>
        /// Xuất file excel danh sách bản ghi
        /// </summary>
        /// <returns>File excel danh sách bản ghi</returns>
        /// Modified by: TTTuan 5/1/2023
        public Stream ExportExcel(string? keyword)
        {
            var stream = new MemoryStream();
            return stream;
        }

        /// <summary>
        /// Xuất file excel danh sách bản ghi
        /// </summary>
        /// <returns>File excel danh sách bản ghi</returns>
        /// Modified by: TTTuan 5/1/2023
        public MemoryStream ExportExcelAspose(string? keyword)
        {
            /*
             * Uncomment the code below when you have purchased license
             * for Aspose.Cells. You need to deploy the license in the
             * same folder as your executable, alternatively you can add
             * the license file as an embedded resource to your project.
            */
            //Aspose.Cells.License cellsLicense = new Aspose.Cells.License();
            //cellsLicense.SetLicense("Aspose.Cells.lic");

            // Create workbook
            var wb = new Workbook();

            // Adding a new worksheet to the Workbook object
            var worksheet = wb.Worksheets[0];
            worksheet.Name = AMISResources.Export_Excel_SheetName;

            var style = wb.CreateStyle();
            style.Font.Name = "Times New Roman";
            style.Font.Size = 11;
            style.HorizontalAlignment = TextAlignmentType.Left; 
            style.VerticalAlignment = TextAlignmentType.Center;

            var styleTitle = wb.CreateStyle();
            styleTitle.Font.Size = 16;
            styleTitle.Font.IsBold = true;
            styleTitle.HorizontalAlignment = TextAlignmentType.Center;
            styleTitle.VerticalAlignment = TextAlignmentType.Center;


            var styleHeader = wb.CreateStyle();
            styleHeader.Font.Size = 10;
            styleHeader.Font.IsBold = true;
            styleHeader.HorizontalAlignment = TextAlignmentType.Center;
            styleHeader.VerticalAlignment = TextAlignmentType.Center;
            styleHeader.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
            styleHeader.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
            styleHeader.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
            styleHeader.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);
            styleHeader.Pattern = BackgroundType.Solid;
            styleHeader.ForegroundColor = System.Drawing.Color.LightGray;

            var styleValue = wb.CreateStyle();
            styleValue.Font.Name = "Times New Roman";
            styleValue.Font.Size = 11;
            styleValue.HorizontalAlignment = TextAlignmentType.Left;
            styleValue.VerticalAlignment = TextAlignmentType.Center;
            styleValue.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
            styleValue.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
            styleValue.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
            styleValue.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);

            var styleAlignCenter = wb.CreateStyle();
            styleAlignCenter.HorizontalAlignment = TextAlignmentType.Center;
            styleAlignCenter.Font.Name = "Times New Roman";
            styleAlignCenter.Font.Size = 11;
            styleAlignCenter.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
            styleAlignCenter.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
            styleAlignCenter.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
            styleAlignCenter.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);

            wb.DefaultStyle= style;

            // Lấy ra các property có attribute name là ExcelColumnNameAttribute 
            var excelColumnProperties = typeof(Employee).GetProperties().Where(p => p.GetCustomAttributes(typeof(ExcelColumnNameAttribute), true).Length > 0);

            // Lấy ra tên column cuối cùng (tính cả số thứ tự)
            var lastColumnName = (char)('A' + excelColumnProperties.Count());


            var dataTableTitle = new DataTable(AMISResources.Export_Excel_TitleName);

            Aspose.Cells.Range rangeTitle = worksheet.Cells.CreateRange("A1", $"{lastColumnName}1");
            rangeTitle.SetStyle(styleTitle);
            rangeTitle.Value = AMISResources.Export_Excel_TitleName;
            rangeTitle.Merge();

            Aspose.Cells.Range rangeEmpty = worksheet.Cells.CreateRange("A2", $"{lastColumnName}2");
            rangeEmpty.Merge();

            // Instantiating a DataTable object
            var dataTable = new DataTable(AMISResources.Export_Excel_TitleName);

            var columnLength = 1;
            dataTable.Columns.Add(AMISResources.Export_Excel_No, typeof(Int32));
            foreach (var property in excelColumnProperties)
            {
                var excelColumnName = (property.GetCustomAttributes(typeof(ExcelColumnNameAttribute), true)[0] as ExcelColumnNameAttribute).ColumnName;
                dataTable.Columns.Add(excelColumnName, typeof(string));
                columnLength++;

            }


            var employees = _employeeDL.ExportExcel(keyword);

            // Creating an empty row in the DataTable object
            var dr = dataTable.NewRow();

            var stt = 1;
            var rowLength = 3;
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
                // Adding filled row to the DataTable object
                dataTable.Rows.Add(dr);
                rowLength++;
            }

            // Importing the contents of DataTable to the worksheet starting from "A1" cell,
            // Where true specifies that the column names of the DataTable would be added to
            // The worksheet as a header row
            worksheet.Cells.ImportDataTable(dataTable, true, "A3");
            
            for(var i = 0; i < columnLength; i++)
            {
                worksheet.Cells[2, i].SetStyle(styleHeader);

            }

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

            worksheet.AutoFitColumns();

            // save workbook to MemoryStream
            var stream = new MemoryStream();
            wb.Save(stream, SaveFormat.Xlsx);
            stream.Position = 0;    // important!

            return stream;
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
