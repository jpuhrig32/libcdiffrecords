using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
    public enum NAATComparisonReportType
    {
        DayRange,
        All,
        ByAdmission,
    };

    public class NAATComparisonReport
    {

        Bin[] reportBins;
        DataPoint[] naat;
        NAATComparisonReportLine[] lines;
        public NAATComparisonReportType ReportType { get; set; }
        public int DayRange { get; set; } //Defaults to 90

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
            if((DayRange == -1) || ReportType == NAATComparisonReportType.All)
            {
                DayRange = -1;
                ReportType = NAATComparisonReportType.All;
            }
            for(int i = 0; i < lines.Length; i++)
            {
                if (ReportType == NAATComparisonReportType.ByAdmission)
                {
                    lines[i] = new NAATComparisonReportLine(reportBins[i], naat, DayRange, ReportType, ComparisonType.ByEndResult, true, NAATCountingType.OncePerPatient);
                }
                else
                {
                    lines[i] = new NAATComparisonReportLine(reportBins[i], naat, DayRange, ReportType, ComparisonType.ByEndResult, true, NAATCountingType.OncePerPatient);
                }
            }
        }

        

        public void WriteReport(string output)
        {
            ReportWriter.WriteReport(output, lines, ',');
        }
    }
}
