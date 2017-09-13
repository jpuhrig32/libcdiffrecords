using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.Data
{

    /// <summary>
    /// Purpose: To provide a simple set of bins for data collection from analytic functions - because Silvia almost always somehow will ask.
    /// This will likely be removed once I have time
    /// </summary>
    public static class DataTap
    {

        public static String saveFile = "./datatap.csv";
        public static Bin data = new Bin("Data");
        
    }
}
