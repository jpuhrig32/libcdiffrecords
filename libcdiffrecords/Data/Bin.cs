using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace libcdiffrecords.Data
{
    public class Bin
    {
        string label;
        List<DataPoint> data;
        bool dataPropertyInvalid = true;
        Dictionary<string, List<DataPoint>> patients;
        Dictionary<string, List<Admission>> admissionsByPatient;
        public Dictionary<string, DataPoint> DataBySampleID { get; set; }


        public static Bin operator +(Bin b1, Bin b2)
        {
            return b1.Union(b2);
        }

        public static Bin operator -(Bin b1, Bin b2)
        {
            return b1.Exclude(b2);
        }


        public List<string> PatientMRNList
        {
            get
            {
                List<string> mrns = new List<string>();
                foreach(string key in DataByPatientAdmissionTable.Keys)
                {
                    mrns.Add(key);
                }
                return mrns;
            }
        }

        public int ItemsInBin
        {
            get { return data.Count; }
        }

        public int PatientCount
        {
            get { return patients.Count; }
        }

        public string Label
        {
            get { return label; }
            set { label = value; }
        }
        public int PatientAdmissionCount
        {
            get
            {
                int ct = 0;
                foreach (string key in admissionsByPatient.Keys)
                {
                    ct += admissionsByPatient[key].Count;
                }
                return ct;

            }
        }

        public string AgeRange
        {
            get
            {
                List<int> ages = PatientAges;
                return Utilities.Range(ages);
                /*
                ages.Sort();
                if (ages.Count < 1)
                    return "";

                int min = ages[0];
                int max = ages[0];

                for (int i = 0; i < ages.Count; i++)
                {
                    min = Math.Min(min, ages[i]);
                    max = Math.Max(max, ages[i]);
                }

                if (min == max)
                    return min.ToString();

                return min.ToString() + " - " + max.ToString();
                */
            }
        }

        public int MedianAge
        {
            get
            {
                List<int> ages = PatientAges;
                return Utilities.Median(ages);
            }
        }

        private List<int> SampleCountPerPatient
        {
            get
            {
                List<int> counts = new List<int>();
                foreach (string key in DataByPatient.Keys)
                {
                    counts.Add(DataByPatient[key].Count);
                }

                return counts;
            }
        }

        public double MeanSampleCountPerPatient
        {
            get
            {
                List<int> samcounts = SampleCountPerPatient;
                return Utilities.Mean(samcounts);
            }
        }

        public int MedianSampleCountPerPatient
        {
            get
            {
                List<int> samcounts = SampleCountPerPatient;
                return Utilities.Median(samcounts);
            }
        }

        public string SampleCountRange
        {
            get
            {
                List<int> samcounts = SampleCountPerPatient;
                return Utilities.Range(samcounts);
            }
        }

        public List<int> PatientAges
        {
            get
            {
                List<int> ages = new List<int>();
                foreach (string key in DataByPatientAdmissionTable.Keys)
                {
                    if (DataByPatientAdmissionTable[key].Count > 0)
                    {
                        if (DataByPatientAdmissionTable[key][0].Points[0].Age < 116 && DataByPatientAdmissionTable[key][0].Points[0].Age > 0)
                        {
                            ages.Add(DataByPatientAdmissionTable[key][0].Points[0].Age);
                        }
                    }
                }

                return ages;
            }
        }

        public List<DataPoint> Data
        {
            get
            {
                if(dataPropertyInvalid || data == null)
                {
                    data = new List<DataPoint>();
                    foreach(string key in DataByPatientAdmissionTable.Keys)
                    {
                        foreach(Admission adm in DataByPatientAdmissionTable[key])
                        {
                            data.AddRange(adm.Points);
                        }
                    }
                    dataPropertyInvalid = false;

                }

                return data;
            }
        }

        public Dictionary<string, List<DataPoint>> DataByPatient
        {
            get { return patients; }
        }

        public Dictionary<string, List<Admission>> DataByPatientAdmissionTable
        {
            get { return admissionsByPatient; }
            set
            {
                admissionsByPatient = value;
                dataPropertyInvalid = true;
            }

        }


        public List<Admission> PatientAdmissions
        {
            get
            {
                List<Admission> dpa = new List<Admission>();
                foreach (string key in admissionsByPatient.Keys)
                {
                    foreach (Admission dp in admissionsByPatient[key])
                        dpa.Add(dp);
                }
                return dpa;
            }
        }

        public double PercentFemale
        {
            get
            {
                double fCount = 0;

                foreach (string key in DataByPatientAdmissionTable.Keys)
                {
                    if (DataByPatientAdmissionTable[key].Count > 0)
                    {
                        if (DataByPatientAdmissionTable[key][0].Points[0].PatientSex == Sex.Female)
                            fCount++;
                    }
                }

                if (fCount > 0)
                    return fCount / PatientCount * 100.0;
                return 0;
            }
        }

        public int FemaleCount
        {
            get
            {
                int fCount = 0;

                foreach (string key in DataByPatientAdmissionTable.Keys)
                {
                    if (DataByPatientAdmissionTable[key].Count > 0)
                    {
                        if (DataByPatientAdmissionTable[key][0].Points[0].PatientSex == Sex.Female)
                            fCount++;
                    }
                }

                return fCount;
            }
        }
        /// <summary>
        /// Produces a full copy of this bin, with a new reference attached to it,
        /// Essentially a memberwise clone
        /// </summary>
        /// <returns></returns>
        public Bin Clone()
        {
            Bin retBin = new Bin("Copy of" + label);
            for (int i = 0; i < Data.Count; i++)
            {
                DataPoint temp = new DataPoint();
                temp = Data[i];
                retBin.Add(temp);
            }

            return retBin;
        }



        public Bin(string binLabel)
        {
      
            data = new List<DataPoint>();
            patients = new Dictionary<string, List<DataPoint>>();
            admissionsByPatient = new Dictionary<string, List<Admission>>();
            DataBySampleID = new Dictionary<string, DataPoint>();
            label = binLabel;

        }

        public Bin(string binLabel, DataPoint[] initalPoints)
        {
    
            data = new List<DataPoint>();
            patients = new Dictionary<string, List<DataPoint>>();
            admissionsByPatient = new Dictionary<string, List<Admission>>();
            label = binLabel;
            DataBySampleID = new Dictionary<string, DataPoint>();
            for (int i = 0; i < initalPoints.Length; i++)
            {
                Add(initalPoints[i]);
            }
        }

        public void Add(DataPoint point)
        {
            if (DataBySampleID.ContainsKey(point.SampleID)) //New sample shares an existing sample dataset. Duplicate or different samples sharing the same ID?
            {
                DataPoint tempSam = DataBySampleID[point.SampleID];
                if (point != tempSam) //They are in fact, different points.
                {
                    string highestSampleID = DataBySampleID.Keys.Max();
                    int sampleMax = int.Parse(highestSampleID.Substring(4));
                    sampleMax++;
                    point.SampleID = "SAM_" + sampleMax.ToString();
                    point.Flags.Add("Different sample attached to Sample ID");
                }
                else
                {
                    return;
                }
            }

            DataBySampleID.Add(point.SampleID, point);
            data.Add(point);

            if (!patients.ContainsKey(point.MRN))
            {
                patients.Add(point.MRN, new List<DataPoint>());
            }

            patients[point.MRN].Add(point);

            if (!admissionsByPatient.ContainsKey(point.MRN))
            {
                admissionsByPatient.Add(point.MRN, new List<Admission>());

                Admission dpa = new Admission();
                dpa.AdmissionDate = point.AdmissionDate;
                dpa.DischargeDate = point.DischargeDate;
                dpa.MRN = point.MRN;
                dpa.unit = point.Unit.Trim();
                dpa.PatientName = point.PatientName;
                dpa.Points.Add(point);


                admissionsByPatient[point.MRN].Add(dpa);

            }
            else
            {

                bool found = false;
                for (int i = 0; i < admissionsByPatient[point.MRN].Count; i++)
                {
                    if (admissionsByPatient[point.MRN][i].AdmissionDate == point.AdmissionDate && admissionsByPatient[point.MRN][i].unit == point.Unit)
                    {
                        found = true;
                        admissionsByPatient[point.MRN][i].Points.Add(point);
                    }

                }
                if (!found)
                {
                    Admission dpa = new Admission();
                    dpa.AdmissionDate = point.AdmissionDate;
                    dpa.MRN = point.MRN;
                    dpa.unit = point.Unit.Trim();
                    dpa.Points.Add(point);
                    dpa.PatientName = point.PatientName;
                    admissionsByPatient[point.MRN].Add(dpa);
                }
            }
            dataPropertyInvalid = true;
        }


        public void Add(Admission dpa)
        {
            for (int i = 0; i < dpa.Points.Count; i++)
            {
                Add(dpa.Points[i]);
            }
        }

        public void SortBinData()
        {
            data.Sort((x, y) => x.SampleDate.CompareTo(y.SampleDate));
            foreach (string key in patients.Keys)
            {
                patients[key].Sort((x, y) => x.SampleDate.CompareTo(y.SampleDate));
            }

            foreach (string key in admissionsByPatient.Keys)
            {
                admissionsByPatient[key].Sort((x, y) => x.AdmissionDate.CompareTo(y.AdmissionDate));

                for (int i = 0; i < admissionsByPatient[key].Count; i++)
                {
                    admissionsByPatient[key][i].SortData();
                }

            }
        }

        public bool TryGetDataPointAdmission(DataPoint dp, out Admission result)
        {
            if (admissionsByPatient.ContainsKey(dp.MRN))
            {
                foreach (Admission dpa in admissionsByPatient[dp.MRN])
                {
                    if (dpa.AdmissionDate == dp.AdmissionDate && dpa.unit == dp.Unit)
                    {
                        result = dpa;
                        return true;
                    }
                }

            }
            result = null;
            return false;
        }

        public BinSummaryStatistics GenerateBinSummaryStatistics()
        {
            BinSummaryStatistics ats = new BinSummaryStatistics();
            foreach (string key in DataByPatientAdmissionTable.Keys)
            {
                foreach (Admission dpa in DataByPatientAdmissionTable[key])
                {
                    if (dpa.Points.Count > 1)
                        ats.AdmissionsWithTwoOrMoreSamples++;

                    switch (dpa.AdmissionStatus)
                    {
                        case AdmissionStatus.EmptyAdmit:
                            ats.EmptySamples++;
                            break;
                        case AdmissionStatus.NegativeNoAdmissionSample:
                            ats.Negative_NoAdmissionSample++;
                            break;
                        case AdmissionStatus.NegativeOnAdmission_RemainedNegative:
                            ats.Negative_RemainedNegative++;
                            break;
                        case AdmissionStatus.NegativeOnAdmission_TurnedPositive:
                            ats.Negative_TurnedPositive++;
                            break;
                        case AdmissionStatus.PositiveNoAdmitSample:
                            ats.Positive_NoAdmissionSample++;
                            break;
                        case AdmissionStatus.PositiveOnAdmission:
                            ats.PositiveOnAdmission++;
                            break;
                    }
                }

                if (DataByPatientAdmissionTable[key].Count > 1)
                {
                    ats.PatientsWithTwoOrMoreAdmits++;
                }
            }

            return ats;
        }

        public void AssignAdmissionIDsToBin()
        {
            int admCt = FindMaxAdmissionID();
            foreach (string key in DataByPatientAdmissionTable.Keys)
            {
                foreach (Admission dpa in DataByPatientAdmissionTable[key])
                {
                    string toAssign = "ADM_" + admCt.ToString().PadLeft(6, '0');
                    if (AdmissionIDAssigned(dpa, out int loc))
                    {
                        toAssign = dpa.Points[loc].AdmissionID;
                    }
                    else { admCt++; }
                    for (int i = 0; i < dpa.Points.Count; i++)
                    {
                        DataPoint temp = dpa.Points[i];
                        temp.AdmissionID = toAssign;
                        dpa.Points[i] = temp;
                    }

                }
            }
        }
        private bool AdmissionIDAssigned(Admission dpa, out int location)
        {
            for (int i = 0; i < dpa.Points.Count; i++)
            {
                if (dpa.Points[i].AdmissionID != "")
                {
                    location = i;
                    return true;

                }
            }
            location = 0;
            return false;

        }

        private int FindMaxAdmissionID()
        {
            int max = 1;
            foreach (string key in DataByPatientAdmissionTable.Keys)
            {
                foreach (Admission dpa in DataByPatientAdmissionTable[key])
                {
                    if (AdmissionIDAssigned(dpa, out int loc) && dpa.Points[loc].AdmissionID != null)
                    {
                        int temp = int.Parse(dpa.Points[loc].AdmissionID.Substring(4));
                        if (temp > max)
                            max = temp;
                    }
                }
            }

            return max;
        }


        public Bin Union(Bin toAdd)
        {
            Bin retBin = Clone();
            retBin.Label = Label + "_unioned with_" + toAdd.Label;
            for(int i =0; i < toAdd.Data.Count; i++)
            {
                retBin.Add(toAdd.Data[i]);
            }
            return retBin;
        }
        /// <summary>
        /// Returns an intersection of two bins (The values that occur in both bins)
        /// </summary>
        /// <param name="toIntersect">The bin to intersect with</param>
        /// <returns>A bin containing samples common to both bins</returns>
        public Bin Intersect(Bin toIntersect)
        {
            Bin retBin = new Bin(Label + "_intersected_with_" + toIntersect.Label);
            foreach(string key in DataBySampleID.Keys)
            {
                if (toIntersect.DataBySampleID.ContainsKey(key))
                    retBin.Add(DataBySampleID[key]);

            }
            return retBin;
        }

        /// <summary>
        /// Returns a bin excluding values in the second bin. 
        /// 
        /// The idea is basically a way to create a negative of a filter.
        /// Say, we create a bin of only clinical outpatient culture samples.
        /// This creates the opposite - a bin that has everything except those samples.
        /// It's a way of allowing more flexibility when current filters are not quite sufficient
        /// </summary>
        /// <param name="toExclude">A bin of values to exclude from this bin</param>
        /// <returns></returns>
        /// 
        public Bin Exclude(Bin toExclude)
        {
            Bin retBin = new Bin(Label + "_excluding_" + toExclude.Label);

            foreach(string key in DataBySampleID.Keys)
            {
                if (!toExclude.DataBySampleID.ContainsKey(key))
                    retBin.Add(DataBySampleID[key]);
            }

            return retBin;
        }

        /// <summary>
        /// Returns a bin that contains values that are unique to one bin or the other, while 
        /// excluding values common to both. This is the opposite  of an intersection
        /// 
        /// Creates an intersection of the two bins, and then performs an exclusion
        /// of that intersection from the sum of the two bins.
        /// </summary>
        /// <param name="toComplement">A bin of values to complement</param>
        /// <returns></returns>
        public Bin Complement(Bin toComplement)
        {
            Bin intersection = Intersect(toComplement);
            Bin sum = this + toComplement;

            return sum.Exclude(intersection);
        }


    }
    }
