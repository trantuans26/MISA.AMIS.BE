namespace MISA.AMIS.Common
{
    /// <summary>
    /// Attribute dùng để xác định 1 property là khóa chính 
    /// </summary>
    /// Modified by: TTTuan 6/1/2023
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {

    }

    /// <summary>
    /// Attribute dùng để xác định 1 property là mã
    /// </summary>
    /// Modified by: TTTuan 6/1/2023
    [AttributeUsage(AttributeTargets.Property)]
    public class CodeAttribute : Attribute
    {
        #region Field
        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public string ErrorMessage;
        #endregion

        #region Constructor
        public CodeAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        #endregion

        /// <summary>
        /// Attribute tạo tên cột phục vụ cho việc Export Excel
        /// </summary> 
        /// Modified by: TTTuan 6/1/2023
        [AttributeUsage(AttributeTargets.Property)]
        public class ExcelColumnNameAttribute : Attribute
        {
            /// <summary>
            /// Tên cột
            /// </summary>
            /// Modified by: TTTuan 6/1/2023
            public string ColumnName { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="columnName">Tên cột</param>
            /// Modified by: TTTuan 6/1/2023
            public ExcelColumnNameAttribute(string columnName)
            {
                ColumnName = columnName;
            }
        }
    }

    /// <summary>
    /// Attribute dùng để xác định 1 property là tên
    /// </summary>
    /// Modified by: TTTuan 6/1/2023
    [AttributeUsage(AttributeTargets.Property)]
    public class NameAttribute : Attribute
    {
        #region Field
        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public string ErrorMessage;
        #endregion

        #region Constructor
        public NameAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        #endregion

        /// <summary>
        /// Attribute tạo tên cột phục vụ cho việc Export Excel
        /// </summary> 
        /// Modified by: TTTuan 6/1/2023
        [AttributeUsage(AttributeTargets.Property)]
        public class ExcelColumnNameAttribute : Attribute
        {
            /// <summary>
            /// Tên cột
            /// </summary>
            /// Modified by: TTTuan 6/1/2023
            public string ColumnName { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="columnName">Tên cột</param>
            /// Modified by: TTTuan 6/1/2023
            public ExcelColumnNameAttribute(string columnName)
            {
                ColumnName = columnName;
            }
        }
    }

    /// <summary>
    /// Attribute dùng để xác định 1 property là email
    /// </summary>
    /// Modified by: TTTuan 6/1/2023
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailAttribute : Attribute
    {
        #region Field
        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public string ErrorMessage;
        #endregion

        #region Constructor
        public EmailAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        #endregion

        /// <summary>
        /// Attribute tạo tên cột phục vụ cho việc Export Excel
        /// </summary> 
        /// Modified by: TTTuan 6/1/2023
        [AttributeUsage(AttributeTargets.Property)]
        public class ExcelColumnNameAttribute : Attribute
        {
            /// <summary>
            /// Tên cột
            /// </summary>
            /// Modified by: TTTuan 6/1/2023
            public string ColumnName { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="columnName">Tên cột</param>
            /// Modified by: TTTuan 6/1/2023
            public ExcelColumnNameAttribute(string columnName)
            {
                ColumnName = columnName;
            }
        }
    }

    /// <summary>
    /// Attribure dùng để xác định 1 property không được để trống
    /// </summary>    
    /// Modified by: TTTuan 6/1/2023
    [AttributeUsage(AttributeTargets.Property)]
    public class IsNotNullOrEmptyAttribute : Attribute
    {
        #region Field
        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public string ErrorMessage;
        #endregion

        #region Constructor
        public IsNotNullOrEmptyAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        #endregion
    }

    /// <summary>
    /// Attribure dùng để xác định 1 property là chỉ số
    /// </summary>    
    /// Modified by: TTTuan 6/1/2023
    [AttributeUsage(AttributeTargets.Property)]
    public class NumbersOnlyAttribute : Attribute
    {
        #region Field
        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public string ErrorMessage;
        #endregion

        #region Constructor
        public NumbersOnlyAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        #endregion
    }

    /// <summary>
    /// Attribure dùng để xác định 1 property là số điện thoại
    /// </summary>    
    /// Modified by: TTTuan 6/1/2023
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneAttribute : Attribute
    {
        #region Field
        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public static string ErrorMessage = "Số điện thoại không hợp lệ";
        #endregion
    }
}
