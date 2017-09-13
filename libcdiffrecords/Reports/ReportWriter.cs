using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace libcdiffrecords.Reports
{
    public enum ReportFormat
    {
        CSV,
        TabDelimited,
    };

   public class ReportWriter
    {

        public static void WriteReport(string file, ReportFormat fmt, IReportLine[] lines)
        {
            if (lines.Length > 0)
            {
                switch(fmt)
                {
                    case ReportFormat.CSV:
                        WriteCharDelimitedFile(file, lines, ',');
                        break;
                    case ReportFormat.TabDelimited:
                        WriteCharDelimitedFile(file, lines, '\t');
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteCharDelimitedFile(string file, IReportLine[] lines, char delim)
        {
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine(BuildLine(lines[0].GenerateReportHeaderLine(), delim));
            string[] sub = lines[0].GenerateReportSubHeaderLine();
            if (sub.Length > 0)
                sw.WriteLine(BuildLine(sub, delim));

            for(int i =0; i < lines.Length; i++)
            {
                sw.WriteLine(BuildLine(lines[i].GenerateReportLine(), delim));
            }
            sw.Close();

        }

        private static String BuildLine(string[] lineParts, char delim)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < lineParts.Length; i++)
            {
                sb.Append(lineParts[i]);
                sb.Append(delim);
            }
            return sb.ToString();
        }

    }
}
