namespace MISA.AMIS.Common
{
    /// <summary>
    /// Kết quả trả về của API Lấy danh sách nhân viên theo bộ lọc và phân trang
    /// </summary>
    public class PagingResult
    {
        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        public List<Employee> Data { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public long TotalCount { get; set; }
    }
}
