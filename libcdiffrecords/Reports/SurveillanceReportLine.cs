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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

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
            int posTotal = 0;
            int posOnAdm = 0;
            int posTurned = 0;
            int posIndeterminate = 0;

            foreach(string key in ReportBin.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission adm in ReportBin.DataByPatientAdmissionTable[key])
                {
                    for(int i =0; i < adm.Points.Count; i++)
                    {
                        if(adm.Points[i].SampleDate >= StartDate && adm.Points[i].SampleDate <= EndDate)
                        {
                            if(adm.Points[i].CdiffResult == TestResult.Positive)
                            {
                                posTotal++;
                                switch(adm.AdmissionStatus)
                                {
                                    case AdmissionStatus.PositiveOnAdmission:
                                        posOnAdm++;
                                        break;
                                    case AdmissionStatus.NegativeOnAdmission_TurnedPositive:
                                        posTurned++;
                                        break;
                                    case AdmissionStatus.PositiveNoAdmitSample:
                                        posIndeterminate++;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            }
                        }
                    }
                    
                }
            }


            fields.Add(posTotal.ToString());
            fields.Add(((double)posTotal / (double)ReportBin.PatientAdmissionCount * 100).ToString("N2"));
            fields.Add(posOnAdm.ToString());
            fields.Add(((double)posOnAdm / (double)ReportBin.PatientAdmissionCount * 100).ToString("N2"));
            fields.Add(posTurned.ToString());
            fields.Add(((double)posTurned / (double)ReportBin.PatientAdmissionCount * 100).ToString("N2"));
            fields.Add(posIndeterminate.ToString());
            fields.Add(((double)posIndeterminate / (double)ReportBin.PatientAdmissionCount * 100).ToString("N2"));

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
