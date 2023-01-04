﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.API
{
    /// <summary>
    /// Model phản hồi người dùng
    /// </summary>
    /// Modified by: TTTuan (3/1/2023)
    public class ServiceResponse
    {
        /// <summary>
        /// Thành công hoặc thất bại
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Data response khi thành công hoặc thất bại
        /// </summary>
        public object? Data { get; set; }
    }
}
