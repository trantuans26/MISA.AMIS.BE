using MISA.AMIS.Common;
using MISA.AMIS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL
{
    public class DepartmentDL : BaseDL<Department>, IDepartmentDL
    {
        #region Field
        private IConnectionDL _connectionDL;
        #endregion

        #region Constructor
        public DepartmentDL(IConnectionDL connectionDL) :base(connectionDL) 
        {
            _connectionDL = connectionDL;
        }
        #endregion
    }
}
