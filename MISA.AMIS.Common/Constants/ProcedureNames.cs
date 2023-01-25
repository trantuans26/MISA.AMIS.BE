using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Common
{
    /// <summary>
    /// Tên các thủ tục
    /// </summary>
    /// Modified by: TTTuan 6/1/2023
    public class ProcedureNames
    {
        /// <summary>
        /// Tên proc lấy tất cả bản ghi
        /// </summary>
        public static readonly string GET_ALL = "Proc_{0}_GetAll";

        /// <summary>
        /// Tên proc lấy 1 bản ghi theo ID
        /// </summary>
        public static readonly string GET_BY_ID = "Proc_{0}_GetByID";

        /// <summary>
        /// Tên proc lấy danh sách bản ghi theo bộ lọc
        /// </summary>
        public static readonly string GET_BY_FILTER = "Proc_{0}_GetByFilter";

        /// <summary>
        /// Tên proc thêm mới bản ghi
        /// </summary>
        public static readonly string INSERT = "Proc_{0}_Insert";

        /// <summary>
        /// Tên proc sửa 1 bản ghi
        /// </summary>
        public static readonly string UPDATE_BY_ID = "Proc_{0}_UpdateByID";

        /// <summary>
        /// Tên proc xoá 1 bản ghi
        /// </summary>
        public static readonly string DELETE_BY_ID = "Proc_{0}_DeleteByID";

        /// <summary>
        /// Tên proc xoá 1 bản ghi
        /// </summary>
        public static readonly string DELETE_BY_IDS = "Proc_{0}_DeleteByIDs";

        /// <summary>
        /// Tên proc lấy mã mới
        /// </summary>
        public static readonly string GET_NEW_CODE = "Proc_{0}_GetNewCode";

        /// <summary>
        /// Tên proc check mã trùng
        /// </summary>
        public static readonly string CHECK_DUPLICATE_CODE = "Proc_{0}_CheckDuplicateCode";

    }
}
