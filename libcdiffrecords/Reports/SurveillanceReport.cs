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
        Bin survDataBin;
        
        public SurveillanceReport(Bin b)
        {
            survDataBin = b;
            survDataBin = DataFilter.RemoveDataWithoutCDiffResult(survDataBin);   
        }

        public void WriteReport(string filename)
        {
            Bin[] reports = DataFilter.StratifyOnUnits(survDataBin);

            List<Bin> reportBins = new List<Bin>();
            survDataBin.Label = "Total";
            reportBins.Add(survDataBin);
            reportBins.AddRange(reports);


            SurveillanceReportLine[] lines = new SurveillanceReportLine[reportBins.Count];

            for(int i =0; i < reportBins.Count; i++)
            {
                lines[i] = new SurveillanceReportLine();
                lines[i].ReportBin = reportBins[i];
            }
            ReportWriter.WriteReport(filename, lines);
        }

    }


}
