using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace ThinkDiff
{
    interface IFilter
    {
        Bin FilterData(Bin input);
        Bin FilterUsingDatabase();
        string DatabaseQuery { get; set; }
        int Priority { get; set; }

    }
}
