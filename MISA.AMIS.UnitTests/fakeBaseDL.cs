using MISA.AMIS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL.UnitTests
{
    public class fakeBaseDL<T> : IBaseDL<T>
    {
        public bool CheckDuplicateCode(string recordCode, Guid? recordID)
        {
            return default;
        }

        public int DeleteRecordByID(Guid recordID)
        {
            return default;
        }

        public IEnumerable<T> GetAllRecords()
        {
            return default;
        }

        public string GetNewCode()
        {
            return default;
        }

        public T GetRecordByID(Guid recordID)
        {
            return default;
        }

        public int InsertRecord(T newRecord)
        {
            return default;
        }

        public int UpdateRecordByID(Guid recordID, T record)
        {
            return default;
        }
    }
}
