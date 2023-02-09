namespace MISA.AMIS.Common
{
    /// <summary>
    /// Kết quả trả về của API Lấy danh sách bản ghi theo bộ lọc và phân trang
    /// </summary>
    public class PagingResult<T>
    {
        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        public List<T>? Data { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public long? TotalRecord { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int? TotalPage { get; set; }
    }
}
