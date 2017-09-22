using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.Data.Filters
{
    public interface IFilter
    {
        string SQLQuery { get; set; }

        Bin FilterData(Bin input);

        Bin FilterFromDatabase();

    }
}
