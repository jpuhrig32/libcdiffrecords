using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords;
using libcdiffrecords.Storage;

namespace libcdiffrecords.Data
{
    public enum AdmissionType
    {
        NegativeOnAdmStaysNegative,
        NegativeOnAdm,
        NegativeOnAdmTurnsPositive,
        PositiveOnAdm,
        PositiveNoAdmSample,
        IndeterminateAdmission,
    };

    public class DataFilter
    {
      

        /// <summary>
        /// Stratifies patients into age-based groups, based on first admission
        /// </summary>
        /// <param name="bin">Unsorted data</param>
        /// <param name="yearsPerBin">Size of each year's age group in years</param>
        /// <returns>A list of bins for each age group</returns>
        public static Bin[] StratifyOnAge(Bin bin, int yearsPerBin)
        {
            if(yearsPerBin < 1)
            {
                yearsPerBin = 1;
            }
            List<Bin> ageBins = new List<Bin>();

            foreach (string key in bin.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission dpa in bin.DataByPatientAdmissionTable[key])
                {
                   int age =  dpa.Points[0].Age;
                    int binIndex = age / yearsPerBin;

                    for(int i = ageBins.Count; i <= binIndex; i++)
                    {
                        if (i == 0)
                            ageBins.Add(new Bin("0 -" + yearsPerBin.ToString()));
                        else
                            ageBins.Add(new Bin((i * yearsPerBin).ToString() + " - " + (i * yearsPerBin + yearsPerBin - 1).ToString()));


                    }

                    ageBins[binIndex].Add(dpa);


                }
            }

            List<Bin> retBins = new List<Bin>();
            for(int i = 0; i < ageBins.Count; i++)
            {
                if (ageBins[i].ItemsInBin > 0)
                    retBins.Add(ageBins[i]);
            }
            return retBins.ToArray();
        }

        /// <summary>
        /// Separates all admissions into separate unit bins
        /// </summary>
        /// <param name="bin">Unseparated data</param>
        /// <returns>An array of bins - one for each unit represented in the original data</returns>
        public static Bin[] StratifyOnUnits(Bin bin)
        {
            Dictionary<string, Bin> bins = new Dictionary<string, Bin>();

            foreach (string key in bin.DataByPatientAdmissionTable.Keys)
            {
                foreach (Admission dpa in bin.DataByPatientAdmissionTable[key])
                {
                    if (!bins.ContainsKey(dpa.unit))
                    {
                        bins.Add(dpa.unit, new Bin(dpa.unit));
                    }

                    foreach (DataPoint dp in dpa.Points)
                        bins[dpa.unit].Add(dp);

                }

            }

            List<Bin> binList = new List<Bin>();
            binList.AddRange(bins.Values);

            binList.Sort((x, y) => x.Label.CompareTo(y.Label));


            return binList.ToArray();
        }

        public static Bin RemoveAdmissionsWithOneSample(Bin b)
        {
            Bin retBin = new Bin(b.Label);
            foreach (string key in b.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission dpa in b.DataByPatientAdmissionTable[key])
                {
                    if(dpa.Points.Count > 1)
                    {
                        retBin.Add(dpa);
                    }
                }
            }

            return retBin;
        }

        public static Bin FilterByCDiffResult(Bin b, TestResult tr)
        {
            Bin retBin = new Bin(b.Label + "_Single_cdiff_result");

            for(int i =0; i < b.Data.Count; i++)
            {
                if (b.Data[i].CdiffResult == tr)
                    retBin.Add(b.Data[i]);
            }
            return retBin;
        }

        /// <summary>
        /// Removes all admissions lacking a sample taken within the admission window (usually 3 days)
        /// </summary>
        /// <param name="bin">Unfiltered data</param>
        /// <param name="admWindow">Days from start of admission that a sample must be taken, in days</param>
        /// <returns>A bin containing all patients with samples taken on admission</returns>
        public static Bin RemoveAdmissionsWithNoAdmissionSample(Bin bin, int admWindow)
        {
            Bin retBin = new Bin(bin.Label);

            foreach (string patient in bin.DataByPatientAdmissionTable.Keys)
            {
                foreach (Admission dpa in bin.DataByPatientAdmissionTable[patient])
                {
                    dpa.AdmissionWindow = admWindow;
                    if(dpa.AdmissionStatus != AdmissionStatus.NegativeNoAdmissionSample && dpa.AdmissionStatus != AdmissionStatus.PositiveNoAdmitSample && dpa.AdmissionStatus != AdmissionStatus.NegativeFirstSample_TurnedPositive)
                    {
                        retBin.Add(dpa);
                    }
                }
            }
            return retBin;          
        }

        /// <summary>
        /// Removes admissions in which a patient was positive on admission
        /// </summary>
        /// <param name="b">Unfiltered data</param>
        /// <returns>A bin containing only patients negative on admission, or indeterminate on admission</returns>
        public static Bin RemovePositveOnAdmissionAdmissions(Bin b)
        {
            Bin retBin = new Bin(b.Label);

            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
                if(b.DataByPatientAdmissionTable[key].Count > 0)
                {
                    if(b.DataByPatientAdmissionTable[key][0].AdmissionStatus != AdmissionStatus.PositiveOnAdmission)
                    {
                        for(int i = 0; i < b.DataByPatientAdmissionTable[key].Count; i++)
                        {
                            retBin.Add(b.DataByPatientAdmissionTable[key][i]);
                        }
                    }
                }
            }

            return retBin;
        }

        /// <summary>
        /// For a given patient, we will choose the admission that defines their encounter with the HC system
        /// Bundles all admissions into a summary admission, based on that we choose one of 3 (maybe 4) options:
        /// 1. Patient remains negative through stay - Choose 1st admit.
        /// 2. Patient is positive on first admit - Choose 1st admit.
        /// 3. Patient turns positive at some point during their encounters - Choose that admit.
        /// 4. Patient is indeterminate on admission - Loop through 1-3. 
        /// </summary>
        /// <param name="b">A bin containing patient data</param>
        /// <returns>A bin with one admit per patient, chosen as above</returns>
        public static Bin FilterIndexAdmissions(Bin b)
        {
            Bin retBin = new Bin("Index Admits");

            b.SortBinData();

            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
                Admission index = PickIndexAdmission(b.DataByPatientAdmissionTable[key]);
                if(index != null)
                     retBin.Add(index);
            }

            return retBin;
        }


        private static Admission PickIndexAdmission(List<Admission> dpa)
        {


            if (dpa.Count > 0)
            {
                // Here is where we deal with the samples that were indeterminate on admission
                while(IndeterminateAdmission(dpa[0].AdmissionStatus))
                {
                    if(dpa.Count <= 1)
                    {
                        return null;
                    }
                    dpa.RemoveAt(0);
                }
                AdmissionStatus status = Admission.MergeAdmissions(dpa.ToArray()).AdmissionStatus;

                if (status == AdmissionStatus.PositiveOnAdmission || status == AdmissionStatus.NegativeOnAdmission_RemainedNegative || status == AdmissionStatus.NegativeNoAdmissionSample)
                    return dpa[0];
                if(status == AdmissionStatus.NegativeOnAdmission_TurnedPositive || status == AdmissionStatus.NegativeFirstSample_TurnedPositive || status == AdmissionStatus.PositiveNoAdmitSample)
                {
                    for(int i = 0; i < dpa.Count; i++)
                    {
                        if (dpa[i].AdmissionStatus == AdmissionStatus.PositiveOnAdmission || dpa[i].AdmissionStatus == AdmissionStatus.NegativeOnAdmission_TurnedPositive)
                            return dpa[i];
                    }
                }
            }

            return null;

        }

        private static bool IndeterminateAdmission(AdmissionStatus status)
        {
            return (status == AdmissionStatus.NegativeNoAdmissionSample || status == AdmissionStatus.PositiveNoAdmitSample || status == AdmissionStatus.NegativeFirstSample_TurnedPositive);
        }

       

        /// <summary>
        /// Removes patients with empty DOB fields, or unknown ages
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bin RemovePatientsWithUnknownDOB(Bin b)
        {
            Bin retBin = new Bin(b.Label);
            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
               foreach(Admission dpa in b.DataByPatientAdmissionTable[key])
                {
                    if (dpa.Points[0].DateOfBirth != new DateTime(1901, 1, 1))
                        retBin.Add(dpa);
                }
            }
            return retBin;
        }

        /// <summary>
        /// Removes patients who are positive on their first admission, or first encounter with the HC system in our records
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bin RemovePatientsPositiveOnFirstAdmission(Bin b)
        {
            Bin retBin = new Bin(b.Label);
            b.SortBinData();
            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
                if(b.DataByPatientAdmissionTable[key][0].AdmissionStatus != AdmissionStatus.PositiveOnAdmission)
                {
                    for(int i = 0; i < b.DataByPatientAdmissionTable[key].Count; i++)
                    {
                        retBin.Add(b.DataByPatientAdmissionTable[key][i]);
                    }
                }
            }

            return retBin;
        }


        /// <summary>
        /// Creates a "Super admit" for all patients - All admits are treated as if they were the
        /// first and only admission a patient has with the HC system.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bin BundlePatientAdmissionsIntoOne(Bin b)
        {
            return BundlePatientAdmissionsIntoOne(b, false, 0);
        }

        public static Bin BundlePatientAdmissionsIntoOne(Bin b, bool ignoreIndeterminateStartAdmissions, int admWindow)
        {
            Bin retBin = new Bin(b.Label);
            b.SortBinData();
            if(ignoreIndeterminateStartAdmissions)
            {
                b = DataFilter.RemoveAdmissionsWithNoAdmissionSample(b, admWindow);
            }
            foreach (string key in b.DataByPatientAdmissionTable.Keys)
            {
                if(b.DataByPatientAdmissionTable[key].Count > 0)
                {
                    DateTime adm = b.DataByPatientAdmissionTable[key][0].AdmissionDate;
                    string unit = b.DataByPatientAdmissionTable[key][0].unit;

                    foreach (DataPoint dp in b.DataByPatient[key])
                    {
                        DataPoint temp = new DataPoint();
                        temp = dp;
                        temp.AdmissionDate = adm;
                        temp.Unit = unit;
                        retBin.Add(temp);
                    }
                }
            }
            return retBin;
        }

        /// <summary>
        /// Returns only the patient admissions meeting a specific admission status.
        /// </summary>
        /// <param name="bin"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static Bin FilterByAdmissionType(Bin bin, AdmissionStatus status)
        {
            Bin ret = new Bin(bin.Label);

            foreach (string key in bin.DataByPatientAdmissionTable.Keys)
            {
                foreach (Admission dpa in bin.DataByPatientAdmissionTable[key])
                {
                   
                    switch(status)
                    {
                        case AdmissionStatus.EmptyAdmit:
                            if (dpa.Points.Count == 0)
                                ret.Add(dpa);
                            break;
                        case AdmissionStatus.NegativeOnAdmission:
                            if (dpa.AdmissionStatus == AdmissionStatus.NegativeOnAdmission  || dpa.AdmissionStatus == AdmissionStatus.NegativeOnAdmission_RemainedNegative || dpa.AdmissionStatus == AdmissionStatus.NegativeOnAdmission_TurnedPositive)
                                ret.Add(dpa);
                            break;
                        case AdmissionStatus.PositiveAdmission:
                            if (dpa.AdmissionStatus == AdmissionStatus.PositiveAdmission || dpa.AdmissionStatus == AdmissionStatus.PositiveOnAdmission || dpa.AdmissionStatus == AdmissionStatus.PositiveNoAdmitSample || dpa.AdmissionStatus == AdmissionStatus.NegativeOnAdmission_TurnedPositive)
                                ret.Add(dpa);
                            break;
                        case AdmissionStatus.NegativeNoAdmissionSample:
                            if (dpa.AdmissionStatus == AdmissionStatus.NegativeNoAdmissionSample)
                                ret.Add(dpa);
                            break;
                        case AdmissionStatus.NegativeOnAdmission_RemainedNegative:
                            if (dpa.AdmissionStatus == AdmissionStatus.NegativeOnAdmission_RemainedNegative)
                                ret.Add(dpa);
                            break;
                        case AdmissionStatus.NegativeAdmission:
                            if (dpa.AdmissionStatus == AdmissionStatus.NegativeNoAdmissionSample || dpa.AdmissionStatus == AdmissionStatus.NegativeOnAdmission_RemainedNegative)
                                ret.Add(dpa);
                            break;
                        case AdmissionStatus.NegativeOnAdmission_TurnedPositive:
                            if (dpa.AdmissionStatus == AdmissionStatus.NegativeOnAdmission_TurnedPositive)
                                ret.Add(dpa);
                            break;
                        case AdmissionStatus.PositiveNoAdmitSample:
                            if(dpa.AdmissionStatus == AdmissionStatus.PositiveNoAdmitSample)
                                ret.Add(dpa);
                            break;
                        case AdmissionStatus.PositiveOnAdmission:
                            if(dpa.AdmissionStatus == AdmissionStatus.PositiveOnAdmission)
                                ret.Add(dpa);
                            break;
                        case AdmissionStatus.IndeterminateAdmission:
                            if(dpa.AdmissionStatus == AdmissionStatus.NegativeNoAdmissionSample || dpa.AdmissionStatus == AdmissionStatus.PositiveNoAdmitSample)
                                ret.Add(dpa);
                            break;

                        case AdmissionStatus.NegativeFirstSample_TurnedPositive:
                            if (dpa.AdmissionStatus == AdmissionStatus.NegativeFirstSample_TurnedPositive || dpa.AdmissionStatus == AdmissionStatus.NegativeOnAdmission_TurnedPositive)
                                ret.Add(dpa);
                            break;
                        default:
                            break;
                    }
                    



                }
            }

            return ret;

        }

        /// <summary>
        /// Stratifies on units, then filters by admission type. 
        /// Equivalent to: StratifyOnUnits -> loop{ FilterByAdmissionType} 
        /// Or StratifyOnUnits(FilterByAdmissionType(Bin, AdmissionStatus))
        /// </summary>
        /// <param name="data">A list of DataPoints loaded from a file</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Bin[] StratifyOnUnitsAndAdmissionType(DataPoint[] data, AdmissionStatus type)
        {
            Bin dataBin = new Bin("Data", data);
            return StratifyOnUnitsAndAdmissionType(dataBin, type);
        }

        /// <summary>
        /// Stratifies on units, then filters by admission type. 
        /// Equivalent to: StratifyOnUnits -> loop{ FilterByAdmissionType} 
        /// Or StratifyOnUnits(FilterByAdmissionType(Bin, AdmissionStatus))
        /// </summary>
        /// <param name="data">A list of DataPoints loaded from a file</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Bin[] StratifyOnUnitsAndAdmissionType(Bin b, AdmissionStatus type)
        {
            return StratifyOnUnits(FilterByAdmissionType(b, type));
        }

        /// <summary>
        /// Returns only the first admissions of a patient
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static Bin FilterFirstAdmissions(Bin bin)
        {
            bin.SortBinData();
            Bin retBin = new Bin(bin.Label);
            foreach(string pat in bin.DataByPatientAdmissionTable.Keys)
            {
                if (bin.DataByPatientAdmissionTable[pat].Count > 0)
                    retBin.Add(bin.DataByPatientAdmissionTable[pat][0]);
            }

            return retBin;
        }

        /// <summary>
        /// Returns only the second or subsequent admissions of a patient.
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static Bin FilterSubsequentAdmissions(Bin bin)
        {
            bin.SortBinData();
            Bin retBin = new Bin(bin.Label);
            foreach (string pat in bin.DataByPatientAdmissionTable.Keys)
            {
                for (int i = 1; i < bin.DataByPatientAdmissionTable[pat].Count; i++)
                    retBin.Add(bin.DataByPatientAdmissionTable[pat][i]);
            }

            return retBin;

        }

     

        /// <summary>
        /// Removes samples marked as "Not Saved" according to the spreadsheet
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static Bin RemoveUnsavedSamples(Bin bin)
        {
            Bin ret = new Bin(bin.Label);

            for (int i = 0; i < bin.Data.Count; i++)
            {
                if(bin.Data[i].Notes == null || !bin.Data[i].Notes.Contains("Not Saved"))
                {
                    ret.Add(bin.Data[i]);
                }
            }

            return ret;
        }

        /// <summary>
        /// Returns only patients positive on their first admission 
        /// Supplies all of their admissions.
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static Bin FilterPatientsPositiveOnFirstAdmission(Bin bin)
        {
            Bin retBin = new Bin(bin.Label);
            bin.SortBinData();

            foreach (string key in bin.DataByPatientAdmissionTable.Keys)
            {
                if(bin.DataByPatientAdmissionTable[key].Count > 0)
                {
                    if(bin.DataByPatientAdmissionTable[key][0].AdmissionStatus != AdmissionStatus.PositiveOnAdmission && bin.DataByPatientAdmissionTable[key][0].AdmissionStatus != AdmissionStatus.PositiveNoAdmitSample)
                    {
                        foreach (Admission dpa in bin.DataByPatientAdmissionTable[key])
                            retBin.Add(dpa);
                    }
                }
            }

            return retBin;
        }


        private static int PositiveCount(Bin bin)
        {
            int pc = 0;
            for(int i =0; i < bin.ItemsInBin; i++)
            {
                if (bin.Data[i].CdiffResult == TestResult.Positive)
                    pc++;
            }


            return pc;
        }

        private static int PatientAdmissionCount(Bin bin)
        {
            int pc = 0;

            foreach(string key in bin.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission dpa in bin.DataByPatientAdmissionTable[key])
                {
                    if (dpa.Points.Count > 0)
                        pc++;
                }
            }
            return pc;
        }

        private static int PositiveOnAdmissionCount(Bin bin, int admWindow)
        {
            int ct = 0; 

            foreach(string key in bin.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission dpa in bin.DataByPatientAdmissionTable[key])
                {
                    if (dpa.Points.Count >= 1 && (dpa.Points[0].SampleDate - dpa.AdmissionDate).Days <= admWindow)
                        ct++;
                }
            }

            return ct;

        }

     



        private static string GenerateDatapointID(DataPoint dp)
        {
            return dp.MRN + dp.AdmissionDate.ToShortDateString() + dp.Unit + dp.SampleDate.ToShortDateString();
        }

        private static bool IsPositiveAdmission(Admission dpa)
        {
            for(int i =0; i < dpa.Points.Count; i++)
            {
                if (dpa.Points[i].CdiffResult == TestResult.Positive)
                    return true;
            }

            return false;

        }

        private static DataPoint[] ExtractDataPointsFromBins(Bin[] bins)
        {
            List<DataPoint> dps = new List<DataPoint>();

            foreach(Bin bin in bins)
            {
                foreach(DataPoint dp in bin.Data)
                {
                    dps.Add(dp);
                }
            }
            return dps.ToArray();
        }

        
        public static Bin[] StratifyOnCommonUnits(Bin b)
        {
            Dictionary<string, bool> common = CreateCommonUnitTable();
            Bin[] bins = StratifyOnUnits(b);
            Bin otherUnits = new Bin("Other Units");

            List<Bin> newBins = new List<Bin>();

            for(int i =0; i < bins.Length; i++)
            {
              
                if(common.ContainsKey(bins[i].Label))
                {
  
                    newBins.Add(bins[i]);
                }
                else
                {
                    otherUnits = MergeBins(otherUnits, bins[i]);
                }
            }
            newBins.Sort((x, y) => x.Label.CompareTo(y.Label));
            newBins.Add(otherUnits);

            return newBins.ToArray();
            
        }

        public static Bin RemoveOutpatientTests(Bin b)
        {
            Bin retBin = new Bin(b.Label);

            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission dpa in b.DataByPatientAdmissionTable[key])
                {
                    for(int i =0; i < dpa.Points.Count; i++)
                    {
                        if (dpa.Points[i].Test != TestType.Clinical_Outpatient_Culture && dpa.Points[i].Test != TestType.Clinical_Outpatient_NAAT)
                            retBin.Add(dpa.Points[i]);
                    }
                }
            }

            return retBin;
        }

        public static Bin FilterByTestType(Bin b, TestType[] test)
        {   

            Bin retBin = new Bin(b.Label);


            foreach (string key in b.DataByPatientAdmissionTable.Keys)
            {
                foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                {
                    for (int i = 0; i < dpa.Points.Count; i++)
                    {
                        for(int j = 0; j < test.Length; j++)
                        {
                            if(dpa.Points[i].Test == test[j])
                            {
                                retBin.Add(dpa.Points[i]);
                                break;
                            }
                        }
                    }
                }
            }

            return retBin;
        }

        /// <summary>
        /// Filters for admissions with samples in a given date range.
        /// Used to construct weekly reports
        /// </summary>
        /// <param name="b">Starting data bin</param>
        /// <param name="start">Start date - inclusive</param>
        /// <param name="end">End date - inclusive</param>
        /// <returns>Admissions with samples falling into the given date range</returns>
        public static Bin FilterAdmissionsWithSamplesInDateRange(Bin b, DateTime start, DateTime end)
        {
            Bin retBin = new Bin(b.Label);

            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission dpa in b.DataByPatientAdmissionTable[key])
                {
                    for(int i = 0; i < dpa.Points.Count; i++)
                    {
                        if(dpa.Points[i].SampleDate >= start && dpa.Points[i].SampleDate <= end)
                        {
                            retBin.Add(dpa);
                            break;
                        }
                    }
                }
            }

            return retBin;

        }

        public static Bin FilterAdmissionsByNumberOfSamples(Bin b, int minSamples)
        {
            Bin retBin = new Bin(b.Label);

            foreach (string key in b.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission adm in b.DataByPatientAdmissionTable[key])
                {
                    if(adm.Points.Count >= minSamples)
                    {
                        retBin.Add(adm);
                    }
                }
            }

            return retBin;

        }

        /// <summary>
        /// Equivalent to FilterForSamplesOccuringAfterGivenSet(criteria, filterFrom, -1);
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="filterFrom"></param>
        /// <returns></returns>
        public static Bin FilterForSamplesOccuringAfterGivenSet(Bin criteria, Bin filterFrom)
        {
            return FilterForSamplesOccuringAfterGivenSet(criteria, filterFrom, -1);
        }

        /// <summary>
        /// Finds and returns all samples from each patient that occur after a given sample from that same patient.
        /// 
        /// The idea here, is that we create a Bin called criteria, and ideally, this should be a Bin
        /// with one sample per patient (but can be more). We take that Bin, and look for all samples occuring
        /// after it, in our second, larger Bin (likely the original Bin of samples) for that same patient.
        /// 
        /// Usage - Situations such as - Grabbing all of the samples that happened after a clinical outpatient test.
        /// Alternatively - grabbing all of the clinical inpatient NAATs that happened after a surveillance test.
        /// (by setting the Bin filterFrom to a list of clinical inpatient NAATs)
        /// </summary>
        /// <param name="criteria">A list of patients with samples (with dates to filter by)</param>
        /// <param name="filterFrom">A list of patients and samples to pick from</param>
        /// <param name="maxDaysAway">Furthest away a patient sample can be from the criteria to be considered a match
        /// Use -1 for no limit</param>
        /// <returns></returns>
        public static Bin FilterForSamplesOccuringAfterGivenSet(Bin criteria, Bin filterFrom, int maxDaysAway)
        {
            Bin retBin = new Bin(criteria.Label + "_samples_after");
            foreach (string key in criteria.DataByPatient.Keys)
            {
                for (int i = 0; i < criteria.DataByPatient[key].Count; i++)
                {
                    DateTime after = criteria.DataByPatient[key][i].SampleDate;

                    if (filterFrom.DataByPatient.ContainsKey(key))
                    {
                        for (int k = 0; k < filterFrom.DataByPatient[key].Count; k++)
                        {
                            if (filterFrom.DataByPatient[key][k].SampleDate > after)
                            {
                                if (maxDaysAway != -1)
                                {
                                    DateTime max = after.AddDays(maxDaysAway);
                                    if(filterFrom.DataByPatient[key][k].SampleDate <= max)
                                        retBin.Add(filterFrom.DataByPatient[key][k]);
                                }
                                else
                                    retBin.Add(filterFrom.DataByPatient[key][k]);
                            }
                           
                                
                        }
                    }
                }
            }
            return retBin;
        }

        /// <summary>
        /// Finds and returns all samples from each patient that occur after a given sample from that same patient.
        /// Equivalent to FilterForsamplesOccuringAfterGivenSet(criteria, filterFrom, -1);
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="filterFrom"></param>
        /// <returns></returns>
        public static Bin FilterForSamplesOccuringAfterGivenSet(Dictionary<string, DateTime> criteria, Bin filterFrom)
        {
            return FilterForSamplesOccuringAfterGivenSet(criteria, filterFrom, -1);
        }

        /// <summary>
        /// Finds and returns all samples from each patient that occur after a given sample from that same patient.
        /// 
        /// The idea here, is that we create a dictionary called criteria, of mrns and date times, 
        /// We take that list, and look for all samples occuring
        /// after it, in our second, larger Bin (likely the original Bin of samples) for that same patient.
        /// 
        /// Usage - Situations such as - Grabbing all of the samples that happened after a clinical outpatient test.
        /// Alternatively - grabbing all of the clinical inpatient NAATs that happened after a surveillance test.
        /// (by setting the Bin filterFrom to a list of clinical inpatient NAATs)
        /// </summary>
        /// <param name="criteria">A Dictionary of patient MRN values and DateTimes to find samples after</param>
        /// <param name="filterFrom">A Bin to find samples after a given date</param>
        /// <param name="maxDaysAway">Maximum days that a given sample can be from the criteria list. Use -1 for no limit</param>
        /// <returns></returns>
        public static Bin FilterForSamplesOccuringAfterGivenSet(Dictionary<string, DateTime> criteria, Bin filterFrom, int maxDaysAway)
        {
            Bin retBin = new Bin(filterFrom.Label + "_Filtered_By_Date_criteria");

            foreach(string key in criteria.Keys)
            {
                if(filterFrom.DataByPatient.ContainsKey(key))
                {
                    for(int i = 0; i < filterFrom.DataByPatient[key].Count; i++)
                    {
                        if(filterFrom.DataByPatient[key][i].SampleDate > criteria[key])
                        {
                            if(maxDaysAway != -1)
                            {
                                DateTime max = criteria[key].AddDays(maxDaysAway);

                                if (filterFrom.DataByPatient[key][i].SampleDate <= max)
                                    retBin.Add(filterFrom.DataByPatient[key][i]);
                            }
                            else
                                retBin.Add(filterFrom.DataByPatient[key][i]);
                        }
                    }
                }
            }

            return retBin;
        }

        /// <summary>
        /// Filters for samples in a given date range
        /// </summary>
        /// <param name="b">The bin to filter from</param>
        /// <param name="start">Earliest sampling date a sample can have (inclusive)</param>
        /// <param name="end">Latest sampling date a sample can have (inclusive)</param>
        /// <returns>A bin of samples in that date range</returns>
        public static Bin FilterForSamplesInDateRange(Bin b, DateTime start, DateTime end)
        {
            Bin retBin = new Bin(b.Label + "_in range: " + start.ToShortDateString() + " - " + end.ToShortDateString());
            for(int i = 0; i < b.Data.Count; i++)
            {
                if (b.Data[i].SampleDate >= start && b.Data[i].SampleDate <= end)
                    retBin.Add(b.Data[i]);
            }

            return retBin;
        }

        public static Bin FilterPatientsWithGivenNumberOfSamples(Bin b, int count)
        {
            Bin retBin = new Bin(b.Label);

            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
                int ct = 0;
                foreach(Admission adm in b.DataByPatientAdmissionTable[key])
                {
                    ct += adm.Points.Count;
                }
                if (ct >= count)
                {
                    foreach (Admission adm in b.DataByPatientAdmissionTable[key])
                    {
                        retBin.Add(adm);
                    }
                }
            }

            return retBin;
        }

        public static Bin FilterPatientsWhoOnlyHaveNegativeSamples(Bin b)
        {
            Bin retBin = new Bin(b.Label);
            foreach(string key in b.DataByPatient.Keys)
            {
                bool onlyNeg = true;
                for(int i = 0; i < b.DataByPatient[key].Count; i++)
                {
                    if (b.DataByPatient[key][i].CdiffResult == TestResult.Positive)
                        onlyNeg = false;
                }
                if(onlyNeg)
                {
                    for (int i = 0; i < b.DataByPatient[key].Count; i++)
                    {
                        retBin.Add(b.DataByPatient[key][i]);
                    }
                }
            }
            return retBin;
        }

        public static Bin RemoveSamplesBasedOnNotes(Bin b, string noteCriteria)
        {
            Bin retBin = new Bin(b.Label);

            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission adm in b.DataByPatientAdmissionTable[key])
                {
                    for(int i = 0; i < adm.Points.Count; i++)
                    {
                        if(!adm.Points[i].Notes.Contains(noteCriteria))
                        {
                            retBin.Add(adm.Points[i]);
                        }
                    }
                }
            }
            return retBin;
        }

        public static Tube[] FilterAllAvailableSampleTubesFromBin(Bin b, StorageData sd)
        {
            List<Tube> tubes = new List<Tube>();

            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
                foreach(Admission adm in b.DataByPatientAdmissionTable[key])
                {
                    for(int i =0; i< adm.Points.Count; i++)
                    {
                        if (sd.TubesBySampleID.ContainsKey(adm.Points[i].SampleID))
                            tubes.AddRange(sd.TubesBySampleID[adm.Points[i].SampleID]);
                    }
                }
            }

            return tubes.ToArray();
        }

        public static Bin FilterAvailableSamples(Bin b, StorageData sd)
        {
            Bin retBin = new Bin(b.Label);

            foreach (string key in b.DataByPatientAdmissionTable.Keys)
            {
                foreach (Admission adm in b.DataByPatientAdmissionTable[key])
                {
                    for (int i = 0; i < adm.Points.Count; i++)
                    {
                        if (sd.TubesBySampleID.ContainsKey(adm.Points[i].SampleID))
                            retBin.Add(adm.Points[i]);
                    }
                }
            }
            return retBin;
        }





      
        public static Bin FilterPositivesByToxinResult(Bin b, TestResult toxResult)
        {
            Bin retBin = new Bin(b.Label);

            for(int i =0; i < b.Data.Count; i++)
            {
                if (b.Data[i].CdiffResult != TestResult.Positive)
                    retBin.Add(b.Data[i]);
                else
                {
                    if (b.Data[i].ToxinResult == toxResult)
                        retBin.Add(b.Data[i]);
                }
            }

            return retBin;
        }
        public static Bin FilterByTestType(Bin b, TestType test)
        {
            return FilterByTestType(b, ExpandTestTypes(test));
        }

        private static TestType[] ExpandTestTypes(TestType[] complex)
        {
            List<TestType> tts = new List<TestType>();

            for(int i =0; i < complex.Length; i++)
            {
                tts.AddRange(ExpandTestTypes(complex[i]));
            }

            return tts.ToArray();

        }

        private static TestType[] ExpandTestTypes(TestType tt)
        {
            TestType[] tests;
            tests = new TestType[1] { tt };
            if (tt == TestType.Surveillance_Test)
            {
                tests = new TestType[4] { TestType.Surveillance_Stool_Culture, TestType.Surveillance_Stool_NAAT, TestType.Surveillance_Swab_Culture, TestType.Surveillance_Swab_NAAT };
            }
            if (tt == TestType.Surveillance_Culture_Test)
            {
                tests = new TestType[2] { TestType.Surveillance_Stool_Culture, TestType.Surveillance_Swab_Culture };
            }
            if (tt == TestType.Surveillance_NAAT_Test)
            {
                tests = new TestType[2] { TestType.Surveillance_Stool_NAAT, TestType.Surveillance_Swab_NAAT };
            }
            if (tt == TestType.Stool)
            {
                tests = new TestType[3] { TestType.Surveillance_Stool_Culture, TestType.Surveillance_Stool_NAAT, TestType.Clinical_Inpatient_NAAT };
            }
            if(tt == TestType.Inpatient_Test)
            {
                tests = new TestType[5] { TestType.Clinical_Inpatient_NAAT, TestType.Surveillance_Stool_Culture, TestType.Surveillance_Stool_NAAT, TestType.Surveillance_Swab_Culture, TestType.Surveillance_Swab_NAAT };
            }
            if(tt == TestType.Surveillance_Stool)
            {
                tests = new TestType[2] { TestType.Surveillance_Stool_Culture, TestType.Surveillance_Stool_NAAT };
            }
            if (tt == TestType.Swab)
            {
                tests = new TestType[2] { TestType.Surveillance_Swab_Culture, TestType.Surveillance_Swab_NAAT };
            }
            return tests;
        }



        public static Bin MergeBins(Bin bin1, Bin bin2)
        {
            for(int i = 0; i < bin2.Data.Count; i++)
            {
                bin1.Add(bin2.Data[i]);
            }

            return bin1;
        }

        public static Bin FilterFlaggedData(Bin b, string[] flags)
        {
            Bin retBin = new Bin(b.Label + "_Flagged");

            for(int i =0; i < b.Data.Count; i++)
            {
                for(int j = 0; j < flags.Length; j++)
                {
                    if (b.Data[i].Flags.Contains(flags[j]))
                    {
                        retBin.Add(b.Data[i]);
                        break;
                    }
                }
            }

            return retBin;
        }

        public static Bin FilterFlaggedData(Bin b, string flag)
        {
            string[] flags = new string[1] { flag };
            return FilterFlaggedData(b, flags);
        }


        public static Bin RemoveFlaggedData(Bin b, string[] flags)
        {
            Bin retBin = new Bin(b.Label + "_FlaggedRemoved");

            for (int i = 0; i < b.Data.Count; i++)
            {
                bool flagged = false;
                for (int j = 0; j < flags.Length; j++)
                {
                    if (b.Data[i].Flags.Contains(flags[j]))
                    {
                        flagged = true;
                        break;
                    }
                }
                if(!flagged)
                {
                    retBin.Add(b.Data[i]);
                }
            }

            return retBin;

        }

        public static Bin RemoveFlaggedData(Bin b, string flag)
        {

            string[] flags = new string[1] { flag };
            return RemoveFlaggedData(b, flags);
        }
        private static Dictionary<string, bool> CreateCommonUnitTable()
        {
            Dictionary<string, bool> common = new Dictionary<string, bool>();

            common.Add("3SW", true);
            common.Add("4SW", true);
            common.Add("7CFAC", true);
            common.Add("7CF", true);
            common.Add("8CFAC", true);
            common.Add("8CF", true);
            common.Add("9NT", true);
            common.Add("CIC", true);
            common.Add("CICU", true);
            common.Add("CVIC", true);
            common.Add("CVICU", true);
            common.Add("NIC", true);
            common.Add("NICU", true);
            common.Add("MIC", true);
            common.Add("MICU", true);
            common.Add("SIC", true);
            common.Add("SICU", true);
            common.Add("TIC", true);
            common.Add("TICU", true);


            return common;
        }

        public static Bin[] StratifyOnPatients(Bin b)
        {
            List<Bin> retBins = new List<Bin>();

            foreach(string key in b.DataByPatientAdmissionTable.Keys)
            {
                Bin temp = new Bin(key);

                foreach(Admission adm in b.DataByPatientAdmissionTable[key])
                {
                    temp.Add(adm);
                }
                retBins.Add(temp);
            }
            return retBins.ToArray();
        }
    }

   
   
}
