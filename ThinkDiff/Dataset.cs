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
    }
}
