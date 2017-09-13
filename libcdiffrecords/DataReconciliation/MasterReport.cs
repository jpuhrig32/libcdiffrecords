using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using libcdiffrecords.ErrorHandling;

namespace libcdiffrecords.DataReconciliation
{
    enum MasterReportParseMode
    {
        BasicInfo,
        FacilityDrugs,
        PrescriptionDrugs,
        Done,
        Seeking,
    };

    /// <summary>
    /// Provides methods for extraction of data from EPIC Master Reports - to efficiently pull
    /// desired data from EPIC
    /// </summary>
    public class MasterReport
    {
        /// <summary>
        /// Fields that will be exposed publically
        /// </summary>
        string mrn;
        string patientName;
        DateTime dob;
        Sex sex;
        List<DrugCourse> drugs;

        /// <summary>
        /// Private fields - used by the parsers only
        /// </summary>
        List<string> drugKeywords;
        Dictionary<string, MasterReportParseMode> modeSearcher;
        const int drugStartDateOffset = 59; //The start date always starts at position 59 (in a zero-based array), or the 60th character overall.
        const int drugEndDateOffset = 70;  //The end date is always position 70 (base zero), or 71st overall - 11 characters apart.
        const int drugDataOffset = 4; //Drug data always starts at position 4 (base zero), or position 5 overall. There are always 4 preceeding spaces.
        const int dateWidth = 10; //The maximum a date can be is 10 chars wide, however, unused positions are always spaces, which are easily removed.
        const int drugNameOffset = 4;

        private void InitializeReader()
        {
            drugKeywords = new List<string>();
            modeSearcher = new Dictionary<string, MasterReportParseMode>();
            modeSearcher.Add("Facility - Administered Medications History", MasterReportParseMode.FacilityDrugs);
            drugs = new List<DrugCourse>();

        }

        public void ReadMasterReport(string filename)
        {
            InitializeReader();
            //The master reports seem to have a fairly consistent structuring of character positioning 
            //Which, makes it easy to pick out start / end dates, based on the positions of characters.
            //The easiest thing to do, is to, starting at the character offset, pull 10 characters (2 month, 2 day, 2 slashes, 4 for the year), and parse it. 
            //Notably - start and end are 11 chars apart - allowing for at least one space between the two, however, intervening spaces are added to maintain
            //the same offsets.
            
           

            List<string> facilDrugs = new List<string>();
            List<string> prescripDrugs = new List<string>();

            Regex facilAdmDrugsSectRegex = new Regex("Administered Medications History");
            Regex prescripDrugsSectRegex = new Regex("Prescription History");
            MasterReportParseMode mode = MasterReportParseMode.BasicInfo;

           
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename);
                string line = "";
                int lineCount = 0;

                while((line = sr.ReadLine()) != null)
                {
                    if(lineCount == 57)
                    {
                        int s = 0;
                    }
                    if (lineCount == 0)
                    {
                        try
                        {
                            mrn = line.Substring(0, 8);
                            patientName = line.Substring(10, 39).Trim(); //Given the fixed-position nature of these files, names are allocated 38 chars - starting at position 10, and extending (likely) 38 characters, before a space.
                            dob = Utilities.ParseDate(line.Substring(50, dateWidth).Trim());
                            sex = Utilities.ParseSexFromString(line.Substring(62, 1));
                            mode = MasterReportParseMode.Seeking;
                        }
                        catch
                        {
                            throw new FileNotPatientDataException("File does not appear to be a MasterReport");
                        }
                    }

                   else
                    {
                        
                        if (mode == MasterReportParseMode.Seeking)
                        {
                            if(facilAdmDrugsSectRegex.IsMatch(line))
                            {
                                mode = MasterReportParseMode.FacilityDrugs;
                                continue;
                            }
                            if(prescripDrugsSectRegex.IsMatch(line))
                            {
                                mode = MasterReportParseMode.PrescriptionDrugs;
                                continue;
                            }
                        }
                        if(mode == MasterReportParseMode.FacilityDrugs)
                        {
                            if(!Utilities.IsNullString(line))
                            {
                                facilDrugs.Add(line);
                            }
                            else
                            {
                                mode = MasterReportParseMode.Seeking;
                            }
                        }
                        if(mode == MasterReportParseMode.PrescriptionDrugs)
                        {
                            if(!Utilities.IsNullString(line))
                            {
                                prescripDrugs.Add(line);
                            }
                            else
                            {
                                mode = MasterReportParseMode.Seeking;
                            }
                        }

                        
                        
                    }
                    lineCount++;
                }

                sr.Close();
                ParseDrugs(facilDrugs, MasterReportParseMode.FacilityDrugs);

            }
        }

        private void ParseDrugs(List<string> drugLines, MasterReportParseMode mode)
        {
            Regex route = new Regex("Route:");
            Regex dose = new Regex("Dose:");
            Regex frequency = new Regex("Frequency:");
            Regex sig = new Regex("Sig:");
            Regex disc = new Regex("Disc:");
            Regex generalKeyword = new Regex("[A-Z]([a-z])+:");
            char[] space = new char[] { ' ' };
            Regex dashNumber = new Regex("-");
            Regex removeNonNumbers = new Regex("[^0-9]");
            Regex isNumber = new Regex("[0-9]+");
            Regex morningNoonNight = new Regex("@(MORNING|AFTERNOON|EVENING)");
            

            bool noDrugsYet = true;
            DrugCourse currentDrug = new DrugCourse();
            for(int i = 0; i < drugLines.Count; i++)
            {
                if(route.IsMatch(drugLines[i]))
                {
                    string line = drugLines[i].Trim();
                    string[] parts = line.Split(space);
                    
                    if(parts.Length > 1 && route.IsMatch(parts[0]))
                    {
                        currentDrug.RouteOfAdministration = parts[1].Trim();
                    }
                }
                else if(dose.IsMatch(drugLines[i]))
                {
                    string[] parts = drugLines[i].Trim().Split(space);
                    if(parts.Length > 1 && dose.IsMatch(parts[0]))
                    {
                       currentDrug.Dose= int.Parse(removeNonNumbers.Replace(parts[1], ""));
                        if (parts.Length > 2)
                            currentDrug.DosingUnits = parts[2].Trim();
                    }
                }
                else if(mode == MasterReportParseMode.PrescriptionDrugs && sig.IsMatch(drugLines[i])) //Data from the Significance
                {
                    string[] parts = drugLines[i].Trim().Split(space);
                    if(parts.Length >2)
                    {
                        if(parts[0].ToUpper().Contains("TAKE"))
                        {
                            double test = 0;
                            if(double.TryParse(parts[1].Trim(), out test))
                            {
                                currentDrug.Dose = test;
                                for(int x = 2; x < parts.Length; x++)
                                {
                                    if(parts[x].ToUpper().Contains("BY"))
                                    {
                                        if(x+2 < parts.Length)
                                        {
                                            int test2;
                                            if(int.TryParse(parts[x+2], out test2))
                                            {

                                            }
                                            else
                                            {
                                                parts[x + 2] = parts[x + 2].ToUpper().Trim();
                                                if (x + 4 < parts.Length)
                                                {
                                                    if (parts[x + 2].Contains("EVERY"))
                                                    {
                                                        if(int.TryParse(parts[x+3], out test2))
                                                        {
                                                            currentDrug.FrequencyPerDay = test2 / 24;
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if(frequency.IsMatch(drugLines[i]))
                {
                    string[] parts = drugLines[i].Trim().Split(space);
                    if(parts.Length > 1)
                    {
                        parts[1] = parts[1].Trim().ToUpper();
                        switch(parts[1])
                        {
                            case "ONCE":
                                currentDrug.FrequencyPerDay = 1;
                                continue;
                            case "PRN":
                                currentDrug.PRN = true;
                                continue;
                            case "Q":
                                if(parts.Length > 3)
                                {
                                    int result = 0;
                                    if(int.TryParse(removeNonNumbers.Replace(parts[2], ""), out result))
                                    {

                                        currentDrug.FrequencyPerDay = result / 24;

                                    }
                                    else
                                    {
                                        if (morningNoonNight.IsMatch(parts[2]))
                                            currentDrug.FrequencyPerDay = 1;

                                    }
                                }

                                continue;
                              
                        }
                    }
                }
                else if(generalKeyword.IsMatch(drugLines[i]))
                {

                }
                else if(drugLines[i].Contains("Cosign")) //A cosign line usually doens't follow the specific rules for the rest of the drug section, so we need to specify directly.
                {
                    continue;
                }
                else
                {
                    if(!noDrugsYet)
                        drugs.Add(currentDrug);
                    else
                        noDrugsYet = false;
                    currentDrug = new DrugCourse();
                    currentDrug.TreatmentStart = Utilities.DateFromString(drugLines[i].Substring(drugStartDateOffset, dateWidth));
                    if (currentDrug.TreatmentStart == DateTime.MaxValue)
                        currentDrug.TreatmentStart = DateTime.MinValue;
                    if(drugLines[i].Length >= drugEndDateOffset+dateWidth)
                        currentDrug.TreatmentEnd = Utilities.DateFromString(drugLines[i].Substring(drugEndDateOffset, dateWidth));
                    string drugName = drugLines[i].Substring(drugNameOffset, drugStartDateOffset - drugNameOffset - 1);
                    drugName = drugName.Trim();

                    string[] dnParts = drugName.ToUpper().Split(space);
                    if(dnParts.Length > 0)
                    {
                        currentDrug.DrugClasses = AssignClassesBasedOnName(dnParts[0].Trim());

                    }
                    currentDrug.DrugName = drugName;

                    if(mode == MasterReportParseMode.FacilityDrugs)
                    {
                        for(int x = 0; x < dnParts.Length; x++)
                        {

                        }
                    }
                   

                }
                
            }
        }


        private DrugClass[] AssignClassesBasedOnName(string drugName)
        {
            if(drugName.ToUpper().Contains("PRAZOLE"))
            {
                return new DrugClass[] { DrugClass.ProtonPumpInhibitor };
            }
            Drug d = AntibioticManager.GetDrugByName(drugName);
            if(!AntibioticManager.IsNullDrug(d)) //The Antibiotic manager has actually given us a known drug
            {
                return d.abxClasses;   
            }

            return new DrugClass[] { };
        }
                /*
                public void ReadMasterReport(string filename)
                {
                    //The master reports seem to have a fairly consistent structuring of character positioning 
                    //Which, makes it easy to pick out start / end dates, based on the positions of characters.
                    //The easiest thing to do, is to, starting at the character offset, pull 10 characters (2 month, 2 day, 2 slashes, 4 for the year), and parse it. 
                    //Notably - start and end are 11 chars apart - allowing for at least one space between the two, however, intervening spaces are added to maintain
                    //the same offsets.
                    const int drugStartDateOffset = 59; //The start date always starts at position 59 (in a zero-based array), or the 60th character overall.
                    const int drugEndDateOffset = 70;  //The end date is always position 70 (base zero), or 71st overall - 11 characters apart.
                    const int dateWidth = 10; //The maximum a date can be is 10 chars wide, however, unused positions are always spaces, which are easily removed.
                    const int drugDataOffset = 4; //Drug data always starts at position 4 (base zero), or position 5 overall. There are always 4 preceeding spaces.

                    Regex facilityDrugs = new Regex("Facility - Administered Medications History");
                    MasterReportParseMode mode = MasterReportParseMode.BasicInfo;

                    if (File.Exists(filename))
                    {
                        StreamReader sr = new StreamReader(filename);
                        int lineCount = 0;
                        string line = "";
                        bool drugsFound = false;
                        DrugCourse currentDrug = new DrugCourse();

                        while((line = sr.ReadLine()) != null)
                        {
                            if(lineCount == 0)
                            {
                                try
                                {
                                    mrn = line.Substring(0, 8);
                                    patientName = line.Substring(10, 39).Trim(); //Given the fixed-position nature of these files, names are allocated 38 chars - starting at position 10, and extending (likely) 38 characters, before a space.
                                    dob = Utilities.ParseDate(line.Substring(50, dateWidth).Trim());
                                    sex = Utilities.ParseSexFromString(line.Substring(62, 1));
                                }
                                catch
                                {
                                    throw new FileNotPatientDataException("File does not appear to be a MasterReport");
                                }
                            }

                            if(lineCount > 0 && mode == MasterReportParseMode.Drugs)
                            {

                                if (!drugsFound && facilityDrugs.IsMatch(line))
                                {
                                    drugsFound = true;
                                    continue;
                                }
                                if (drugsFound)
                                {
                                    if(line.Trim() == "")
                                    {
                                        mode = MasterReportParseMode.Done;
                                        drugs.Add(currentDrug);
                                        continue;
                                    }


                                }
                            }

                            if(mode == MasterReportParseMode.Done)
                            {
                                goto Cleanup;
                            }
                        }

                        goto Cleanup;

                        Cleanup:
                            sr.Close();
                    }
                    else
                        throw new FileNotFoundException("Master Report file at " + filename + " not found");
                }
                */


            }
}
