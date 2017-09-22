using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using libcdiffrecords.ErrorHandling;
using libcdiffrecords.Data;

namespace libcdiffrecords
{
    public class TabLoader
    {

        public DataPoint[] LoadPatientDataToPoints(string path)
        {
            List<DataPoint> points = new List<DataPoint>();

            if (File.Exists(path))
            {
               StreamReader file = new StreamReader(path);
                string line;
                int lineCount = 0;
               

                    string[] fields = new string[] { };
                    while ((line = file.ReadLine()) != null)
                    {
                        if (lineCount == 0)
                        {
                            char[] splitChar = new char[] { '\t' };
                            fields = line.Trim().Split(splitChar);
                            if (fields.Length >= 11)
                            {
                                if (fields.Length > 1)
                                {
                                    if (!fields[2].ToUpper().Contains("MRN") || !fields[1].ToUpper().Contains("NAME"))
                                    {
                                        throw new FileNotPatientDataException("File does not contain an MRN or Patient Name field");
                                    }
                                }
                            }
                            else
                            {
                                throw new FileNotPatientDataException("File does not contain enough fields to construct patient records");
                            }
                        }
                        if (lineCount > 0)
                        {
                             points.Add(ProcessLineToDataPoint(line));  
                        }

                        lineCount++;

                    }


                file.Close();
            }
            else
            {
                throw new FileNotFoundException("Patient record file does not exist!");
            }

            return points.ToArray();
        }

      


        private DataPoint ProcessLineToDataPoint(string line)
        {
            DataPoint point = new DataPoint();
            point.Initalize();
            char[] split = new char[] { '\t' };
            string[] parts = line.Trim().Split(split);
            if (parts.Length >= 11)
            {
                point.SampleID = (parts[0].Trim()).ToUpper();
                Char t = point.SampleID[0];

                TestType tt = TestType.No_Test;
                switch(t)
                {
                    case 'F':
                        tt = TestType.Surveillance_Stool_Culture;
                        break;
                    case 'S':
                        tt = TestType.Surveillance_Swab_Culture;
                        break;
                    case 'N':
                        tt = TestType.Clinical_Inpatient_NAAT;
                        break;
                    case 'C':
                        tt = TestType.Clinical_Outpatient_Culture;
                        break;
                    case 'R':
                        tt = TestType.Surveillance_Stool_NAAT;
                        break;
                    case 'V':
                        tt = TestType.Surveillance_Swab_NAAT;
                        break;
                }
                point.Test = tt;

                point.PatientName = parts[1].Trim();
                point.MRN = parts[2].Trim().PadLeft(8, '0');
                point.PatientSex = Utilities.ParseSexFromString(parts[3].Trim());
                point.DateOfBirth = DateTime.Parse(parts[4].Trim());
                point.AdmissionDate = DateTime.Parse(parts[5].Trim());
                point.SampleDate = DateTime.Parse(parts[6].Trim());
                point.CdiffResult = Utilities.ParseTestResult(parts[7].Trim());
                point.ToxinResult = Utilities.ParseTestResult(parts[8].Trim());
                point.Unit = parts[10].Trim();
                if (parts.Length >= 12)
                {
                    point.Room = parts[11].Trim();
                    


                }

                if(parts.Length >= 13)
                    point.Notes = parts[12].Trim(); 


              
            }
            return point;
        }

       

      

        public DataPoint[] LoadNAATResults(string path)
        {
            List<DataPoint> points = new List<DataPoint>();

            if (File.Exists(path))
            {
                StreamReader file = new StreamReader(path);
                string line;
                int lineCount = 0;


                string[] fields = new string[] { };
                while ((line = file.ReadLine()) != null)
                {
                    if (lineCount == 0)
                    {
                        char[] splitChar = new char[] { '\t' };
                        fields = line.Trim().Split(splitChar);
                            if (fields.Length > 1)
                            {
                                if (!fields[2].ToUpper().Contains("MRN") || !fields[0].ToUpper().Contains("NAME"))
                                {
                                    throw new FileNotPatientDataException("File does not contain an MRN or Patient Name field");
                                }
                            }
                        else
                        {
                            throw new FileNotPatientDataException("File does not contain enough fields to construct patient records");
                        }
                    }
                    if (lineCount > 0)
                    {
                        points.Add(ProcessLineToNAATDataPoint(line));
                    }

                    lineCount++;

                }


                file.Close();
            }
            else
            {
                throw new FileNotFoundException("Patient record file does not exist!");
            }

            return points.ToArray();

        }

        private DataPoint ProcessLineToNAATDataPoint(string line)
        {
            DataPoint point = new DataPoint();
            point.Initalize();
            char[] split = new char[] { '\t' };
            string[] parts = line.Trim().Split(split);
            if (parts.Length >= 7)
            {
                point.PatientName = parts[0].Trim();
                point.DateOfBirth = DateTime.Parse(parts[1].Trim());
                point.MRN = parts[2].Trim().PadLeft(8, '0');
                point.Unit = parts[4].Trim();
                point.SampleDate = DateTime.Parse(parts[5].Trim());
                point.CdiffResult = Utilities.ParseTestResult(parts[6].Trim());
                point.ToxinResult = Utilities.ParseTestResult(parts[7].Trim());

            }
            return point;
        }

        public DataPoint[] LoadSurveillanceData_NF(string file)
        {
            List<DataPoint> data = new List<DataPoint>();
            


            return data.ToArray();

        }

        public DataPoint[] LoadDynacareSurveillanceData(string file)
        {
            List<DataPoint> data = new List<DataPoint>();
            StreamReader sr = new StreamReader(file);

            string line = "";
            int lineCount = 0;
            while ((line = sr.ReadLine()) != null)
            {

                if(lineCount > 0)
                {
                    DataPoint dp = ProcessDynacareSurveillanceDataToPoint(line);
                    if (dp.MRN != null)
                        data.Add(dp);
                }
            }

            sr.Close();

            return data.ToArray();
        }

        private DataPoint ProcessDynacareSurveillanceDataToPoint(string line)
        {
            DataPoint dp = new DataPoint();
            char[] delim = new char[] { '\t' };
            string[] parts = line.Split(delim);

            if(!parts[0].Equals(""))
            {
                dp.Unit = parts[2].Trim();
                dp.SampleDate = DateTime.Parse(parts[3].Trim());
                dp.AdmissionDate = DateTime.Parse(parts[4].Trim());
                dp.MRN = parts[6].Trim().PadLeft(8, '0');
                dp.PatientName = parts[7].Trim();
                dp.CdiffResult = Utilities.ParseTestResult(parts[10].Trim());
                    
            }


            return new DataPoint();
        }
    }
}
