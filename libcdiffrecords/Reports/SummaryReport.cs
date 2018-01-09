using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
    public class SummaryReport
    {
        SummaryReportLine[] lines;

        public SummaryReport(Bin[] lineBins)
        {
            lines = new SummaryReportLine[lineBins.Length];

            for(int i = 0; i < lines.Length; i++)
            {
                lines[i] = new SummaryReportLine(lineBins[i]);
            }
        }

        public SummaryReport(Bin b)
        {
            lines = new SummaryReportLine[1] { new SummaryReportLine(b) };
        }

        public void WriteReport(string filename)
        {
            WriteReport(filename, ',');
        }

        public void WriteReport(string filename, char delimiter)
        {
            ReportWriter.WriteReport(filename, lines, delimiter);
        }
    }
}
