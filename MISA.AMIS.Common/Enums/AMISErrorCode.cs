namespace MISA.AMIS.Common
{
    public enum AMISErrorCode
    {
        /// <summary>
        /// Lỗi exception
        /// </summary>
        Exception = 0,

        /// <summary>
        /// Thêm mới thất bại
        /// </summary>
        InsertFailed = 1,

        /// <summary>
        /// Cập nhật thất bại
        /// </summary>
        UpdateFailed = 2,

        /// <summary>
        /// Xoá thất bại
        /// </summary>
        DeleteFailed = 2,

        /// <summary>
        /// Không tìm thấy
        /// </summary>
        NotFound = 2,

        /// <summary>
        /// Trùng mã
        /// </summary>
        DuplicateCode = 1062,
    }
}
