using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords
{
    public class DataPoint
    {
       public string mrn;
        public string patName;
        public string sampleID;
        public Sex patientSex;
        public DateTime dob;
        public int age;
        public DateTime admDate;
        public DateTime sampleDate;
        public Cdiff cdResult;
        public Toxin toxStatus;
        public string unit;


       
        public DataPoint()
        {
            mrn = "00000000";
            patName = "John Doe";
            patientSex = Sex.Male;
            dob = new DateTime(1900, 1, 1);
            admDate = new DateTime(2000, 1, 1);
            sampleDate = new DateTime(2000, 1, 1);
            cdResult = Cdiff.Negative;
            unit = "FMLH";
            Update();
        }


        public void Update()
        {
            if (admDate != DateTime.MaxValue)
            {
                age = (int)((admDate - dob).Days / 365.25);
            }
            else if(sampleDate != DateTime.MaxValue)
            {
                age = (int)((sampleDate - dob).Days / 365.25);
            }
            if (cdResult == Cdiff.Negative || cdResult == Cdiff.Unknown)
                toxStatus = Toxin.NotApplicable;

        }


    }
}
