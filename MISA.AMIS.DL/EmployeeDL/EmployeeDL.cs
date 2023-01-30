using Dapper;
using MISA.AMIS.Common;
using MISA.AMIS.Common.Resourcses;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        #region Field
        private readonly IConnectionDL _connectionDL;
        #endregion

        #region Constructor
        public EmployeeDL(IConnectionDL connectionDL) : base(connectionDL)
        {
            _connectionDL = connectionDL;
        }

        public IEnumerable<Employee> ExportExcel(string? keyword)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.EXPORT_EXCEL, typeof(Employee).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("$Keyword", keyword); ;

            // Khai báo kết quả trả về
            var employees = default(IEnumerable<Employee>);

            // Khởi tạo kết nối đến DB
            using (var connection = new MySqlConnection(connectionString))
            {
                // Thực hiện gọi vào db
                employees = connection.Query<Employee>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return employees;
        }
        #endregion

        /// <summary>
        /// API Lấy danh sách thông tin nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Mã nhân viên, tên nhân viên, số điện thoại</param>
        /// <param name="pageSize">Số bản ghi muốn lấy</param>
        /// <param name="pageNumber">Số chỉ mục của trang muốn lấy</param>
        /// <returns>Danh sách thông tin nhân viên & tổng số trang và tổng số bản ghi</returns>
        /// Created by: TTTuan (23/12/2022)
        public PagingResult GetEmployeesByFilter(string? keyword, int pageSize, int pageNumber)
        {
            // Chuẩn bị chuỗi kết nối
            var connectionString = DataContext.ConnectionString;

            // Chuẩn bị tên stored procedure
            var storedProcedureName = string.Format(ProcedureNames.GET_BY_FILTER, typeof(Employee).Name);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("$Keyword", keyword); ;
            parameters.Add("$PageSize", pageSize);
            parameters.Add("$PageNumber", pageNumber);

            // Khai báo kết quả trả về
            var count = 0;
            var list = new List<Employee>();
            var totalPage = 0;

            // Khởi tạo kết nối đến DB
            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                // Gọi vào DB để chạy stored ở trên
                var employees = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                count = employees.Read<int>().FirstOrDefault();
                list = employees.Read<Employee>().ToList();
                totalPage = (int)Math.Ceiling((double)count / pageSize);
            }

            return new PagingResult
            {
                Data = list,
                TotalRecord = count,
                TotalPage = totalPage
            };
        }
    }
}
