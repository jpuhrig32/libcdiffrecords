using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;
using libcdiffrecords.Storage;

namespace C_Diff_Surveillance_Data_Manager
{
    public class DataManager
    {
        public static Bin StartingBin { get; set; }
        public static Bin WorkingBin { get; set; }
        public static bool BinInit { get; set; }
        
        public static StorageData Storage { get; set; }
        public static bool StorageInit { get; set; }
    }
}
