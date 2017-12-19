using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
    public class NAATComparisonReport
    {

        Bin[] reportBins;
        DataPoint[] naat;
        NAATComparisonReportLine[] lines;

        public NAATComparisonReport(Bin reportBin, DataPoint[] naats)
        {
            reportBins = new Bin[1] { reportBin };
            naat = naats;
            SetupReportLines();
        }

        public NAATComparisonReport(Bin[] reportBin, DataPoint[] naats)
        {
            reportBins = reportBin;
            naat = naats;
            SetupReportLines();
        }

        private void SetupReportLines()
        {
            lines = new NAATComparisonReportLine[reportBins.Length];

            for(int i = 0; i < lines.Length; i++)
            {
                lines[i] = new NAATComparisonReportLine(reportBins[i], naat, 90, ComparisonType.ByEndResult, true, NAATCountingType.OncePerPatient);
            }
        }

        

        public void WriteReport(string output)
        {
            ReportWriter.WriteReport(output, lines, ',');
        }
    }
}
