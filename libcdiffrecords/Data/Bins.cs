﻿using System;
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
        Dictionary<string, List<DataPointAdmission>> admissionsByPatient;
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
                        ages.Add(DataByPatientAdmissionTable[key][0].points[0].Age);
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
                    foreach (DataPointAdmission dpa in DataByPatientAdmissionTable[key])
                    {
                        dps.AddRange(dpa.points);
                    }
                }
                return dps;
            }
        }

        public Dictionary<string, List<DataPoint>> DataByPatient
        {
            get { return patients; }
        }

        public Dictionary<string, List<DataPointAdmission>> DataByPatientAdmissionTable
        {
            get { return admissionsByPatient; }
            set { admissionsByPatient = value; }

        }

        public List<DataPointAdmission> PatientAdmissions
        {
            get
            {
                List<DataPointAdmission> dpa = new List<DataPointAdmission>();
                foreach (string key in admissionsByPatient.Keys)
                {
                    foreach (DataPointAdmission dp in admissionsByPatient[key])
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
                        if (DataByPatientAdmissionTable[key][0].points[0].PatientSex == Sex.Female)
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
            admissionsByPatient = new Dictionary<string, List<DataPointAdmission>>();
            label = binLabel;
        }

        public Bin(string binLabel, DataPoint[] initalPoints)
        {
            data = new List<DataPoint>();
            patients = new Dictionary<string, List<DataPoint>>();
            admissionsByPatient = new Dictionary<string, List<DataPointAdmission>>();
            label = binLabel;

            for (int i = 0; i < initalPoints.Length; i++)
            {
                Add(initalPoints[i]);
            }
        }

        public void Add(DataPoint point)
        {
            data.Add(point);

            if (!patients.ContainsKey(point.MRN))
            {
                patients.Add(point.MRN, new List<DataPoint>());
            }
            patients[point.MRN].Add(point);

            if (!admissionsByPatient.ContainsKey(point.MRN))
            {
                admissionsByPatient.Add(point.MRN, new List<DataPointAdmission>());

                DataPointAdmission dpa = new DataPointAdmission();
                dpa.admissionDate = point.AdmissionDate;
                dpa.MRN = point.MRN;
                dpa.unit = point.Unit.Trim();
                dpa.points.Add(point);

                admissionsByPatient[point.MRN].Add(dpa);

            }
            else
            {

                bool found = false;
                for (int i = 0; i < admissionsByPatient[point.MRN].Count; i++)
                {
                    if (admissionsByPatient[point.MRN][i].admissionDate == point.AdmissionDate && admissionsByPatient[point.MRN][i].unit == point.Unit)
                    {
                        found = true;
                        admissionsByPatient[point.MRN][i].points.Add(point);
                    }

                }
                if (!found)
                {
                    DataPointAdmission dpa = new DataPointAdmission();
                    dpa.admissionDate = point.AdmissionDate;
                    dpa.MRN = point.MRN;
                    dpa.unit = point.Unit.Trim();
                    dpa.points.Add(point);
                    admissionsByPatient[point.MRN].Add(dpa);
                }
            }

        }

        public void Add(DataPointAdmission dpa)
        {
            for (int i = 0; i < dpa.points.Count; i++)
            {
                Add(dpa.points[i]);
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
                admissionsByPatient[key].Sort((x, y) => x.admissionDate.CompareTo(y.admissionDate));

                for (int i = 0; i < admissionsByPatient[key].Count; i++)
                {
                    admissionsByPatient[key][i].SortData();
                }

            }
        }

        public bool TryGetDataPointAdmission(DataPoint dp, out DataPointAdmission result)
        {
            if (admissionsByPatient.ContainsKey(dp.MRN))
            {
                foreach (DataPointAdmission dpa in admissionsByPatient[dp.MRN])
                {
                    if (dpa.admissionDate == dp.AdmissionDate && dpa.unit == dp.Unit)
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
                foreach (DataPointAdmission dpa in DataByPatientAdmissionTable[key])
                {
                    if (dpa.points.Count > 1)
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
                foreach (DataPointAdmission dpa in DataByPatientAdmissionTable[key])
                {
                    string toAssign = "ADM_" + admCt.ToString().PadLeft(6, '0');
                    if (AdmissionIDAssigned(dpa, out int loc))
                    {
                        toAssign = dpa.points[loc].AdmissionID;
                    }
                    else { admCt++; }
                    for (int i = 0; i < dpa.points.Count; i++)
                    {
                        DataPoint temp = dpa.points[i];
                        temp.AdmissionID = toAssign;
                        dpa.points[i] = temp;
                    }

                }
            }
        }
        private bool AdmissionIDAssigned(DataPointAdmission dpa, out int location)
        {
            for (int i = 0; i < dpa.points.Count; i++)
            {
                if (dpa.points[i].AdmissionID != "")
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
                foreach (DataPointAdmission dpa in DataByPatientAdmissionTable[key])
                {
                    if (AdmissionIDAssigned(dpa, out int loc) && dpa.points[loc].AdmissionID != null)
                    {
                        int temp = int.Parse(dpa.points[loc].AdmissionID.Substring(4));
                        if (temp > max)
                            max = temp;
                    }
                }
            }

            return max;
        }



    }
    }
