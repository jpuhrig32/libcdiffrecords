using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;


namespace libcdiffrecords.Storage
{

    public enum TubeAdditive
    {
        Glycerol,
        PBS,
        RNAProtect,
        None,
    };

    public class Tube
    {
 


        public static Tube EmptyTube
        {
            get
            {
                Tube t = new Tube();
                t.Additives = "";
                t.LegacyID = "X0000";
                t.SampleDate = DateTime.MinValue;
                t.TubeAccession = "";

                return t;
            }
        }

 
        public string LegacyID { get; set; }
        public DateTime SampleDate { get; set; }
        public string Comments { get; set; }
        public BoxLocation TubeLocation { get; set; }
        public string TubeAccession { get; set; }
        public string TubeLabel { get; set; }
        public string SampleID { get; set; }
        public string Additives { get; set; }

    }
}
