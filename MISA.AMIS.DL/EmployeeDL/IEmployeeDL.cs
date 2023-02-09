using MISA.AMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DL
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// Xuất file excel
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        /// Modified by: TTTuan 5/1/2023
        public IEnumerable<Employee> ExportExcel(string? keyword);
    }
}
