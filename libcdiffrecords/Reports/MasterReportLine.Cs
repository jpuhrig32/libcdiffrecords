using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
    public class MasterReportLine : IReportLine
    {
        Bin repBin;

        public Bin ReportBin { get => repBin; set => repBin = value; }


        public MasterReportLine(Bin b)
        {
            repBin = b;
        }


        public string[] GenerateReportHeaderLine()
        {
            List<string> header = new List<string>();
            header.Add("Group");
            header.Add("Unique Patients");
            header.Add("Median Age");
            header.Add("Age Range");
            header.Add("Female Count");
            header.Add("Percent Female");
            header.Add("Patient Admissions");
            header.Add("Sample Count");
            header.Add("C. difficile positive (regardless of timing)");
            header.Add("");
            header.Add("C. difficile Positive upon admission");
            header.Add("");
            header.Add("C. difficile positive with prior admission sample");
            header.Add("");
            header.Add("C. difficile positive with no admission sample");
            header.Add("");
            header.Add("Admissions with two or more samples");
            header.Add("Patients with two or more admissions");
            return header.ToArray();
        }

        public string[] GenerateReportLine()
        {
            List<String> line = new List<string>();
            line.Add(repBin.Label);
            line.Add(repBin.PatientCount.ToString());
            line.Add(repBin.MedianAge.ToString());
            line.Add(repBin.AgeRange);
            line.Add(repBin.FemaleCount.ToString());
            line.Add(repBin.PercentFemale.ToString());
            line.Add(repBin.PatientAdmissionCount.ToString());
            line.Add(repBin.ItemsInBin.ToString());
            BinSummaryStatistics ats = repBin.GenerateBinSummaryStatistics();
            line.Add(ats.Positive_RegardlessOfTiming.ToString());
            line.Add(((double)ats.Positive_RegardlessOfTiming / repBin.PatientAdmissionCount * 100.0).ToString());
            line.Add(ats.PositiveOnAdmission.ToString());
            line.Add(((double)ats.PositiveOnAdmission / repBin.PatientAdmissionCount * 100.0).ToString());
            line.Add(ats.Negative_TurnedPositive.ToString());
            line.Add(((double)ats.Negative_TurnedPositive / repBin.PatientAdmissionCount * 100.0).ToString());
            line.Add(ats.Positive_NoAdmissionSample.ToString());
            line.Add(((double)ats.Positive_NoAdmissionSample/ repBin.PatientAdmissionCount * 100.0).ToString());
            line.Add(ats.AdmissionsWithTwoOrMoreSamples.ToString());
            line.Add(ats.PatientsWithTwoOrMoreAdmits.ToString());


            return line.ToArray();
        }



        public string[] GenerateReportSubHeaderLine()
        {
            List<string> header = new List<string>();
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("number");
            header.Add("%");
            header.Add("number");
            header.Add("%");
            header.Add("number");
            header.Add("%");
            header.Add("number");
            header.Add("%");
            header.Add("");
            header.Add("");



            return header.ToArray();
        }

        
    }
}
