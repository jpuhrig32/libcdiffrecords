using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
    public class SurveillanceReport
    {
        public Bin SurveillanceData { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public SurveillanceReport(Bin b)
        {
            SurveillanceData = b;
            SurveillanceData = DataFilter.RemoveDataWithoutCDiffResult(SurveillanceData);
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MaxValue;
        }

        public SurveillanceReport(Bin b, DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            SurveillanceData = b;
            SurveillanceData = DataFilter.RemoveDataWithoutCDiffResult(SurveillanceData);
        }

        public void WriteReport(string filename)
        {
            Bin[] reports = DataFilter.StratifyOnUnits(SurveillanceData);

            List<Bin> reportBins = new List<Bin>();
            SurveillanceData.Label = "Total";
            reportBins.Add(SurveillanceData);
            reportBins.AddRange(reports);


            SurveillanceReportLine[] lines = new SurveillanceReportLine[reportBins.Count];

            for(int i =0; i < reportBins.Count; i++)
            {
                lines[i] = new SurveillanceReportLine();
                lines[i].StartDate = StartDate;
                lines[i].EndDate = EndDate;
                lines[i].ReportBin = reportBins[i];
            }
            ReportWriter.WriteReport(filename, lines);
        }

    }


}
