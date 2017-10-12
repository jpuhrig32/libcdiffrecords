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
        Dictionary<string, List<DataPoint>> patients;
        Dictionary<string, List<Admission>> admissionsByPatient;
        public Dictionary<string, DataPoint> DataBySampleID { get; set; }
        

        public static Bin operator +(Bin b1, Bin b2)
        {
            Bin ret = b1.Clone();
            for(int i = 0; i < b2.Data.Count; i++)
            {
                ret.Add(b2.Data[i]);
            }
            return ret;
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
            }
        }

        public int MedianAge
        {
            get
            {
                List<int> ages = PatientAges;
                if (ages.Count < 1)
                    return 0;
                ages.Sort();

                if (ages.Count % 2 == 0)
                {
                    int midpt = (int)Math.Floor((double)ages.Count / 2);
                    return (ages[midpt] + ages[midpt - 1]) / 2;
                }
                else
                {
                    return ages[ages.Count / 2];
                }
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
                        ages.Add(DataByPatientAdmissionTable[key][0].Points[0].Age);
                    }
                }

                return ages;
            }
        }

        public List<DataPoint> Data
        {
            get
            {
                List<DataPoint> dps = new List<DataPoint>();
                foreach (string key in DataByPatientAdmissionTable.Keys)
                {
                    foreach (Admission dpa in DataByPatientAdmissionTable[key])
                    {
                        dps.AddRange(dpa.Points);
                    }
                }
                return dps;
            }
        }

        public Dictionary<string, List<DataPoint>> DataByPatient
        {
            get { return patients; }
        }

        public Dictionary<string, List<Admission>> DataByPatientAdmissionTable
        {
            get { return admissionsByPatient; }
            set { admissionsByPatient = value; }

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

        /// <summary>
        /// Produces a full copy of this bin, with a new reference attached to it,
        /// Essentially a memberwise clone
        /// </summary>
        /// <returns></returns>
        public Bin Clone()
        {
            Bin retBin = new Bin("Copy of" + label);
            for(int i = 0; i < Data.Count; i++)
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
           if(DataBySampleID.ContainsKey(point.SampleID)) //New sample shares an existing sample dataset. Duplicate or different samples sharing the same ID?
            {
                DataPoint tempSam = DataBySampleID[point.SampleID];
                if(point != tempSam) //They are in fact, different points.
                {
                    string highestSampleID = DataBySampleID.Keys.Max();
                    int sampleMax = int.Parse(highestSampleID.Substring(4));
                    sampleMax++;
                    point.SampleID = "SAM_" + sampleMax.ToString();
                    point.Flags.Add(DataFlag.DifferentSamplesAttachedToSampleID);
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



    }
    }
