using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;


namespace libcdiffrecords.Storage
{


    public class Tube
    {
 


        public static Tube EmptyTube
        {
            get
            {
                Tube t = new Tube();
                t.Additives = "";
                t.LegacyID = "";
                t.SampleDate = DateTime.MinValue;
                t.TubeAccession = "";
                t.SampleType = "UNKNOWN";

                return t;
            }
        }

 
        public string LegacyID { get; set; }
        public DateTime SampleDate { get; set; }
        public string Comments { get; set; }
        public string ParentBox { get; set; }
        public int BoxPosition { get; set; }
        public BoxLocation TubeLocation { get; set; }
        public string TubeAccession { get; set; }
        public string TubeLabel { get; set; }
        public string SampleID { get; set; }
        public string Additives { get; set; }
        public string Notes { get; set; }
        public string SampleType { get; set; }

    }
}
