using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using libcdiffrecords.Data;

namespace libcdiffrecords.Storage
{
    public class SampleDatabase
    {
        Application xlApp;
        _Workbook book;
        _Worksheet sheet;
        
        Bin survStools;
        Bin survSwabs;
        Bin clinNAAT;
        Bin outPatient;
        DataPoint[] rawData;


        Dictionary<string, List<BoxLocation>> stoolLocations;

        Dictionary<string, string> sampleTranslationTable;

        bool databaseInitialized = false;
       
        
        public SampleDatabase()
        {
            survStools = new Bin("survStool");
            survSwabs = new Bin("survSwab");
            clinNAAT = new Bin("ClinNaat");
            outPatient = new Bin("Outpatients");
        }
        private void InitializeExcel()
        {
            xlApp = new Application();
//            xlApp.Visible = true;

            book = (_Workbook)(xlApp.Workbooks.Add(""));
        }

        public void ConvertDatabase(string legacyStoolDB, string[] boxes, string output)
        {
            LoadLegacyDatabase(legacyStoolDB);

            DatabaseConversion dc = new DatabaseConversion();
            dc.AddData(rawData);
            string file = legacyStoolDB + "_boxesData.csv";
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine("Box, Sample ID, Date, Location,");
            for (int i = 0; i < boxes.Length; i++)
            {
                
                
                StorageBox b = BoxLoader.LoadStorageBox(boxes[i]);
                if (b != null)
                {

                    dc.AddStorageBox(b);
                    AppendBoxData(sw, b);
                }
               
            }

            rawData = dc.AttachData();
            sw.Close();
          //  WriteDatabase(output);
        }

        private void AppendBoxData(StreamWriter sw, StorageBox b)
        {
            for(int i = 0; i < b.SampleTubes.Count; i++)
            {
                if(b.SampleTubes[i].LegacyID != "")
                    sw.WriteLine(b.Name + "," + b.SampleTubes[i].LegacyID + "," + b.SampleTubes[i].SampleDate.ToShortDateString() + "," + b.SampleTubes[i].TubeLocation.LocationInBox +",");
            }
        }
      

        public void WriteDatabase(string filename)
        {
            InitializeExcel();
            //Writing the Initial Surveillance Samples
            _Worksheet stools;
            stools = (_Worksheet)book.Worksheets.Add();
            stools.Name = "Stools";
            SetupSampleDatabaseHeader(stools);

            int rowIndex = 2;
            for(int i = 0; i < rawData.Length; i++)
            {
                if(IsStool(rawData[i]))
                {
                    int rowsWritten = 0;
                   stools = WriteSample(rawData[i], stools, rowIndex, out rowsWritten);
                    rowIndex += rowsWritten;
                }
            }
            book.SaveAs(filename);
            
        }

        private _Worksheet WriteSample(DataPoint dp, _Worksheet sheet, int rowIndex, out int rowsWritten)
        {


            if (dp.Tubes.Count == 0)
            {
                sheet = WriteRow(dp, sheet, Tube.EmptyTube, rowIndex);
                rowsWritten = 1;
            }
            else
            {
                for (int i = 0; i < dp.Tubes.Count; i++)
                {
                    sheet = WriteRow(dp, sheet, dp.Tubes[i], rowIndex + i);
                   
                }
                rowsWritten = dp.Tubes.Count;
            }
          
            return sheet;

        }

        private _Worksheet WriteRow(DataPoint dp, _Worksheet sheet, Tube t, int rowIndex)
        {
            int x = rowIndex;

            sheet.Cells[x, 2] = dp.SampleID;
            sheet.Cells[x, 3] = dp.AdmissionID;
            sheet.Cells[x, 4] = dp.LegacyID;
            sheet.Cells[x, 5] = dp.PatientName;
            sheet.Cells[x, 6] = dp.MRN;

            switch (dp.PatientSex)
            {
                case Sex.Female:
                    sheet.Cells[x, 7] = "F";
                    break;
                case Sex.Male:
                    sheet.Cells[x, 7] = "M";
                    break;
                default:
                    sheet.Cells[x, 7] = "NA";
                    break;

            }

            sheet.Cells[x, 8] = dp.DateOfBirth.ToShortDateString();
            sheet.Cells[x, 9] = dp.AdmissionDate.ToShortDateString();
            sheet.Cells[x, 10] = dp.SampleDate.ToShortDateString();
            sheet.Cells[x, 11] = Utilities.TestTypeToString(dp.Test);
            sheet.Cells[x, 12] = Utilities.TestResultToString(dp.CdiffResult);
            sheet.Cells[x, 13] = Utilities.TestResultToString(dp.ToxinResult);
            sheet.Cells[x, 14] = dp.Unit;
            sheet.Cells[x, 15] = dp.Room;

            if(!t.LegacyID.Equals("X0000"))
            {
                sheet.Cells[x, 16] = "Saved";
                sheet.Cells[x, 17] = t.TubeLocation.Box;
                sheet.Cells[x, 18] = t.TubeLocation.LocationInBox;
                sheet.Cells[x, 19] = t.Additives;
                sheet.Cells[x, 24] = t.Comments;

            }
            else
            {
                sheet.Cells[x, 16] = "Not Saved";
            }

            return sheet;
        }


        private bool IsStool(DataPoint d)
        {
            return (d.Test == TestType.Clinical_Outpatient_Culture || d.Test == TestType.Surveillance_Stool_Culture || d.Test == TestType.Clinical_Inpatient_NAAT || d.Test == TestType.Surveillance_Stool_NAAT);
        }
        private bool IsSwab(DataPoint d)
        {
            return (d.Test == TestType.Surveillance_Swab_NAAT || d.Test == TestType.Surveillance_Swab_Culture);
        }

        private string GetTestString(DataPoint dp)
        {
            switch(dp.Test)
            {
                case TestType.Clinical_Outpatient_Culture:
                    return "CultureOutpatient";
                case TestType.Surveillance_Stool_Culture:
                    return "CultureStool";
                case TestType.Surveillance_Swab_Culture:
                    return "CultureSwab";
                case TestType.No_Test:
                    return "No Test";
                case TestType.Clinical_Inpatient_NAAT:
                    return "Clinical Reflex NAAT Stool";
                case TestType.Surveillance_Stool_NAAT:
                    return "Surveillance NAAT Stool";
                case TestType.Surveillance_Swab_NAAT:
                    return "Surveillance NAAT Swab";
                default:
                    return "No Test";
            }
        }

       
    







        private void SetupSampleDatabaseHeader(_Worksheet active)
        {
            active.Cells[1, 1] = "Tube Identifier";
            active.Cells[1, 2] = "Specimen Identifier";
            active.Cells[1, 3] = "Admission Identifier";
            active.Cells[1, 4] = "Legacy Identifier";
            active.Cells[1, 5] = "Patient Name";
            active.Cells[1, 6] = "MRN";
            active.Cells[1, 7] = "Sex";
            active.Cells[1, 8] = "DOB";
            active.Cells[1, 9] = "Admission Date";
            active.Cells[1, 10] = "Sample Date";
            active.Cells[1, 11] = "Test";
            active.Cells[1, 12] = "C diff Result";
            active.Cells[1, 13] = "Toxin Result";
            active.Cells[1, 14] = "Unit";
            active.Cells[1, 15] = "Room";
            active.Cells[1, 16] = "Saved Status";
            active.Cells[1, 17] = "Box Identifier";
            active.Cells[1, 18] = "Tube Position";
            active.Cells[1, 19] = "Additives";
            active.Cells[1, 20] = "Admission Identifier";
            active.Cells[1, 20] = "C diff qPCR Ct";
            active.Cells[1, 21] = "Bacterioides qPCR Ct";
            active.Cells[1, 22] = "GeneXpert Ct";
            active.Cells[1, 23] = "Comments";
        }

        public void LoadLegacyDatabase(string filename)
        {
            TabLoader tl = new TabLoader();
            rawData = tl.LoadPatientDataToPoints(filename);
            

            for(int i =0; i < rawData.Length; i++)
            {
                rawData[i].LegacyID = rawData[i].SampleID;
                rawData[i].Test = IdentifySampleType(rawData[i]);
                
                switch(rawData[i].Test)
                {
                    case TestType.Surveillance_Swab_Culture:
                        survSwabs.Add(rawData[i]);
                        break;
                    case TestType.Surveillance_Swab_NAAT:
                        survSwabs.Add(rawData[i]);
                        break;
                    case TestType.Surveillance_Stool_NAAT:
                        survStools.Add(rawData[i]);
                        break;
                    case TestType.Surveillance_Stool_Culture:
                        survStools.Add(rawData[i]);
                        break;
                    case TestType.Clinical_Inpatient_NAAT:
                        clinNAAT.Add(rawData[i]);
                        break;
                    case TestType.Clinical_Outpatient_Culture:
                        outPatient.Add(rawData[i]);
                        break;
                    default:
                        break;
                }
               


            }
            rawData = AssignAdmissionAccessionNumbers(rawData);
            rawData = AssignSampleAccessionNumbers(rawData);
            databaseInitialized = true;
        }

        

        

        
        private TestType IdentifySampleType(DataPoint dp)
        {
            switch(dp.LegacyID[0])
            {
                case 'F':
                   return TestType.Surveillance_Stool_Culture;
                case 'C':
                   return TestType.Clinical_Outpatient_Culture;
                case 'S':
                   return TestType.Surveillance_Swab_Culture;
                case 'N':
                   return TestType.Clinical_Inpatient_NAAT;
                case 'R':
                   return TestType.Surveillance_Stool_NAAT;
                case 'V':
                   return TestType.Surveillance_Swab_NAAT;
                default:
                    return TestType.No_Test;
            }
        }

        private DataPoint[] AssignAdmissionAccessionNumbers(DataPoint[] dps)
        {
            Bin bin = new Bin("", dps);

            List<Admission> dpa = bin.PatientAdmissions;

            dpa.Sort((x, y) => x.AdmissionDate.CompareTo(y.AdmissionDate));

            string admissionPrefix = "ADM";
            List<DataPoint> dataPoints = new List<DataPoint>();

            for (int i =0; i < dpa.Count; i++)
            {
                foreach (DataPoint dp in dpa[i].Points)
                {
                    DataPoint add = dp;
                    add.AdmissionID = (admissionPrefix + i.ToString().PadLeft(6, '0'));
                    dataPoints.Add(add);
                }
            }
            return dataPoints.ToArray();

        }

        private DataPoint[] AssignSampleAccessionNumbers(DataPoint[] dps)
        {
            string accPrefix = "SAM";
            List<DataPoint> data = new List<DataPoint>(dps);

            data.Sort((x, y) => x.SampleDate.CompareTo(y.SampleDate));

            for(int i = 0; i < data.Count; i++)
            {
                DataPoint temp = data[i];
                temp.AdmissionID = (accPrefix + i.ToString().PadLeft(6, '0'));
                data[i] = temp;
            }

            return data.ToArray();
        }

        private void ConstructSampleTranslationTable(DataPoint[] dps)
        {
            //Key is a concatenation of the legacy ID, Sample Date, and Unit. Value is the actual sample accession number.
            sampleTranslationTable = new Dictionary<string, string>();
            for(int i = 0; i < dps.Length; i++)
            {
                string key = dps[i].LegacyID + dps[i].SampleDate.ToShortDateString() + dps[i].Unit;

                if(!sampleTranslationTable.ContainsKey(key))
                {
                    sampleTranslationTable.Add(key, dps[i].SampleID);
                }
            }

        }


    }
}
