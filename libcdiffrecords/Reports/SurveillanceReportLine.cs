using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
    public class SurveillanceReportLine : IReportLine
    {
        public Bin ReportBin { get; set; }


        public string[] GenerateReportHeaderLine()
        {
            List<string> header = new List<string>();

            header.Add("Unit");
            header.Add("Number of Stool Samples");
            header.Add("C. difficile positive (regardless of timing)");
            header.Add("");
            header.Add("C. difficile positive upon admission");
            header.Add("");
            header.Add("C. difficile initially negative and turned");
            header.Add("");
            header.Add("C.difficile positive, no admission sample");
            header.Add("");


            return header.ToArray();
        }

        public string[] GenerateReportLine()
        {
            throw new NotImplementedException();
        }

        public string[] GenerateReportSubHeaderLine()
        {
            throw new NotImplementedException();

        }
    }
}
