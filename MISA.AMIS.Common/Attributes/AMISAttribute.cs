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
    /// Attribute dùng để xác định độ dài tối đa property
    /// </summary>
    /// Modified by: TTTuan 6/1/2023
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLengthAttribute : Attribute
    {
        #region Field
        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public string ErrorMessage;

        /// <summary>
        /// Kích thước tối đa
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public int MaxLength;
        #endregion

        #region Constructor
        public MaxLengthAttribute(string errorMessage, int maxLength)
        {
            ErrorMessage = errorMessage;
            MaxLength = maxLength;
        }
        #endregion
    }

    /// <summary>
    /// Attribure dùng để xác định 1 property regex
    /// </summary>    
    /// Modified by: TTTuan 6/1/2023
    [AttributeUsage(AttributeTargets.Property)]
    public class RegexAttribute : Attribute
    {
        #region Field
        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public string ErrorMessage;

        /// <summary>
        /// Chuỗi regex
        /// </summary>
        /// Modified by: TTTuan 6/1/2023
        public string Pattern;
        #endregion

        #region Constructor
        public RegexAttribute(string errorMessage, string pattern)
        {
            ErrorMessage = errorMessage;
            Pattern = pattern;
        }
        #endregion
    }
}
