using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// Check trùng mã
        /// </summary>
        /// <returns>Bản ghi có mã nhân viên trùng</returns>
        /// Modified by: TTTuan 5/1/2023
        //public T CheckDuplicateCode(string recordCode, Guid? id);

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <returns></returns>
        /// Modified by: TTTuan 5/1/2023
        //public int DeleteRecordByID(Guid recordID);

        /// <summary>
        /// Lấy danh sách tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách toàn bộ bản ghi trong bảng</returns>
        /// Modified by: TTTuan 5/1/2023
        public IEnumerable<T> GetAllRecords();

        /// <summary>
        /// Lấy ra mã bản ghi mới
        /// </summary>
        /// <returns>Mã bản ghi mới</returns>
        /// Modified by: TTTuan 5/1/2023
        //public string GetNewCode();

        /// <summary>
        /// API lấy thông tin chi tiết của 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID của bản ghi</param>
        /// <returns>Thông tin của bản ghi theo ID</returns>
        /// Modified by: TTTuan 5/1/2023
        public T GetRecordByID(Guid recordID);

        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <returns></returns>
        /// Modified by: TTTuan 5/1/2023
        //public Guid InsertRecord(T obj);

        /// <summary>
        /// Sửa 1 bản ghi
        /// </summary>
        /// <returns></returns>
        /// Modified by: TTTuan 5/1/2023
        //public Guid UpdateRecordByID(Guid id, T obj);
    }
}
