using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace libcdiffrecords.Data
{
    /// <summary>
    /// A class for reading CSV formatted data files for the dataset
    /// </summary>
    public class DatabaseFileIO
    {
        /// <summary>
        /// Reads the main dataset file - this will likely in the future be the product of database exports,
        /// but for now, the database is stored as Excel files. 
        /// </summary>
        /// <param name="file">The CSV formatted file containing all of the data</param>
        /// <returns></returns>
        public static DataPoint[] ReadDatabaseFile(string file)
        {
            List<DataPoint> data = new List<DataPoint>();

            TextFieldParser parser = new TextFieldParser(file);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            int lineCount = 0;
            Dictionary<string, int> headerTable = new Dictionary<string, int>();

            Dictionary<string, bool> standardFields = ProduceStandardFieldTable();

            while (!parser.EndOfData)
            {
                if (lineCount == 0)
                {
                    string[] header = parser.ReadFields();
                    headerTable = CreateHeaderTranslationTable(header);
                }

                else
                {
                    string[] fields = parser.ReadFields();
                    data.Add(ProcessLineToDataPoint(fields, headerTable, standardFields));
                }

                lineCount++;
            }

            parser.Close();
            return AssignAdmissionIDsToAllSamples(data.ToArray());
            ;
        }


        public static Bin ReadDatabaseFileToBin(string file)
        {
            return new Bin("Database_data", ReadDatabaseFile(file));
        }
        /// <summary>
        /// Produces a table of header values and their corresponding columns
        /// This is to allow the database file to have columns reordered without having to re-write the 
        /// reader function. 
        /// Additionally, the idea is to allow for the expansion of the dataset without having to do 
        /// a parser re-write.
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private static Dictionary<string, int> CreateHeaderTranslationTable(string[] header)
        {
            Dictionary<string, int> transTable = new Dictionary<string, int>();

            for (int i = 0; i < header.Length; i++)
                if (!transTable.ContainsKey(header[i]))
                    transTable.Add(header[i], i);

            return transTable;
        }

        /// <summary>
        /// Takes a line from the CSV reader, and assigns individual fields to those in a DataPoint
        /// </summary>
        /// <param name="line">The fields from the CSV reader</param>
        /// <param name="headerTable">A header-to-column translation table. Created with CreateHeaderTranslationTable</param>
        /// <returns>The DataPoint representation of a line in the dataset.</returns>
        private static DataPoint ProcessLineToDataPoint(string[] line, Dictionary<string, int> headerTable, Dictionary<string, bool> standardFields)
        {
            DataPoint point = new DataPoint();
            point.Initalize();
            point.SampleID = line[headerTable["Sample_ID"]];
            point.PatientName = line[headerTable["Patient_Name"]];
            point.MRN = line[headerTable["MRN"]];
            if (point.MRN == "" || point.MRN == "00000000")
                point.Flags.Add("Missing MRN");
            if (point.MRN.Length > 8)
                point.Flags.Add("MRN more than 8 digits_Not likely FMLH");
            point.PatientSex = Utilities.ParseSexFromString(line[headerTable["Sex"]]);

            DateTime temp;
            if (TryParseDate(line[headerTable["DOB"]], out temp))
                point.DateOfBirth = temp;
            else
                point.DateOfBirth = new DateTime(1901, 1, 1);

            if (point.DateOfBirth <= new DateTime(1901, 1, 1)) //Not included in the if-else block so that we can also account for "John Doe" patients, whose DOB is 1/1/1901
                point.Flags.Add("Missing DOB");

        

            if (TryParseDate(line[headerTable["Adm_Date"]], out temp))
                point.AdmissionDate = temp;
            else
            {
                point.AdmissionDate = DateTime.MaxValue;
                point.Flags.Add("Missing Admission Date");
            }

            if(point.DateOfBirth >= point.AdmissionDate)
            {
                point.DateOfBirth = new DateTime(point.DateOfBirth.Year - 100, point.DateOfBirth.Month, point.DateOfBirth.Day);
                point.Flags.Add("DOB likely 100 years off");
            }


            if (TryParseDate(line[headerTable["Sample_Date"]], out temp))
            {
                point.SampleDate = temp;
                if (point.AdmissionDate == DateTime.MaxValue) //Indicates a missing AdmissionDate. Practice is to set it to the sample date, and now flag it.
                    point.AdmissionDate = point.SampleDate;
            }
            else
            {
                point.SampleDate = DateTime.MaxValue;
                point.Flags.Add("Missing Sample Date");
            }


            point.CdiffResult = Utilities.ParseTestResult(line[headerTable["C_diff_Test_Result"]]);
            if (point.CdiffResult == TestResult.NotTested)
                point.Flags.Add("Missing C diff Result");
            point.ToxinResult = Utilities.ParseTestResult(line[headerTable["Toxin_Result"]]);
            point.Test = Utilities.ParseTestTypeFromString(line[headerTable["Test_Type"]]);
            if (point.Test == TestType.No_Test)
                point.Flags.Add("Missing Test Type");
            point.Unit = line[headerTable["Unit"]];
            if (point.Unit == "")
                point.Flags.Add("Missing Unit");
            point.Room = line[headerTable["Room"]];
            point.LegacyID = line[headerTable["Legacy_ID"]];
            if (point.LegacyID != "")
            {
                string lab = point.LegacyID[0] + point.LegacyID.Substring(1).PadLeft(4, '0');
                point.LegacyID = lab;
            }
            point.Notes = line[headerTable["Notes"]];

            //Anything that isn't in our normal set of fields goes here. Normal fields listed above |
            foreach (string field in headerTable.Keys)
            {
                if (!standardFields.ContainsKey(field))
                {
                    point.Fields.Add(field, line[headerTable[field]]);
                }
            }

            return point;
        }

        /// <summary>
        /// Assigns AdmissionID numbers to all samples. ID numbers are sequentially assigned.
        /// Only assigns to samples that have no admission ID numbers already. Admissions with existing IDs will
        /// keep the same admission ID
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private static DataPoint[] AssignAdmissionIDsToAllSamples(DataPoint[] points)
        {
            Bin global = new Bin("global", points);
            int admCt = 1;
            foreach (string key in global.DataByPatientAdmissionTable.Keys)
            {
                foreach (Admission dpa in global.DataByPatientAdmissionTable[key])
                {
                    string toAssign = "ADM_" + admCt.ToString().PadLeft(6, '0');
                    if (AdmissionIDAssigned(dpa, out int loc))
                    {
                        toAssign = dpa.Points[loc].AdmissionID;
                    }
                    else { admCt++; }
                    for (int i = 0; i < dpa.Points.Count; i++)
                    {
                        DataPoint temp = dpa.Points[i];
                        temp.AdmissionID = toAssign;
                        dpa.Points[i] = temp;
                    }

                }
            }

            return global.Data.ToArray();
        }

        public static void WriteDataToFile(DataPoint[] data, string filename, char delimiter)
        {
            if (data.Length > 0)
            {
                lock (data)
                {
                    StreamWriter sw = new StreamWriter(filename);
                    sw.WriteLine(BuildHeaderRow(data[0], delimiter));

                    for (int i = 0; i < data.Length; i++)
                    {
                        sw.WriteLine(CreateDataRow(data[i], delimiter));
                    }


                    sw.Close();
                }
            }

        }

       

        public static void WriteDataToFile(DataPoint[] data, string filename)
        {
            WriteDataToFile(data, filename, ',');
        }

        public static void WriteDataToFile(Bin b, string filename)
        {
            WriteDataToFile(b.Data.ToArray(), filename, ',');
        }
        public static void WriteDataToFile(Bin b, string filename, char delimiter)
        {
            WriteDataToFile(b.Data.ToArray(), filename, delimiter);
        }

        public static void WriteDataToFile(Bin[] b, string filename)
        {
            WriteDataToFile(b, filename, ',');
        }

        public static void WriteDataToFile(Bin[] b, string filename, char delimiter)
        {
            List<DataPoint> binData = new List<DataPoint>();
            for (int i = 0; i < b.Length; i++)
            {
                binData.AddRange(b[i].Data);
            }
            WriteDataToFile(binData.ToArray(), filename, delimiter);
        }

        public static void WriteEventBasedDataToFile(Bin b, string filename, char delim)
        {
            StreamWriter sw = new StreamWriter(filename);
            int ptCount = 1;
            WriteEventBasedHeaderRow(sw, delim);

            foreach (string key in b.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission adm in b.DataByPatientAdmissionTable[key])
                {
                    WriteAdmission(adm, ptCount, sw, delim);
                }

                ptCount++;
            }

            sw.Close();
        }

        private static void WriteAdmission(Admission adm, int id, StreamWriter sw, char delim)
        {
            List<string> admit = new List<string>();
            admit.Add(id.ToString());
            admit.Add(adm.MRN);
            admit.Add(adm.AdmissionDate.ToShortDateString());
            admit.Add(adm.Points[0].Unit);
            admit.Add(adm.Points[0].Room);
            admit.Add("1");
            admit.Add("");
            admit.Add("");
            admit.Add("");

            WriteLine(admit, sw, delim);

            for(int i = 0; i < adm.Points.Count; i++)
            {
                List<string> data = new List<string>();
                data.Add(id.ToString());
                data.Add(adm.MRN);
                data.Add(adm.Points[i].SampleDate.ToShortDateString());
                data.Add(adm.Points[0].Unit);
                data.Add(adm.Points[0].Room);
                data.Add("");
                if(IsSurveillanceTest(adm.Points[i].Test))
                {
                    if (adm.Points[i].CdiffResult == TestResult.Positive)
                        data.Add("1");
                    else
                        data.Add("0");
                    data.Add("");
                    data.Add("");
                }
                else
                {
                    data.Add("");
                    if (adm.Points[i].CdiffResult == TestResult.Positive)
                    {
                        data.Add("1");
                        if(adm.Points[i].ToxinResult == TestResult.Positive)
                        {
                            data.Add("1");
                        }
                        else
                            data.Add("0");
                    }
                    else
                    {
                        data.Add("0");
                        data.Add("0");
                    }
                }
                WriteLine(data, sw, delim);
            }

          
           
        }

        private static void WriteLine(List<string> line, StreamWriter sw, char delim)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < line.Count; i++)
            {
                sb.Append(line[i]);
                sb.Append(delim);
            }
            sw.WriteLine(sb.ToString());
        }

        private static void WriteEventBasedHeaderRow(StreamWriter sw, char delim)
        {
            List<string> head = new List<string>();
            head.Add("Patient ID");
            head.Add("Patient MRN");
            head.Add("Event Date");
            head.Add("Unit");
            head.Add("Room");
            head.Add("Admission");
            head.Add("Surveillance");
            head.Add("NAAT Result");
            head.Add("EIA Result");

            WriteLine(head, sw, delim);

        }

        private static string BuildHeaderRow(DataPoint dp, char delim)
        {
            List<String> fields = new List<string>();
            //Build header row
            StringBuilder head = new StringBuilder();
            fields.Add("Sample_ID");
            fields.Add("Patient_Name");
            fields.Add("MRN");           
            fields.Add("Sex");            
            fields.Add("DOB");            
            fields.Add("Adm_Date");            
            fields.Add("Sample_Date");            
            fields.Add("C_diff_Test_Result");            
            fields.Add("Toxin_Result");            
            fields.Add("Test_Type");            
            fields.Add("Unit");            
            fields.Add("Room");            
            fields.Add("Admission_ID");         
            fields.Add("Legacy_ID");         
            fields.Add("Notes");
            
            foreach (string key in dp.Fields.Keys)
            {
                fields.Add(key);
            }

            for(int i =0; i < fields.Count; i++)
            {
                head.Append(fields[i]);
                head.Append(delim);
            }


            return head.ToString();
        }

        private static string CreateDataRow(DataPoint dp, char delim)
        {
            StringBuilder row = new StringBuilder();
            List<string> fields = new List<string>();

            fields.Add(dp.SampleID);
            fields.Add('\"' + dp.PatientName + '\"');
            fields.Add('"' + dp.MRN + '"');
            switch(dp.PatientSex)
            {
                case Sex.Female:
                    fields.Add("F");
                    break;
                case Sex.Male:
                    fields.Add("M");
                    break;
                default:
                    fields.Add("");
                    break;
            }
            fields.Add(dp.DateOfBirth.ToShortDateString());
            fields.Add(dp.AdmissionDate.ToShortDateString());
            fields.Add(dp.SampleDate.ToShortDateString());
            fields.Add(Utilities.TestResultToString(dp.CdiffResult));
            fields.Add(Utilities.TestResultToString(dp.ToxinResult));
            fields.Add(Utilities.TestTypeToString(dp.Test));
            fields.Add(dp.Unit);
            fields.Add(dp.Room);
            fields.Add(dp.AdmissionID);
            fields.Add(dp.LegacyID);
            fields.Add(dp.Notes);

            foreach(string key in dp.Fields.Keys)
            {
                fields.Add(dp.Fields[key]);
            }

            for(int i = 0; i < fields.Count; i++)
            {
                row.Append(fields[i]);
                row.Append(delim);
            }
            return row.ToString();
        }

        private static Dictionary<string, bool> ProduceStandardFieldTable()
        {
            Dictionary<string, bool> fields = new Dictionary<string, bool>();

            fields.Add("Sample_ID", true);
            fields.Add("Patient_Name", true);
            fields.Add("MRN", true);
            fields.Add("Sex", true);
            fields.Add("DOB", true);
            fields.Add("Adm_Date", true);
            fields.Add("Sample_Date", true);
            fields.Add("C_diff_Test_Result", true);
            fields.Add("Toxin_Result", true);
            fields.Add("Test_Type", true);
            fields.Add("Unit", true);
            fields.Add("Room", true);
            fields.Add("Admission_ID", true);
            fields.Add("Legacy_ID", true);
            fields.Add("Notes", true);

            return fields;
        }

        private static bool AdmissionIDAssigned(Admission dpa, out int location)
        {
            for(int i =0; i < dpa.Points.Count; i++)
            {
                if(dpa.Points[i].AdmissionID != "")
                {
                    location = i;
                    return true;

                }
            }
            location = 0;
            return false;
           
        }

        private static bool TryParseDate(string toParse, out DateTime result)
        {
            try
            {
                result = DateTime.Parse(toParse);
            }
            catch(Exception e)
            {
                result = DateTime.MinValue;
                return false;
            }
            return true;
        }

        private static bool IsSurveillanceTest(TestType tt)
        {
            return (tt == TestType.Surveillance_Stool_Culture || tt == TestType.Surveillance_Stool_NAAT || tt == TestType.Surveillance_Swab_Culture || tt == TestType.Surveillance_Swab_NAAT);
        }
    }
}
