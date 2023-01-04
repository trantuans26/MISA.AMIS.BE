using System.ComponentModel.DataAnnotations;

namespace MISA.AMIS.API
{
    /// <summary>
    /// Class định nghĩa attribute là khoá chính
    /// </summary>
    /// TTTuan (1/3/2023)
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {

    }

    /// <summary>
    /// Class định nghĩa attribute không được bỏ trống
    /// </summary>
    /// TTTuan (1/3/2023)
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmptyAttribute : Attribute 
    {
        public string MessageError;

        public NotEmptyAttribute(string messageError) 
        {
            MessageError = messageError;
        }
    }

    /// <summary>
    /// Class định nghĩa attribute bị trùng mã
    /// </summary>
    /// TTTuan (1/3/2023)
    [AttributeUsage(AttributeTargets.Property)]
    public class DuplicateCodeAttribute : Attribute
    {

    }
}
