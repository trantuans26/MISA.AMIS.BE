using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Common
{
    public enum StatusResponse
    {
        /// <summary>
        /// Hoàn thành
        /// </summary>
        Done = 0,

        /// <summary>
        /// Lỗi đầu vào không hợp lệ
        /// </summary>
        Invalid = 1,

        /// <summary>
        /// Mã trùng
        /// </summary>
        DuplicateCode = 2,

        /// <summary>
        /// Gọi procedure trong database thất bại
        /// </summary>
        Failed = 3,
    }
}
