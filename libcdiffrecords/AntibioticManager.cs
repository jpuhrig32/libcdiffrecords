using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace libcdiffrecords
{
    public class AntibioticManager
    {
        static Dictionary<string, Drug> antibiotics;
        static Dictionary<string, Drug> drugsByName;
        static bool init = false;

        /// <summary>
        /// Initializes a list of common antibiotics into
        /// Antibiotic objects. 
        /// 
        /// WARNING: LIST IS INCOMPLETE!
        /// MISSING ENTRIES WILL DEFAULT TO UNKNOWN
        /// WITH THE ABX ABBREVIATION.
        /// </summary>
        public static void InitializeAntibioticList()
        {
            antibiotics = new Dictionary<string, Drug>();
            drugsByName = new Dictionary<string, Drug>();

            Drug amik = new Drug();
            amik.abbrev = "AMK";
            amik.name = "Amikacin";
            amik.drug = ABX.Amikacin;
            amik.abxClasses = new DrugClass[] { DrugClass.Aminoglycoside };
            amik.spectrums = new ABXSpectrum[] { ABXSpectrum.GramNegative };
            amik.mech = ABXMechanism.ProteinSynthesis;
            antibiotics.Add("AMK", amik);

            Drug amox = new Drug();
            amox.abbrev = "AMX";
            amox.name = "Amoxicillin";
            amox.drug = ABX.Amoxicillin;
            amox.abxClasses = new DrugClass[] { DrugClass.Betalactam, DrugClass.Penicillin, DrugClass.Aminopenicillin };
            amox.spectrums = new ABXSpectrum[] { ABXSpectrum.GramNegative, ABXSpectrum.GramPositive, ABXSpectrum.Moderate };
            amox.mech = ABXMechanism.CellWall;
            antibiotics.Add("AMX", amox);

            Drug atov = new Drug();
            atov.abbrev = "ATO";
            atov.name = "Atovaquone";
            atov.drug = ABX.Atovaquone;
            atov.abxClasses = new DrugClass[] { DrugClass.Napthoquinone };
            atov.spectrums = new ABXSpectrum[] { ABXSpectrum.Narrow, ABXSpectrum.Antiprotezoal };
            atov.mech = ABXMechanism.CytochromeInhibitor;
            antibiotics.Add("ATO", atov);

            Drug clavamox = new Drug();
            clavamox.abbrev = "AMC";
            clavamox.name = "Amoxicillin-Clavulanic Acid";
            clavamox.drug = ABX.AmoxicillinClavulanicAcid;
            clavamox.abxClasses = new DrugClass[] { DrugClass.Betalactam, DrugClass.Penicillin, DrugClass.Aminopenicillin, DrugClass.PenicillinCombination };
            clavamox.spectrums = new ABXSpectrum[] { ABXSpectrum.GramNegative, ABXSpectrum.GramPositive, ABXSpectrum.Moderate };
            clavamox.mech = ABXMechanism.CellWall;
            antibiotics.Add("AMC", clavamox);

            Drug amp = new Drug();
            amp.abbrev = "AMP";
            amp.name = "Ampicillin";
            amp.drug = ABX.Ampicillin;
            amp.abxClasses = new DrugClass[] { DrugClass.Betalactam, DrugClass.Penicillin, DrugClass.Aminopenicillin };
            amp.spectrums = new ABXSpectrum[] { ABXSpectrum.GramNegative, ABXSpectrum.GramPositive, ABXSpectrum.Moderate };
            amp.mech = ABXMechanism.CellWall;
            antibiotics.Add("AMP", amp);

            Drug ampsub = new Drug();
            ampsub.abbrev = "SAM";
            ampsub.name = "Ampicillin Sublactam";
            amp.drug = ABX.AmpicillinSublactam;
            amp.abxClasses = new DrugClass[] { DrugClass.Betalactam, DrugClass.Penicillin, DrugClass.Aminopenicillin, DrugClass.PenicillinCombination };
            amp.spectrums = new ABXSpectrum[] { ABXSpectrum.GramNegative, ABXSpectrum.GramPositive, ABXSpectrum.Moderate };
            amp.mech = ABXMechanism.CellWall;
            antibiotics.Add("SAM", ampsub);

            Drug azith = new Drug();
            azith.abbrev = "AZM";
            azith.name = "Azithromycin";
            azith.drug = ABX.Azithromycin;
            azith.abxClasses = new DrugClass[] { DrugClass.Macrolide };
            azith.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            azith.mech = ABXMechanism.ProteinSynthesis;
            antibiotics.Add("AZM", azith);

            Drug azloc = new Drug();
            azloc.abbrev = "AZL";
            azloc.name = "Azlocillin";
            azloc.drug = ABX.Azlocillin;
            azloc.abxClasses = new DrugClass[] { DrugClass.Ureidopenicillin, DrugClass.Betalactam };
            azloc.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            azloc.mech = ABXMechanism.CellWall;
            antibiotics.Add("AZL", azloc);

            Drug aztreonam = new Drug();
            aztreonam.abbrev = "ATM";
            aztreonam.name = "Aztreonam";
            aztreonam.drug = ABX.Aztreonam;
            aztreonam.abxClasses = new DrugClass[] { DrugClass.Monolactam, DrugClass.Betalactam };
            aztreonam.spectrums = new ABXSpectrum[] { ABXSpectrum.GramNegative, ABXSpectrum.Narrow, ABXSpectrum.Aerobic };
            aztreonam.mech = ABXMechanism.CellWall;
            antibiotics.Add("ATM", aztreonam);

            Drug carb = new Drug();
            carb.abbrev = "CAR";
            carb.name = "Carbenicillin";
            carb.drug = ABX.Carbenicillin;
            carb.abxClasses = new DrugClass[] { DrugClass.Carboxypenicillin, DrugClass.Betalactam };
            carb.spectrums = new ABXSpectrum[] { ABXSpectrum.GramNegative, ABXSpectrum.Narrow, ABXSpectrum.Aerobic };
            carb.mech = ABXMechanism.CellWall;
            antibiotics.Add("CAR", carb);

            Drug cec = new Drug();
            cec.abbrev = "CEC";
            cec.name = "Cefaclor";
            cec.drug = ABX.Cefaclor;
            cec.abxClasses = new DrugClass[] { DrugClass.Cephalosporin, DrugClass.Cephalosporin_2G, DrugClass.Betalactam };
            cec.spectrums = new ABXSpectrum[] { ABXSpectrum.GramPositive, ABXSpectrum.GramNegative, ABXSpectrum.Broad, ABXSpectrum.Aerobic };
            cec.mech = ABXMechanism.CellWall;
            antibiotics.Add("CEC", cec);

            Drug cfr = new Drug();
            cfr.abbrev = "CFR";
            cfr.name = "Cefadroxil";
            cfr.drug = ABX.Cefadroxil;
            cfr.abxClasses = new DrugClass[] { DrugClass.Cephalosporin, DrugClass.Cephalosporin_1G, DrugClass.Betalactam };
            cfr.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            cfr.mech = ABXMechanism.CellWall;
            antibiotics.Add("CFR", cfr);

            Drug fam = new Drug();
            fam.abbrev = "FAM";
            fam.name = "Cefamandole";
            fam.drug = ABX.Cefamandole;
            fam.abxClasses = new DrugClass[] { DrugClass.Cephalosporin, DrugClass.Cephalosporin_2G, DrugClass.Betalactam };
            fam.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            fam.mech = ABXMechanism.CellWall;
            antibiotics.Add("FAM", fam);

            Drug cefazolin = new Drug();
            cefazolin.abbrev = "CFZ";
            cefazolin.name = "Cefazolin";
            cefazolin.drug = ABX.Cefazolin;
            cefazolin.abxClasses = new DrugClass[] { DrugClass.Cephalosporin, DrugClass.Cephalosporin_1G, DrugClass.Betalactam };
            cefazolin.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            cefazolin.mech = ABXMechanism.CellWall;
            antibiotics.Add("CFZ", cefazolin);

            Drug cefoxitin = new Drug();
            cefoxitin.abbrev = "FOX";
            cefoxitin.name = "Cefoxitin";
            cefoxitin.drug = ABX.Cefoxitin;
            cefoxitin.abxClasses = new DrugClass[] { DrugClass.Cephalosporin, DrugClass.Cephalosporin_2G, DrugClass.Betalactam };
            cefoxitin.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            cefoxitin.mech = ABXMechanism.CellWall;
            antibiotics.Add("FOX", cefoxitin);

            Drug ceftriaxone = new Drug();
            ceftriaxone.abbrev = "CRO";
            ceftriaxone.name = "Ceftriaxone";
            ceftriaxone.drug = ABX.Ceftriaxone;
            ceftriaxone.abxClasses = new DrugClass[] { DrugClass.Cephalosporin, DrugClass.Cephalosporin_3G, DrugClass.Betalactam };
            ceftriaxone.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            ceftriaxone.mech = ABXMechanism.CellWall;
            antibiotics.Add("CRO", ceftriaxone);

            Drug clinda = new Drug();
            clinda.abbrev = "CLI";
            clinda.name = "Clindamycin";
            clinda.drug = ABX.Clindamycin;
            clinda.abxClasses = new DrugClass[] { DrugClass.Lincosamide };
            clinda.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.Aerobic, ABXSpectrum.GramPositive, ABXSpectrum.Anaerobic, ABXSpectrum.GramNegative };
            clinda.mech = ABXMechanism.ProteinSynthesis;
            antibiotics.Add("CLI", clinda);

            Drug cipro = new Drug();
            cipro.abbrev = "CIP";
            cipro.name = "Ciprofloxacin";
            cipro.drug = ABX.Ciprofloxacin;
            cipro.abxClasses = new DrugClass[] { DrugClass.Quinolone, DrugClass.Fluoroquinolone };
            cipro.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            cipro.mech = ABXMechanism.DNASynthesis;
            antibiotics.Add("CIP", cipro);

            Drug dapto = new Drug();
            dapto.abbrev = "DAP";
            dapto.name = "Daptomycin";
            dapto.drug = ABX.Daptomycin;
            dapto.abxClasses = new DrugClass[] { DrugClass.Lipopeptide };
            dapto.spectrums = new ABXSpectrum[] { ABXSpectrum.Moderate, ABXSpectrum.GramPositive };
            dapto.mech = ABXMechanism.CellMembrane;
            antibiotics.Add("DAP", dapto);

            Drug dapsone = new Drug();
            dapsone.abbrev = "DDS";
            dapsone.name = "Dapsone";
            dapsone.drug = ABX.Dapsone;
            dapsone.abxClasses = new DrugClass[] { DrugClass.Other };
            dapsone.spectrums = new ABXSpectrum[] { ABXSpectrum.Narrow, ABXSpectrum.Mycobacterial };
            dapsone.mech = ABXMechanism.FolateBlock;
            antibiotics.Add("DDS", dapsone);

            Drug erta = new Drug();
            erta.abbrev = "ETP";
            erta.name = "Ertapenem";
            erta.drug = ABX.Ertapenem;
            erta.abxClasses = new DrugClass[] { DrugClass.Carbapenam, DrugClass.Betalactam };
            erta.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            erta.mech = ABXMechanism.CellWall;
            antibiotics.Add("ETP", erta);

            Drug etham = new Drug();
            etham.abbrev = "EMB";
            etham.name = "Ethambutol";
            etham.drug = ABX.Ethambutol;
            etham.abxClasses = new DrugClass[] { DrugClass.Other };
            etham.spectrums = new ABXSpectrum[] { ABXSpectrum.Mycobacterial };
            etham.mech = ABXMechanism.CellWall;
            antibiotics.Add("EMB", etham);

            Drug leva = new Drug();
            leva.abbrev = "LVX";
            leva.name = "Levafloxacin";
            leva.drug = ABX.Levafloxacin;
            leva.abxClasses = new DrugClass[] { DrugClass.Quinolone, DrugClass.Fluoroquinolone };
            leva.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            leva.mech = ABXMechanism.DNASynthesis;
            antibiotics.Add("LVX", leva);

            Drug line = new Drug();
            line.abbrev = "LZD";
            line.name = "Linezolid";
            line.drug = ABX.Linezolid;
            line.abxClasses = new DrugClass[] { DrugClass.Oxazolidinone };
            line.spectrums = new ABXSpectrum[] { ABXSpectrum.Moderate, ABXSpectrum.GramPositive };
            line.mech = ABXMechanism.ProteinSynthesis;
            antibiotics.Add("LZD", line);

            Drug mero = new Drug();
            mero.abbrev = "MEM";
            mero.name = "Meropenem";
            mero.drug = ABX.Meropenem;
            mero.abxClasses = new DrugClass[] { DrugClass.Carbapenam, DrugClass.Betalactam };
            mero.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            mero.mech = ABXMechanism.CellWall;
            antibiotics.Add("MEM", mero);

            Drug metro = new Drug();
            metro.abbrev = "MTZ";
            metro.name = "Metronidazole";
            metro.drug = ABX.Metronidazole;
            metro.abxClasses = new DrugClass[] { DrugClass.Nitromidazole };
            metro.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.Anaerobic };
            metro.mech = ABXMechanism.DNASynthesis;
            antibiotics.Add("MTZ", metro);

            Drug piper = new Drug();
            piper.abbrev = "PIP";
            piper.name = "Piperacillin";
            piper.drug = ABX.Piperacillin;
            piper.abxClasses = new DrugClass[] { DrugClass.Ureidopenicillin, DrugClass.Betalactam };
            piper.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            piper.mech = ABXMechanism.CellWall;
            antibiotics.Add("PIP", piper);

            Drug penicillin = new Drug();
            penicillin.abbrev = "PEN";
            penicillin.name = "Penicillin";
            penicillin.drug = ABX.Penicillin;
            penicillin.abxClasses = new DrugClass[] { DrugClass.Penicillin, DrugClass.Penicillin_1G, DrugClass.Betalactam };
            penicillin.spectrums = new ABXSpectrum[] { ABXSpectrum.GramPositive, ABXSpectrum.Narrow };
            penicillin.mech = ABXMechanism.CellWall;
            antibiotics.Add("PEN", penicillin);

            Drug penta = new Drug();
            penta.abbrev = "PMD";
            penta.name = "Pentamidine";
            penta.drug = ABX.Pentamidine;
            penta.abxClasses = new DrugClass[] { DrugClass.Other};
            penta.spectrums = new ABXSpectrum[] { ABXSpectrum.Antiprotezoal };
            penta.mech = ABXMechanism.CytochromeInhibitor;
            antibiotics.Add("PMD", penta);

            Drug piptaz = new Drug();
            piptaz.abbrev = "TZP";
            piptaz.name = "Pipercillin - Tazobactam";
            piptaz.drug = ABX.PiperaciliinTazobactam;
            piptaz.abxClasses = new DrugClass[] { DrugClass.Ureidopenicillin, DrugClass.Betalactam, DrugClass.PenicillinCombination };
            piptaz.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            piptaz.mech = ABXMechanism.CellWall;
            antibiotics.Add("TZP", piptaz);

            Drug rifbut = new Drug();
            rifbut.abbrev = "RFB";
            rifbut.name = "Rifabutin";
            rifbut.drug = ABX.Rifabutin;
            rifbut.abxClasses = new DrugClass[] { DrugClass.Rifamycins };
            rifbut.spectrums = new ABXSpectrum[] { ABXSpectrum.Narrow, ABXSpectrum.GramNegative, ABXSpectrum.Mycobacterial };
            rifbut.mech = ABXMechanism.RNASynthesis;
            antibiotics.Add("RFB", rifbut);

            Drug rifax = new Drug();
            rifax.abbrev = "RFX";
            rifax.name = "Rifaximin";
            rifax.drug = ABX.Rifaximin;
            rifax.abxClasses = new DrugClass[] { DrugClass.Rifamycins };
            rifax.spectrums = new ABXSpectrum[] { ABXSpectrum.Narrow, ABXSpectrum.GramNegative, ABXSpectrum.Mycobacterial };
            rifax.mech = ABXMechanism.RNASynthesis;
            antibiotics.Add("RFX", rifax);

            Drug sxt = new Drug();
            sxt.abbrev = "SXT";
            sxt.name = "Trimethoprim / Sulfamethoxazole";
            sxt.drug = ABX.TrimethoprimSulfamethoxazole;
            sxt.abxClasses = new DrugClass[] { DrugClass.Sulfonamide };
            sxt.spectrums = new ABXSpectrum[] { ABXSpectrum.Broad, ABXSpectrum.Aerobic, ABXSpectrum.Anaerobic, ABXSpectrum.GramNegative, ABXSpectrum.GramPositive };
            sxt.mech = ABXMechanism.FolateBlock;
            antibiotics.Add("SXT", sxt);


            Drug vanco = new Drug();
            vanco.abbrev = "VAN";
            vanco.name = "Vancomycin";
            vanco.drug = ABX.Vancomycin;
            vanco.abxClasses = new DrugClass[] { DrugClass.Glycopeptide };
            vanco.spectrums = new ABXSpectrum[] { ABXSpectrum.GramPositive, ABXSpectrum.Moderate };
            vanco.mech = ABXMechanism.CellWall;
            antibiotics.Add("VAN", vanco);

           

            Drug ampB = new Drug();
            ampB.abbrev = "AMB";
            ampB.name = "Amphotericin B";
            ampB.drug = ABX.AmphotericinB;
            ampB.abxClasses = new DrugClass[] {DrugClass.Antifungal, DrugClass.Polyene };
            ampB.spectrums = new ABXSpectrum[] { ABXSpectrum.Fungal, ABXSpectrum.Broad };
            ampB.mech = ABXMechanism.ErgosterolSynthesis;
            antibiotics.Add("AMB", ampB);

            Drug fluc = new Drug();
            fluc.abbrev = "FLC";
            fluc.name = "Fluconazole";
            fluc.drug = ABX.Fluconazole;
            fluc.abxClasses = new DrugClass[] { DrugClass.Antifungal, DrugClass.Triazole, DrugClass.Azole };
            fluc.spectrums = new ABXSpectrum[] { ABXSpectrum.Fungal, ABXSpectrum.Broad };
            fluc.mech = ABXMechanism.ErgosterolSynthesis;
            antibiotics.Add("FLC", fluc);

            Drug mica = new Drug();
            mica.abbrev = "MFG";
            mica.name = "Micafungin";
            mica.drug = ABX.Micafungin;
            mica.abxClasses = new DrugClass[] { DrugClass.Antifungal, DrugClass.Echinocandin };
            mica.spectrums = new ABXSpectrum[] { ABXSpectrum.Fungal, ABXSpectrum.Narrow };
            mica.mech = ABXMechanism.GlucanSynthesis;
            antibiotics.Add("MFG", mica);

            Drug nyst = new Drug();
            nyst.abbrev = "NYT";
            nyst.name = "Nystatin";
            nyst.drug = ABX.Nystatin;
            nyst.abxClasses = new DrugClass[] { DrugClass.Antifungal, DrugClass.Polyene};
            nyst.spectrums = new ABXSpectrum[] {ABXSpectrum.Fungal, ABXSpectrum.Narrow };
            nyst.mech = ABXMechanism.ErgosterolSynthesis;
            antibiotics.Add("NYT", nyst);

            Drug posa = new Drug();
            posa.abbrev = "PSC";
            posa.name = "Posaconazole";
            posa.drug = ABX.Posaconazole;
            posa.abxClasses = new DrugClass[] { DrugClass.Antifungal, DrugClass.Triazole, DrugClass.Azole };
            posa.spectrums = new ABXSpectrum[] { ABXSpectrum.Fungal, ABXSpectrum.Broad };
            posa.mech = ABXMechanism.ErgosterolSynthesis;
            antibiotics.Add("PSC", posa);

            Drug voric = new Drug();
            voric.abbrev = "VRC";
            voric.name = "Voriconazole";
            voric.drug = ABX.Voriconazole;
            voric.abxClasses = new DrugClass[] { DrugClass.Antifungal, DrugClass.Triazole, DrugClass.Azole };
            voric.spectrums = new ABXSpectrum[] { ABXSpectrum.Fungal, ABXSpectrum.Broad };
            voric.mech = ABXMechanism.ErgosterolSynthesis;
            antibiotics.Add("VRC", voric);

            for (int i = 0; i < antibiotics.Count; i++)
            {
                DrugClass[] mod;
                if (antibiotics.Values.ElementAt(i).abxClasses.Contains(DrugClass.Antifungal))
                {
                    mod = new DrugClass[antibiotics.Values.ElementAt(i).abxClasses.Length + 1];
                    antibiotics.Values.ElementAt(i).abxClasses.CopyTo(mod, 0);
                    mod[antibiotics.Values.ElementAt(i).abxClasses.Length] = DrugClass.AntiInfective;
                }
                else
                {
                    mod = new DrugClass[antibiotics.Values.ElementAt(i).abxClasses.Length + 2];
                    antibiotics.Values.ElementAt(i).abxClasses.CopyTo(mod, 0);
                    mod[antibiotics.Values.ElementAt(i).abxClasses.Length] = DrugClass.AntiInfective;
                    mod[antibiotics.Values.ElementAt(i).abxClasses.Length + 1] = DrugClass.AntiBacterial;
                }
                antibiotics.Values.ElementAt(i).abxClasses = mod;
            }

            Drug none = new Drug();
            none.abbrev = "NONE";
            none.name = "None";
            none.drug = ABX.None;
            none.abxClasses = new DrugClass[] { DrugClass.Unknown };
            none.spectrums = new ABXSpectrum[] { ABXSpectrum.Unknown };
            none.mech = ABXMechanism.Unknown;
            antibiotics.Add("NONE", none);


            Drug unknown = new Drug();
            unknown.abbrev = "UNK";
            unknown.name = "Unknown";
            unknown.drug = ABX.Unknown;
            unknown.abxClasses = new DrugClass[] { DrugClass.Unknown };
            unknown.spectrums = new ABXSpectrum[] { ABXSpectrum.Unknown };
            unknown.mech = ABXMechanism.Unknown;
            antibiotics.Add("UNK", unknown);

            
            
            for(int i =0; i < antibiotics.Count; i++)
            {
                drugsByName.Add(antibiotics.Values.ElementAt(i).name.ToUpper(), antibiotics.Values.ElementAt(i));
            }
        }

        /// <summary>
        /// Returns the antibiotic corresponding to the abbreviation.
        /// 
        /// Calls InitializeAntibioticList() if not
        /// previously initalized.
        /// 
        /// WARNING: LIST IS INCOMPLETE!
        /// MISSING ENTRIES WILL DEFAULT TO UNKNOWN
        /// WITH THE ABX ABBREVIATION.
        /// </summary>
        /// <param name="abbrev">The abbreviation of the drug name, such as PEN, VAN, or CIP</param>
        /// <returns></returns>
        public static Drug GetAntibiotic(string abbrev)
        {
            if(!init)
            {
                InitializeAntibioticList();
                init = true;
            }

            Regex reg = new Regex("[^a-zA-Z0-9 -]");
            abbrev = reg.Replace(abbrev, "").ToUpper();

            Drug ret;

            if (antibiotics.ContainsKey(abbrev))
            {
                ret = antibiotics[abbrev];
            }
            else
            {
                ret = antibiotics["UNK"];
                ret.abbrev = abbrev;
            }
            return ret;
        }

        public static Drug GetDrugByName(string name)
        {
            if (!init)
            {
                InitializeAntibioticList();
                init = true;
            }
            if (drugsByName.ContainsKey(name))
                return drugsByName[name];
            return drugsByName["UNKNOWN"];   
        }

        public static bool IsNullDrug(Drug toTest)
        {
            return (toTest.name == "UNKNOWN" || toTest.name == "NONE" || toTest.abbrev == "UNK" || toTest.abbrev == "NONE");
        }
    }
}
