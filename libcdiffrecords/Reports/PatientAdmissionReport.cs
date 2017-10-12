using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;
using System.IO;


namespace libcdiffrecords.Reports
{
    public class PatientAdmissionReport
    {
        Bin patientBin;
        public PatientAdmissionReport(Bin b)
        {
            patientBin = b;
        }

        public PatientAdmissionReportLine[] GenerateReportLines()
        {
            List<PatientAdmissionReportLine> lines = new List<PatientAdmissionReportLine>();

            Bin[] patientBins = DataFilter.StratifyOnPatients(patientBin);

            for(int i =0; i < patientBins.Length; i++)
            {
                PatientAdmissionReportLine parl = new PatientAdmissionReportLine(patientBins[i]);
                lines.Add(parl);
            }
            return lines.ToArray();
        }


    }
}
