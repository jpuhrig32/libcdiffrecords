using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;

namespace ThinkDiff.Data_Operations
{
    class ReadFromDatabase : IDataOperation
    {

        public string Query { get; set; }

        public ReadFromDatabase(string query)
        {
            Query = query;
        }

        public void DoDataOperation()
        {
            DbDataReader ddr = Settings.DataInterface.ExecuteDataReaderQuery(Query);
            


        }
    }
}
