using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
    public class MasterReport
    {
        MasterReportLine[] lines;

        public MasterReport(Bin[] lineBins)
        {
            lines = new MasterReportLine[lineBins.Length];

            for(int i = 0; i < lines.Length; i++)
            {
                lines[i] = new MasterReportLine(lineBins[i]);
            }
        }

        public MasterReport(Bin b)
        {
            lines = new MasterReportLine[1] { new MasterReportLine(b) };
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
