using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using MISA.AMIS.Common;
using MISA.AMIS.BL;

namespace MISA.AMIS.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : BasesController<Department>
    {
        #region Constructor
        public DepartmentsController(IBaseBL<Department> baseBL) : base(baseBL)
        {
        }
        #endregion
    }
}
