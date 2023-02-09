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
    }
}
