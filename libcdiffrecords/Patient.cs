using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords
{
    public enum Sex
    {
        Female,
        Male,
        Unknown,
    };

    public enum PatientSortMode
    {
        DOB,
        FirstAdmit,
        LastAdmit,
    }


    public class Patient:IComparable
    {
        DateTime dob;
        Sex sex;
        string mrn;
        string name;
        string pid;
        const int mrnLength = 8;
        List<Admission> admits;
        Dictionary<string, Object> patientProperties;
        PatientSortMode sort = PatientSortMode.DOB;
        List<DrugCourse> drugs = new List<DrugCourse>();


        /// <summary>
        /// Gets or Sets the Date of Birth (DOB)
        /// </summary>
        public DateTime DOB
        {
            get { return dob; }
            set { dob = value; }
        }

        /// <summary>
        /// Gets or Sets the sex of the patient
        /// </summary>
        public Sex Gender
        {
            get { return sex; }
            set { sex = value; }
        }

       


        /// <summary>
        /// Gets or sets the patient name
        /// </summary>
        public string PatientName
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Returns all the admissions this patient has had.
        /// </summary>
        public Admission[] Admissions
        { get { return admits.ToArray(); } }

        /// <summary>
        /// Returns all of the study inclusive admissions
        /// </summary>
        public Admission[] ValidAdmissions
        {
            get
            {
                List<Admission> valid = new List<Admission>();
                for (int i = 0; i < admits.Count; i++)
                {
                    if (!admits.ElementAt(i).Exclude)
                        valid.Add(admits.ElementAt(i));
                }
                return valid.ToArray();

            }
        }

        /// <summary>
        /// Returns the ages (in years) of all
        /// valid admissions of this patient
        /// </summary>
        public int[] ValidAdmissionAges
        {
            get
            {
                Admission[] valid = ValidAdmissions;
                int[] ages = new int[valid.Length];
                for(int i =0; i < ages.Length; i++)
                {
                    ages[i] = valid[i].Age;
                }
                return ages;
            }
        }

        /// <summary>
        /// Returns the number of valid admissions.
        /// </summary>
        public int ValidAdmissionCount
        {
            get
            {
                int valid = 0;
                for(int i = 0; i < admits.Count; i++)
                {
                    if (!admits.ElementAt(i).Exclude)
                        valid++;
                }
                return valid;
            }

        }

        /// <summary>
        /// Returns the number of samples from all valid admissions
        /// for this patient.
        /// </summary>
        public int ValidSampleCount
        {
            get
            {
                int valid = 0;
                for(int i = 0; i < admits.Count; i++)
                {
                    if(!admits.ElementAt(i).Exclude)
                    {
                        valid += admits.ElementAt(i).SampleCount;
                    }
                }
                return valid;
            }

        }

        public PatientSortMode SortMode
        {
            get { return sort; }
            set { sort = value; }
        }

        
        //Returns all of the samples of this patient. 
        public Sample[] PatientSamples
        {
            get
            {
                List<Sample> samples = new List<Sample>();

                for (int i = 0; i < admits.Count; i++)
                {
                    samples.AddRange(admits[i].PatientSamples);
                }
                return samples.ToArray();
            }
        }


        public List<DrugCourse> DrugTreatments
        {
            get
            {
                return drugs;
            }
            set
            {
                drugs = value;
            }
        }
        /// <summary>
        /// Gets or sets the EPIC Medical Record Number (MRN)
        /// 
        /// Note - if the MRN is less than 8 characters long,
        /// this will pad it with leading zeroes.
        /// </summary>
        public string MRN
        {
            get { return mrn; }
            set
            {
                mrn = value;
                mrn = mrn.Trim();
                //EPIC MRN values are always 8 digits long
                //However, Excel frequently truncates leading zeroes
                //This re-pads those zeroes.
                if(mrn.Length < mrnLength)
                {
                    mrn.PadLeft(mrnLength, '0');
                }
            }
        }

        public Patient()
        {
            dob = DateTime.MinValue;
            sex = Sex.Unknown;
            mrn = "00000000";
            pid = "Unknown";
            admits = new List<Admission>();
            patientProperties = new Dictionary<string, Object>();
        }

        public void AddAdmission(Admission toAdd)
        {
            if(toAdd.AdmissionDate  != DateTime.MaxValue)
            {
                bool add = true;
                for(int i =0; i < admits.Count; i++)
                {
                    if (admits[i].AdmissionDate == toAdd.AdmissionDate)
                    {
                        admits[i].MergeAdmissions(toAdd);
                        add = false;
                    }
                }
                if(add)
                 admits.Add(toAdd);
            }
            
        }

        public Object GetPatientProperty(string key)
        {
            if (patientProperties.ContainsKey(key))
                return patientProperties[key];

            return null;
        }

        public void SetPatientProperty(string key, Object property)
        {
            if(patientProperties.ContainsKey(key))
            {
                patientProperties[key] = property;
            }
            else
            {
                patientProperties.Add(key, property);
            }
        }

        public KeyValuePair<string, Object>[] PatientProperties
        {
            get { return patientProperties.ToArray(); }
            set {
                patientProperties = new Dictionary<string, object>();
                    for(int i = 0; i < value.Length; i++)
                {
                    patientProperties[value[i].Key] = value[i].Value;
                }
                }
        }


        /// <summary>
        /// Generally used for the October Foramat tracking files
        /// Every line in the file is 1 sample, and as such.
        /// creates a new "Patient" and a new "Admission".
        /// 
        /// This function de-duplicates and combines that data
        /// to create an updated patient record
        /// </summary>
        /// <param name="toMerge">The updates to add to the record</param>
        public void MergePatientRecords(Patient toMerge)
        {
            //We only want to do the merge if it is actually the same patient
            if(toMerge.MRN == MRN)
            {
                //Ok, so this patient exists. Let's check for any updated fields
                MergeFields(toMerge);
                int adminStartLength = admits.Count;
                for (int i = 0; i < toMerge.Admissions.Length; i++)
                {
                    if(!AdmissionIsDuplicate(toMerge.Admissions[i]))
                    {
                        admits.Add(toMerge.Admissions[i]);
                    }
                }
            }

        }
        /// <summary>
        /// The idea here is to check for any default-value fields, and update them with new information from this new patient record
        /// </summary>
        /// <param name="toMerge"></param>
        private void MergeFields(Patient toMerge)
        {
            if((dob == DateTime.MinValue)&&(toMerge.DOB != DateTime.MinValue))
                    dob = toMerge.DOB;
            if ((sex == Sex.Unknown) && (toMerge.Gender != Sex.Unknown))
                sex = toMerge.Gender;
            for(int i = 0; i < toMerge.PatientProperties.Length; i++)
            {
                if (!patientProperties.ContainsKey(toMerge.PatientProperties[i].Key))
                {
                    patientProperties.Add(toMerge.PatientProperties[i].Key, toMerge.PatientProperties[i].Value);
                }
                else
                {
                    if ((patientProperties[toMerge.PatientProperties[i].Key].ToString() == "UNK") && (toMerge.PatientProperties[i].Value.ToString() != "UNK"))
                        {
                             patientProperties[toMerge.PatientProperties[i].Key] = toMerge.PatientProperties[i].Value;
                        }
                }
                   
            }
        }
        private bool AdmissionIsDuplicate(Admission adm)
        {
            for(int i = 0; i < admits.Count; i++)
            {
                if (adm.AdmissionDate == admits[i].AdmissionDate)
                {
                    admits[i].MergeAdmissions(adm);
                    return true;
                }
            }

            return false;
        }

        public void SortAdmissions()
        {
            admits.Sort();
            for (int i = 0; i < admits.Count; i++)
                admits[i].SortSamples();
        }

        public int CompareTo(object obj)
        {
            if(sort == PatientSortMode.DOB)
                return dob.CompareTo(((Patient)obj).DOB);
            if (sort == PatientSortMode.FirstAdmit)
                return dob.CompareTo(((Patient)obj).admits[0].AdmissionDate);
            if(sort == PatientSortMode.LastAdmit)
                return dob.CompareTo(((Patient)obj).admits[((Patient)obj).admits.Count - 1].AdmissionDate);

            return dob.CompareTo(((Patient)obj).DOB);
        }

        public bool PatientPropertiesContainsKey(string key)
        {
            return patientProperties.ContainsKey(key);
        }

        public void SetAdmissions(Admission[] toSet)
        {
            admits = new List<Admission>(toSet.Length);
            for(int i =0; i < toSet.Length; i++)
            {
                admits.Add(toSet[i]);
            }
        }

    }
}
