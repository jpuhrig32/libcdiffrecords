using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;
using System.Data;
using libcdiffrecords;

namespace ThinkDiff
{
   public class AppData
    {
        public static Stack<Bin> Undo { get; set; }
        public static Stack<Bin> Redo { get; set; }
        public static Bin WorkingBin { get; set; }


        public static void Initialize()
        {
            Undo = new Stack<Bin>();
            Redo = new Stack<Bin>();
            WorkingBin = new Bin("Empty");
        }
        public static void AddNewData(Bin newBin)
        {
           if(WorkingBin != null)
            {
                Undo.Push(WorkingBin);
            }

            WorkingBin = newBin;
        }

        public static void UndoBinAction()
        {
            if (WorkingBin != null)
            {
                Redo.Push(WorkingBin);
                WorkingBin = Undo.Pop();
            }
        }

        public static void RedoBinAction()
        {
            if (WorkingBin != null)
            {
                Undo.Push(WorkingBin);
                WorkingBin = Redo.Pop();
            }
        }

       public static DataTable BuildDataSetFromWorkingBin()
        {
            DataTable dt = new DataTable();

            List<DataColumn> dcs = new List<DataColumn>();

            dcs.Add(new DataColumn("Sample ID"));
            dcs.Add(new DataColumn("Admission ID"));
            dcs.Add(new DataColumn("Patient Name"));
            dcs.Add(new DataColumn("MRN"));
            dcs.Add(new DataColumn("Sex"));
            dcs.Add(new DataColumn("Age", Type.GetType("System.Int32")));
            dcs.Add(new DataColumn("DOB", Type.GetType("System.DateTime")));
            dcs.Add(new DataColumn("Admission Date", Type.GetType("System.DateTime")));
            dcs.Add(new DataColumn("Sample Date", Type.GetType("System.DateTime")));
            dcs.Add(new DataColumn("C. diff Result"));
            dcs.Add(new DataColumn("Toxin Result"));
            dcs.Add(new DataColumn("Test Type"));
            dcs.Add(new DataColumn("Unit"));
            dcs.Add(new DataColumn("Room"));
            dcs.Add(new DataColumn("Legacy ID"));
            dcs.Add(new DataColumn("Notes"));
            
            if(WorkingBin.ItemsInBin > 0)
            {
                foreach(string key in WorkingBin.Data[0].Fields.Keys)
                {
                    dcs.Add(new DataColumn(key));
                }
            }

            for(int i =0; i < dcs.Count; i++)
            {
                dt.Columns.Add(dcs[i]);  
            }

            for(int i = 0; i < WorkingBin.Data.Count; i++)
            {
                dt.Rows.Add(CreateDataRowFromDataPoint(WorkingBin.Data[i], dt));
            }

            return dt;
        }

        public async static Task<DataTable> BuildDatasetFromWorkingBinAsync(ThinkDiffMainForm tdmf)
        {
            return await Task.Run(() =>
            {
           
                DataTable dt = new DataTable();

                List<DataColumn> dcs = new List<DataColumn>();

                dcs.Add(new DataColumn("Sample ID"));
                dcs.Add(new DataColumn("Admission ID"));
                dcs.Add(new DataColumn("Patient Name"));
                dcs.Add(new DataColumn("MRN"));
                dcs.Add(new DataColumn("Sex"));
                dcs.Add(new DataColumn("Age", Type.GetType("System.Int32")));
                dcs.Add(new DataColumn("DOB", Type.GetType("System.DateTime")));
                dcs.Add(new DataColumn("Admission Date", Type.GetType("System.DateTime")));
                dcs.Add(new DataColumn("Sample Date", Type.GetType("System.DateTime")));
                dcs.Add(new DataColumn("C. diff Result"));
                dcs.Add(new DataColumn("Toxin Result"));
                dcs.Add(new DataColumn("Test Type"));
                dcs.Add(new DataColumn("Unit"));
                dcs.Add(new DataColumn("Room"));
                dcs.Add(new DataColumn("Legacy ID"));
                dcs.Add(new DataColumn("Notes"));

                if (WorkingBin.ItemsInBin > 0)
                {
                    foreach (string key in WorkingBin.Data[0].Fields.Keys)
                    {
                        dcs.Add(new DataColumn(key));
                    }
                }

                for (int i = 0; i < dcs.Count; i++)
                {
                    dt.Columns.Add(dcs[i]);
                }

                for (int i = 0; i < WorkingBin.Data.Count; i++)
                {
                    dt.Rows.Add(CreateDataRowFromDataPoint(WorkingBin.Data[i], dt));
              
                }

                return dt;
            });
        }

        private static DataRow CreateDataRowFromDataPoint(DataPoint dp, DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["Sample ID"] = dp.SampleID;
            dr["Admission ID"] = dp.AdmissionID;
            dr["Patient Name"] = dp.PatientName;
            dr["MRN"] = dp.MRN;
            dr["Sex"] = Utilities.PatientSexToString(dp.PatientSex);
            dr["Age"] = dp.Age;
            dr["DOB"] = dp.DateOfBirth;
            dr["Admission Date"] = dp.AdmissionDate;
            dr["Sample Date"] = dp.SampleDate;
            dr["C. diff Result"] = Utilities.TestResultToString(dp.CdiffResult);
            dr["Toxin Result"] = Utilities.TestResultToString(dp.ToxinResult);
            dr["Test Type"] = Utilities.TestTypeToString(dp.Test);
            dr["Unit"] = dp.Unit;
            dr["Room"] = dp.Room;

            foreach (string key in dp.Fields.Keys)
            {
                dr[key] = dp.Fields[key];
            }


            return dr;
        }

        public async static void WriteBinToDatabaseAsync(Bin b)
        {
            await Task.Run(async () =>
            {
                Dictionary<string, string> fieldType = await Settings.DataInterface.GetTableFields("surveillance_data");
                Dictionary<string, bool> standardFields = ProduceStandardFieldTable();
                List<string> binDataQueries = new List<string>();

                if(b.Data.Count > 0)
                {
                    List<string> missingFields = new List<string>();
                    foreach(string key in fieldType.Keys)
                        if (!b.Data[0].Fields.ContainsKey(key) && !standardFields.ContainsKey(key))
                            missingFields.Add(key);
                    if(missingFields.Count > 0)
                        AddMissingFieldColumnsToDatabase(missingFields.ToArray(), Settings.FieldCharWidth);
                    
                }
                for(int i = 0; i < b.Data.Count; i++)
                {
                    binDataQueries.Add(BuildWriteQueryForDataPoint(b.Data[i]));
                }
                await Settings.DataInterface.ExecuteMultipleNonQueriesAsync(binDataQueries.ToArray());
                Settings.DataInterface.CloseConnection();
            });
        }

        private async static void AddMissingFieldColumnsToDatabase(string[] fieldCols, int fieldSize)
        {
            await Task.Run(async () =>
            {
                string[] queries = new string[fieldCols.Length];
                for (int i = 0; i < fieldCols.Length; i++)
                {
                    queries[i] = "ALTER TABLE surveillance_data ADD " + fieldCols[i] + " VARCHAR(" + fieldSize.ToString() + ");";

                }
               await Settings.DataInterface.ExecuteMultipleNonQueriesAsync(queries);
            });
        }
        private static string BuildWriteQueryForDataPoint(DataPoint dp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( "INSERT OR REPLACE INTO surveillance_data VALUES(");
            List<string> fields = new List<string>();
            fields.Add(dp.SampleID);
            fields.Add(dp.AdmissionID);
            fields.Add(dp.PatientName);
            fields.Add(dp.MRN);
            fields.Add(Utilities.PatientSexToString(dp.PatientSex));
            fields.Add(dp.DateOfBirth.ToShortDateString());
            fields.Add(dp.AdmissionDate.ToShortDateString());
            fields.Add(dp.SampleDate.ToShortDateString());
            fields.Add(Utilities.TestResultToString(dp.CdiffResult));
            fields.Add(Utilities.TestResultToString(dp.ToxinResult));
            fields.Add(Utilities.TestTypeToString(dp.Test));
            fields.Add(dp.Unit);
            fields.Add(dp.Room);
            fields.Add(dp.LegacyID);
            fields.Add(dp.Notes);

            foreach(string key in dp.Fields.Keys)
            {
                fields.Add(dp.Fields[key]);
            }

            for(int i = 0; i < fields.Count; i++)
            {
                sb.Append("'");
                sb.Append(fields[i]);
                sb.Append("'");
                if(i < fields.Count -1)
                    sb.Append(",");
            }
            sb.Append(");");
            return sb.ToString();
        }

        private static Dictionary<string, bool> ProduceStandardFieldTable()
        {
            Dictionary<string, bool> fields = new Dictionary<string, bool>();

            fields.Add("sample_id", true);
            fields.Add("patient_name", true);
            fields.Add("mrn", true);
            fields.Add("sex", true);
            fields.Add("dob", true);
            fields.Add("adm_date", true);
            fields.Add("sample_date", true);
            fields.Add("c_diff_test_result", true);
            fields.Add("toxin_result", true);
            fields.Add("test_type", true);
            fields.Add("unit", true);
            fields.Add("room", true);
            fields.Add("admission_id", true);
            fields.Add("legacy_id", true);
            fields.Add("notes", true);

            return fields;
        }
    }
}
