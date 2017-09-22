using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Storage;

namespace libcdiffrecords.Data
{
    public struct DataPoint
    {
        private string mrn;
        private TestResult cdresult;
        public string MRN { get => mrn; set => mrn = value.PadLeft(8, '0'); }
        public string PatientName { get; set; }
        public string SampleID { get; set; }
        public string LegacyID { get; set; }
        public string AdmissionID { get; set; }
        public string Notes { get; set; }
        public List<Tube> Tubes { get; set; }
        public List<DataFlag> Flags { get; set; }
        public TestType Test { get; set; }
        public Sex PatientSex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get => (int)((AdmissionDate - DateOfBirth).Days / 365.25); }
        public DateTime AdmissionDate { get; set; }
        public DateTime SampleDate { get; set; }
       
        public TestResult CdiffResult
        {
            get => cdresult;
            set
            {
                cdresult = value;
                if (cdresult != TestResult.Positive)
                    ToxinResult = TestResult.NotTested;
            }
        }
        public TestResult ToxinResult { get; set; }
        public string Unit { get; set; }
        public string Room { get; set; }
        public Dictionary<string, string> Fields { get; set; }

       

        public void Initalize()
        {
            MRN = "00000000";
            PatientName = "John Doe";
            SampleID = "";
            LegacyID = "";
            AdmissionID = "";
            Notes = "";
            Tubes = new List<Tube>();
            Flags = new List<DataFlag>();

            Test = TestType.No_Test;
            PatientSex = Sex.Male;
            DateOfBirth = new DateTime(1900, 1, 1);
            AdmissionDate = new DateTime(2000, 1, 1);
            SampleDate = new DateTime(2000, 1, 1);
            CdiffResult = TestResult.Negative;
            Unit = "";
            Room = "00";
            Fields = new Dictionary<string, string>();
     
        }

        public static bool operator ==(DataPoint d1, DataPoint d2)
        {
            return (d1.SampleID.Equals(d2.SampleID)) && (d1.AdmissionDate.Equals(d2.AdmissionDate)) && (d1.Unit.Equals(d2.Unit)) && (d1.MRN.Equals(d2.MRN)) && (d1.SampleDate.Equals(d2.SampleDate)) && (d1.CdiffResult.Equals(d2.CdiffResult)) && (d1.ToxinResult.Equals(d2.ToxinResult));
        }

        public static bool operator !=(DataPoint d1, DataPoint d2)
        {
            return !(d1 == d2);
        }

        


    }
}
