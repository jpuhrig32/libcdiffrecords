using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using libcdiffrecords.Data;

namespace libcdiffrecords
{
    public class TabDelimWriter
    {
      

      

        public static void WriteBinData(string filename, Bin[] bins)
        {
            StreamWriter sw = new StreamWriter(filename);

            List<string> header = new List<string>();

            header.Add("SampleID");
            // header.Add("Barcode");
            header.Add("Patient Name");
            header.Add("MRN");
            header.Add("Sex");
            header.Add("DOB");
            header.Add("Age");
            header.Add("Admit Date");
            header.Add("Sample Date");
            header.Add("C diff status");
            header.Add("Toxin Result");
            header.Add("Ribotype");
            header.Add("Unit");
            header.Add("Bed");
            header.Add("Saved Status");

            StringBuilder topline = new StringBuilder();
            for (int i = 0; i < header.Count; i++)
            {
                topline.Append(header[i] + "\t");
            }
            sw.WriteLine(topline);

            foreach(Bin b in bins)
            {
                if (b.ItemsInBin > 0)
                {
                  //  sw.WriteLine(b.Label + "\t");

                    foreach (string key in b.DataByPatientAdmissionTable.Keys)
                    {
                        foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                        {
                            foreach (DataPoint dp in dpa.Points)
                            {
                                sw.WriteLine(ProduceDataPointRow(dp));
                            }
                        }
                    }
                }
            }

            sw.Close();
        }

        public static void WriteBinAdmissionData(string filename, Bin[] bins)
        {
            StreamWriter sw = new StreamWriter(filename);

            List<string> header = new List<string>();

            header.Add("Patient Name");
            header.Add("MRN");
            header.Add("Sex");
            header.Add("DOB");
            header.Add("Age");
            header.Add("Admit Date");
            header.Add("Unit");

            StringBuilder topline = new StringBuilder();
            for (int i = 0; i < header.Count; i++)
            {
                topline.Append(header[i] + "\t");
            }
            sw.WriteLine(topline);

            foreach (Bin b in bins)
            {
                if (b.ItemsInBin > 0)
                {
                    if (b.Label != "")
                    {
                        sw.WriteLine(b.Label + "\t");
                    }

                    foreach (string key in b.DataByPatientAdmissionTable.Keys)
                    {
                        foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                        {                           
                                sw.WriteLine(ProduceAdmissionDataRow(dpa.Points[0]));
                        }
                    }
                }
            }

            sw.Close();
        }

        public static void WriteBinAdmissionData(string filename, Bin bin)
        {
            Bin[] bins = new Bin[1];
            bins[0] = bin;
            WriteBinAdmissionData(filename, bins);
        }

        public static void WriteBinData(string filename, Bin bin)
        {
            Bin[] bins = new Bin[1];
            bins[0] = bin;
            WriteBinData(filename, bins);
        }

        private static string ProduceDataPointRow(DataPoint dp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(dp.SampleID);
            sb.Append("\t");
            sb.Append(dp.PatientName);
            sb.Append("\t");
            sb.Append(dp.MRN);
            sb.Append("\t");

            if (dp.PatientSex == Sex.Male)
                sb.Append("M");
            else
                sb.Append("F");
            sb.Append("\t");

            sb.Append(dp.DateOfBirth.ToShortDateString());
            sb.Append("\t");
            sb.Append(dp.Age.ToString());
            sb.Append("\t");
            sb.Append(dp.AdmissionDate.ToShortDateString());
            sb.Append("\t");
            sb.Append(dp.SampleDate.ToShortDateString());
            sb.Append("\t");

            sb.Append(Utilities.TestResultToString(dp.CdiffResult));
            sb.Append("\t");

            sb.Append(Utilities.TestResultToString(dp.ToxinResult));
            sb.Append("\t");

            sb.Append("\t");

            sb.Append(dp.Unit);
            sb.Append("\t");
            sb.Append(dp.Room);
            sb.Append("\t");
            if (dp.Notes != null)
            {
                sb.Append(dp.Notes);
                sb.Append("\t");
            }

            return sb.ToString();
        }


        private static string ProduceAdmissionDataRow(DataPoint dp)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append(dp.PatientName);
            sb.Append("\t");
            sb.Append(dp.MRN);
            sb.Append("\t");

            if (dp.PatientSex == Sex.Male)
                sb.Append("M");
            else
                sb.Append("F");
            sb.Append("\t");

            sb.Append(dp.DateOfBirth.ToShortDateString());
            sb.Append("\t");
            sb.Append(dp.Age.ToString());
            sb.Append("\t");
            sb.Append(dp.AdmissionDate.ToShortDateString());
            sb.Append("\t");

            sb.Append(dp.Unit);
            sb.Append("\t");

            return sb.ToString();
        }

        public static void WriteStratificationReport(string filename, SurveillanceReportLine[] lines)
        {
            StreamWriter sw = new StreamWriter(filename);

            sw.WriteLine("Label\tNumber of Samples\tNumber of Patients\tNumber of Patient Admissions\tNumber Positive (regardless of timing)\t\tNumber Positive on Admission\t\tNumber Positive during Stay\t\t Number Positive without admission sample\t");
            sw.WriteLine("\t\t\tNumber\tPercent\tNumber\tPercent\tNumber\tPercent\tNumber\tPercent");

            for(int i =0; i < lines.Length; i++)
            {
                if (lines[i].SampleCount > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(lines[i].Label);
                    sb.Append("\t");

                    sb.Append(lines[i].SampleCount);
                    sb.Append("\t");

                    sb.Append(lines[i].PatientCount);
                    sb.Append("\t");

                    sb.Append(lines[i].PatientAdmissionsCount);
                    sb.Append("\t");

                    sb.Append(lines[i].PositiveSamples);
                    sb.Append("\t");

                    sb.Append(lines[i].PercentPositive);
                    sb.Append("\t");

                    sb.Append(lines[i].PositiveOnAdmission);
                    sb.Append("\t");

                    sb.Append(lines[i].PercentPositiveOnAdmission);
                    sb.Append("\t");

                    sb.Append(lines[i].PositiveDuringStay);
                    sb.Append("\t");

                    sb.Append(lines[i].PercentPositiveDuringStay);
                    sb.Append("\t");

                    sb.Append(lines[i].PositiveNoAdmissionSample);
                    sb.Append("\t");

                    sb.Append(lines[i].PercentPositiveNoAdmission);
                    sb.Append("\t");

                    sw.WriteLine(sb.ToString());
                }
            }

            sw.Close();
        }

        public static void WriteStringListReport(string filename, string[] lines)
        {
            StreamWriter sw = new StreamWriter(filename);

            for(int i = 0; i < lines.Length; i++)
            {
                sw.WriteLine(lines[i]);
            }

            sw.Close();
        }
    }
}
