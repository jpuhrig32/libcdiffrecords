using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords
{
    public enum ABX
    {
        Amikacin,
        Amoxicillin,
        AmoxicillinClavulanicAcid,
        Ampicillin,
        AmpicillinSublactam,
        Atovaquone,
        Azithromycin,
        Azlocillin,
        Aztreonam,
        Carbenicillin,
        Cefaclor,
        Cefadroxil,
        Cefamandole,
        Cefazolin,
        Cefdinir,
        Cefditoren,
        Cefetamet,
        Cefixime,
        Cefmetazole,
        Cefonicid,
        Cefoperazone,
        Cefotaxime,
        Cefotetan,
        Cefoxitin,
        Cefpodoxime,
        Cefprozil,
        Ceftazidime,
        Cefibuten,
        Cefitzoxime,
        Ceftriaxone,
        Cefuroxime,
        Cephalexin,
        Cephalothin,
        Cephapirin,
        Cephradine,
        Chloramphenicol,
        Cinoxacin,
        Ciprofloxacin,
        Clarithromycin,
        Clinafloxacin,
        Clindamycin,
        Colistin,
        Dapsone,
        Daptomycin,
        Dicloxacillin,
        Dirithromycin,
        Doxycycline,
        Enoxacin,
        Erythromycin,
        Ertapenem,
        Ethambutol,
        Fleroxacin,
        Fosfomycin,
        Gatifloxacin,
        Gentamicin,
        Grepafloxacin,
        Imipenem,
        Kanamycin,
        Levafloxacin,
        Linezolid,
        Lomefloxacin,
        Loracarbef,
        Meropenem,
        Methicillin,
        Metronidazole,
        Mezocillin,
        Minocycline,
        Moxalactam,
        Moxifloxacin,
        Naficillin,
        NalidixicAcid,
        Netilmicin,
        Nitrofurantoin,
        Ofloxacin,
        Oxacillin,
        Penicillin,
        Pentamidine,
        Piperacillin,
        PiperaciliinTazobactam,
        PolymyxinB,
        QuinupristinDalfopristin,
        Rifabutin,
        Rifampin,
        Rifapentine,
        Rifaximin,
        Sparfloxacin,
        Spectinomycin,
        Streptomycin,
        Teicoplanin,
        Telithromycin,
        Tetracycline,
        Ticarcillin,
        TicarcillinClavulanicAcid,
        Tigecycline,
        Tobramycin,
        Trimethoprim,
        TrimethoprimSulfamethoxazole,
        Trovafloxacin,
        Vancomycin,
        AmphotericinB,
        Clotrimazole,
        Flucytosine,
        Fluconazole,
        Itraconazole,
        Ketoconazole,
        Micafungin,
        Nystatin,
        Terbinafine,
        Voriconazole,
        Posaconazole,
        Unknown,
        None,
    };
    public enum DrugClass
    {
        Penicillin,
        Betalactam,
        Cephalosporin,
        Carbapenam,
        Monolactam,
        Fluoroquinolone,
        Glycopeptide,
        Cephalosporin_1G,
        Cephalosporin_2G,
        Cephalosporin_3G,
        Cephalosporin_4G,
        Cephalosporin_5G,
        Napthoquinone,
        Polymyxin,
        Aminoglycoside,
        Sulfonamide,
        Nitromidazole,
        Tetracycline,
        AntiMycobacterial,
        Other,
        Quinolone,
        Ansamycin,
        Lincosamide,
        Lipopeptide,
        Macrolide,
        Oxazolidinone,
        Penicillin_1G,
        Penicillin_2G,
        Aminopenicillin,
        Carboxypenicillin,
        Ureidopenicillin,
        PenicillinCombination,
        Rifamycins,
        Imidazole,
        Antifungal,
        Azole,
        Triazole,
        Echinocandin,
        Polyene,
        AntiInfective,
        AntiBacterial,
        ProtonPumpInhibitor,
        AntiNeoplastic,
        Unknown,
    };

    /// <summary>
    /// Use caution in using this for analysis.
    /// Spectrum is very general, and may not completely
    /// reflect actual usage.
    /// </summary>
    public enum ABXSpectrum
    {
        Broad,
        Narrow,
        GramPositive,
        GramNegative,
        Aerobic,
        Anaerobic,
        Mycobacterial,
        Fungal,
        Moderate,
        Antipseudomonal,
        Antiprotezoal,
        Unknown,
    };

    public enum ABXMechanism
    {
        ProteinSynthesis,
        CellWall,
        RNASynthesis,
        DNASynthesis,
        CellMembrane,
        FolateBlock,
        ErgosterolSynthesis,
        GlucanSynthesis,
        CytochromeInhibitor,
        Unknown
    };


    public class AntibioticTreatment:DrugCourse
    {
        ABXSpectrum[] spectrums;
        ABXMechanism mech;
        ABX drug;

        public ABXSpectrum[] AntibioticSpectrums
        {
            get { return spectrums; }
            set { spectrums = value; }
        }

        public ABXMechanism AntibioticMechanism
        {
            get { return mech; }
            set { mech = value; }
        }

        public override void SetFromDrug(Drug d)
        {
            spectrums = d.spectrums;
            mech = d.mech;
            drug = d.drug;
            base.SetFromDrug(d);
        }

        public AntibioticTreatment() : base()
        {
            spectrums = new ABXSpectrum[] { ABXSpectrum.Unknown };
            mech = ABXMechanism.Unknown;
            drug = ABX.Unknown;
        }
    }

    public class Drug:IComparable
    {
       public DrugClass[] abxClasses;
       public  ABXSpectrum[] spectrums;
       public ABXMechanism mech;
       public ABX drug;
       public string abbrev;
        public string name;

        public Drug()
        {

            abbrev = "UNK";
            drug = ABX.Unknown;
            abxClasses = new DrugClass[] { DrugClass.Unknown };
            spectrums = new ABXSpectrum[] { ABXSpectrum.Unknown };
            mech = ABXMechanism.Unknown;
            name = "Unknown";
        }

        public int CompareTo(object obj)
        {
            return name.CompareTo(((Drug)obj).name);
        }
    }
}
