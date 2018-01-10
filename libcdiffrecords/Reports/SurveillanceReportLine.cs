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
            List<string> fields = new List<string>();
            fields.Add(ReportBin.Label);
            fields.Add(ReportBin.PatientAdmissionCount.ToString());
            BinSummaryStatistics bss = ReportBin.GenerateBinSummaryStatistics();
            fields.Add(bss.Positive_RegardlessOfTiming.ToString());
            fields.Add(((double)bss.Positive_RegardlessOfTiming / (double)ReportBin.PatientAdmissionCount * 100).ToString("N2"));
            fields.Add(bss.PositiveOnAdmission.ToString());
            fields.Add(((double)bss.PositiveOnAdmission / (double)ReportBin.PatientAdmissionCount * 100).ToString("N2"));
            fields.Add(bss.Negative_TurnedPositive.ToString());
            fields.Add(((double)bss.Negative_TurnedPositive / (double)ReportBin.PatientAdmissionCount * 100).ToString("N2"));
            fields.Add(bss.Positive_NoAdmissionSample.ToString());
            fields.Add(((double)bss.Positive_NoAdmissionSample / (double)ReportBin.PatientAdmissionCount * 100).ToString("N2"));

            return fields.ToArray();
        }

        public string[] GenerateReportSubHeaderLine()
        {
            List<string> header = new List<string>();

            header.Add("");
            header.Add("");
            header.Add("Number");
            header.Add("%");
            header.Add("Number");
            header.Add("%");
            header.Add("Number");
            header.Add("%");
            header.Add("Number");
            header.Add("%");


            return header.ToArray();
        }
    }
}
