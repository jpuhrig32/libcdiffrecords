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
         DataPoint patientData;
         TubeAdditive additive;
        string legacyID;
        DateTime sampleDate;
        string comments;
        BoxLocation location;
        string accession;


        public Tube()
        {
            additive = TubeAdditive.None;

        }

        public static Tube EmptyTube
        {
            get
            {
                Tube t = new Tube();
                t.Additives = TubeAdditive.None;
                t.LegacyID = "X0000";
                t.SampleDate = DateTime.MinValue;
                t.accession = "";

                return t;
            }
        }

        public DataPoint PatientData { get => patientData; set => patientData = value; }
        public TubeAdditive Additives { get => additive; set => additive = value; }
        public string LegacyID { get => legacyID; set => legacyID = value; }
        public DateTime SampleDate { get => sampleDate; set => sampleDate = value; }
        public string Comments { get => comments; set => comments = value; }
        public BoxLocation TubeLocation { get => location; set => location = value; }
        public string TubeAccession { get => accession; set => accession = value; }

    }
}
