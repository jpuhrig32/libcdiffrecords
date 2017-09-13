using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
   public interface IReportLine
    {

        Bin ReportBin
        {
            get;
            set;
        }


        string[] GenerateReportHeaderLine();
        string[] GenerateReportSubHeaderLine();
        string[] GenerateReportLine();
    }
}
