using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
    public class PatientAdmissionReportLine : IReportLine
    {
        public Bin ReportBin { get; set; }
        private Admission adm;

        public PatientAdmissionReportLine(Bin b)
        {
            ReportBin = b;
        }

        public string[] GenerateReportHeaderLine()
        {
            List<string> header = new List<string>();

            header.Add("Patient Name");
            header.Add("MRN");
            header.Add("Gender");
            header.Add("Age");
            header.Add("Sample Count");
            header.Add("Negative Samples");
            header.Add("Positve Samples");

            return header.ToArray();
        }

        public string[] GenerateReportLine()
        {
            List<string> line = new List<string>();
            int count = 0;
            foreach (string key in ReportBin.DataByPatientAdmissionTable.Keys)
            {

                


                int pos = 0;
                int neg = 0;
                foreach (Admission adm in ReportBin.DataByPatientAdmissionTable[key])
                {
                    if (count == 0)
                    {
                        line.Add('\"' + adm.PatientName + '\"');
                        line.Add(adm.MRN);
                        line.Add(Utilities.PatientSexToString(adm.Points[0].PatientSex));
                        line.Add(((int)((adm.AdmissionDate - adm.Points[0].DateOfBirth).Days / 365.25)).ToString());
                        line.Add(adm.AdmissionDate.ToShortDateString());
                        line.Add(adm.unit);
                        line.Add(adm.Points.Count.ToString());
                    }
                    for (int i = 0; i < adm.Points.Count; i++)
                    {
                        if (adm.Points[i].CdiffResult == TestResult.Positive)
                            pos++;
                        else
                            neg++;
                    }
                    count++;
                }
            
                    line.Add(neg.ToString());
                    line.Add(pos.ToString());

                

            }
            return line.ToArray();

        }

        public string[] GenerateReportSubHeaderLine()
        {
            return new string[] { };
        }
    }
}
