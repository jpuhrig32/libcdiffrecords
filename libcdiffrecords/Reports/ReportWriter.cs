using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace libcdiffrecords.Reports
{
   public class ReportWriter
    {
        public static void WriteReport(string file, IReportLine[] lines, char delim)
        {
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine(BuildLine(lines[0].GenerateReportHeaderLine(), delim));
            string[] sub = lines[0].GenerateReportSubHeaderLine();
            if (sub.Length > 0)
                sw.WriteLine(BuildLine(sub, delim));

            for (int i = 0; i < lines.Length; i++)
            {
                sw.WriteLine(BuildLine(lines[i].GenerateReportLine(), delim));
            }
            sw.Close();

        }

        /// <summary>
        /// Writes a set of report lines into a CSV file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="lines"></param>
        public static void WriteReport(string file, IReportLine[] lines)
        {
            WriteReport(file, lines, ',');
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
