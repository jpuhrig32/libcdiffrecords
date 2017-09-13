using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords
{
    /// <summary>
    /// Every sample in the DB taks place during an admission
    /// 
    /// </summary>
    public class Admission : IComparable
    {
        int patientAge = 0;
        List<Sample> samples = new List<Sample>();
        DateTime admDate = DateTime.MaxValue;
        bool exclude = true;
        List<Cdiff> naatResults = new List<Cdiff>();
        List<DateTime> naatDates = new List<DateTime>();

        public String MRN = "00000000"; 


        /// <summary>
        /// Exclude from analysis due to not meeting basic sample 
        /// criteria:
        /// 1. Have at least 2 samples
        /// 2. First sample must not be positive (not pos on admission)
        /// </summary>
        public bool Exclude
        {
            get
            {
                return !(samples.Count >= 2 && samples[0].CDResult != Cdiff.Positive);
            }
        }

        /// <summary>
        /// Returns the number of days between admission
        /// and the first sampling
        /// </summary>
        public int TimeToFirstSample
        {
            get { return (samples.ElementAt(0).SampleDate - admDate).Days; }
        }

        /// <summary>
        /// Returns the Time To Event - either the first
        /// C. diff positive stool, or the final sample
        /// if the patient remains negative
        /// </summary>
        public int TimeToEvent
        {
            get
            {
                if (!exclude)
                {
                    for (int i = 0; i < samples.Count; i++)
                    {
                        if ((i == samples.Count - 1) || (samples.ElementAt(i).CDResult == Cdiff.Positive))
                        {
                            return (samples.ElementAt(i).SampleDate - admDate).Days;
                        }

                    }
                }
                else
                {
                    if (samples.Count > 0)
                    {
                        return (samples.ElementAt(0).SampleDate - admDate).Days;
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// Returns the number of samples for this admission
        /// </summary>
        public int SampleCount
        { get { return samples.Count; } }

        /// <summary>
        /// Returns the age of the patient at the time of this admission
        /// </summary>
        public int Age
        {
            get { return patientAge; }
            set { patientAge = value; }
        }

        /// <summary>
        /// Indicates whether or not the patient became positive during their stay. Returns false
        /// if the patient has fewer than 2 samples, or if the first sample is negative.
        /// </summary>
        public bool BecamePositiveDuringStay
        {
            get
            {
                if (!Exclude)
                {
                    for(int i = 1; i < samples.Count; i++)
                    {
                        if (samples[i].CDResult == Cdiff.Positive)
                            return true;
                    }
                }
                return false;
            }
        }


        /// <summary>
        /// Date first admitted to FMLH
        /// </summary>
        public DateTime AdmissionDate
        {
            get { return admDate; }
            set { admDate = value; }
        }

        /// <summary>
        /// Gets the array of patient samples
        /// </summary>
        public Sample[] PatientSamples
        {
            get { return samples.ToArray(); }
        }

        public List<Cdiff> NAATResults
        {
            get { return naatResults; }
            set { naatResults = value; }
        }
        public List<DateTime> NAATDates
        {
            get { return naatDates; }
            set { naatDates = value; }
        }


        /// <summary>
        /// Sets the age of the patient using their DOB
        /// </summary>
        /// <param name="dob"></param>
        public void SetAge(DateTime dob)
        {
            if (dob <= admDate) //Can't really have a negative age
            {
                TimeSpan ts = admDate - dob;
                patientAge = (int)Math.Floor(ts.Days / 365.25);
            }
            else
            {
                patientAge = -1;
            }
        }

        /// <summary>
        /// Adds a sample to the list of samples for this admission.
        /// Excludes ones with dates matching existing samples
        /// </summary>
        /// <param name="toAdd"></param>
        public void AddSample(Sample toAdd)
        {
            bool add = true;
            for (int i = 0; i < samples.Count; i++)
            {

                if (samples[i].SampleDate == toAdd.SampleDate)
                {
                    add = false;
                }
            }
            if (add)
                samples.Add(toAdd);
        }

        /// <summary>
        /// Merges this admission with another, preserving the admission date of this instance.
        /// </summary>
        /// <param name="toMerge"></param>
        public void MergeAdmissions(Admission toMerge)
        {
            MergeAdmissions(toMerge, false);
        }

        /// <summary>
        /// Merges two admission objects- useful for reconciliation of admission data with other data sources
        /// </summary>
        /// <param name="toMerge">The new data to merge with this instance</param>
        /// <param name="overwriteAdmDate">Assume that the data to merge is new, and any conflicting data should be overwritten</param>
        public void MergeAdmissions(Admission toMerge, bool overwriteAdmDate)
        {
            if (admDate == toMerge.AdmissionDate)
            {
                for (int i = 0; i < toMerge.PatientSamples.Length; i++)
                {
                    bool addToSamples = true;
                    for (int x = 0; x < samples.Count; x++)
                    {
                        if (samples[x].SampleDate == toMerge.PatientSamples[i].SampleDate)
                        {
                            samples[x].MergeSamples(toMerge.PatientSamples[i]);
                            samples[x].AdmitDate = toMerge.AdmissionDate;
                            addToSamples = false;
                        }
                    }
                    if (addToSamples)
                        samples.Add(toMerge.PatientSamples[i]);
                }

            }
        }

        /// <summary>
        /// Sorts the list of samples, by their sample date. Also sorts the drug treatments in all of the samples as well
        /// </summary>
        public void SortSamples()
        {
            samples.Sort();
            for(int i = 0; i < samples.Count; i++)
            {
                samples[i].SortDrugs();
            }
        }



        public int CompareTo(object obj)
        {
            return admDate.CompareTo(((Admission)obj).AdmissionDate);
        }
    }
}
