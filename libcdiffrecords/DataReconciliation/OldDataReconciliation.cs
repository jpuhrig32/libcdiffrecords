using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace libcdiffrecords.DataReconciliation
{
    public class OldReconciliationStructure
    {
        private string mrn;
        private string id;
        private int age;
        private Sex gender;
        private List<DateTime> admissions;
        private DateTime bmtDate;

        /// <summary>
        /// Gets or sets the patient MRN
        /// MRNs are padded automatically on
        /// the left to 8 digits with 0s.
        /// </summary>
        public string MRN
        {
            get { return mrn; }
            set { mrn = value.PadLeft(8, '0'); }

        }

        /// <summary>
        /// Gets or sets the patient ID.
        /// This is not the Medical Record Number (MRN)
        /// </summary>
        public string PID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Gets or sets the patient age
        /// </summary>
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        /// <summary>
        /// Gets or sets the patient sex/gender
        /// </summary>
        public Sex Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        /// <summary>
        /// Gets or sets the date of Bone Marrow Transplant
        /// CAUTION: Returns DateTime.Max if not set
        /// Treat this value as "NA"
        /// </summary>
        public DateTime BMTDate
        {
            get { return bmtDate; }
            set { bmtDate = value; }
        }

        /// <summary>
        /// Gets or sets the list of admission dates
        /// </summary>
        public List<DateTime> AdmissionDates
        {
            get { return admissions; }
            set { admissions = value; }
        }

        public OldReconciliationStructure()
        {
            mrn = "";
            mrn = mrn.PadLeft(8, '0');
            id = "0000";
            age = 0;
            gender = Sex.Unknown;
            admissions = new List<DateTime>();
            bmtDate = DateTime.MaxValue;
        }

        public void AddAdmission(DateTime adm)
        {
            admissions.Add(adm);
        }

        public void SortAdmissions()
        {
            admissions.Sort();
        }
    }

    public class OldDataReconciliation
    {

        public Patient[] ReconcileData(Patient[] patients, string recFile)
        {
            Dictionary<string, OldReconciliationStructure> recData = LoadReconciliationFile(recFile);
            for(int i = 0; i< patients.Length; i++)
            {
                
                if(recData.ContainsKey(patients[i].MRN))
                {
                    OldReconciliationStructure rec = recData[patients[i].MRN];
                    Sample[] patientSamples = patients[i].PatientSamples;
                    Admission[] adms = new Admission[rec.AdmissionDates.Count];

                    for(int x = 0; x < adms.Length; x++)
                    {
                        adms[x] = new Admission();
                        adms[x].AdmissionDate = rec.AdmissionDates[x];
                        adms[x].Age = rec.Age;
                    }
                    for(int y = 0; y < patientSamples.Length; y++)
                    {
                        for(int x = 0; x < adms.Length; x++)
                        {
                            if(x < adms.Length-1)
                            {
                                if(patientSamples[y].SampleDate >= adms[x].AdmissionDate && patientSamples[y].SampleDate < adms[x+1].AdmissionDate)
                                {
                                    patientSamples[y].AdmitDate = adms[x].AdmissionDate;
                                    adms[x].AddSample(patientSamples[y]);
                                    x = adms.Length;
                                }
                            }
                            else
                            {
                                if (patientSamples[y].SampleDate >= adms[x].AdmissionDate)
                                {
                                    patientSamples[y].AdmitDate = adms[x].AdmissionDate;
                                    adms[x].AddSample(patientSamples[y]);
                                    x = adms.Length;
                                }
                            }
                        }
                    }

                    Patient repPatient = new Patient();
                    repPatient.MRN = patients[i].MRN;
                    if (rec.BMTDate != DateTime.MaxValue)
                        repPatient.SetPatientProperty("BMT_Date", rec.BMTDate.ToShortDateString());
                    else
                        repPatient.SetPatientProperty("BMT_Date", "");
                    repPatient.DOB = rec.AdmissionDates[0].AddYears(-rec.Age);
                    repPatient.Gender = rec.Gender;
                    repPatient.PatientName = patients[i].PatientName;
                    
                    for(int x = 0; x < adms.Length; x++)
                    {
                        repPatient.AddAdmission(adms[x]);
                    }
                    patients[i] = repPatient;
                }
                else
                {
                    if (!patients[i].PatientPropertiesContainsKey("BMT_Date"))
                        patients[i].SetPatientProperty("BMT_Date", "");
                }
            }
            return patients;
        }

        private Dictionary<string,OldReconciliationStructure> LoadReconciliationFile(string recFile)
        {
            Dictionary<string, OldReconciliationStructure> recTable = new Dictionary<string, OldReconciliationStructure>();

            if (File.Exists(recFile))
            {
                StreamReader sr = new StreamReader(recFile);
                char[] tab = new char[] { '\t' };
                int lineCount = 0;
                string line = "";

                while ((line = sr.ReadLine()) != null)
                {
                    if(lineCount > 0)
                    {
                        string[] parts = line.Split(tab);
                        if (parts.Length > 30)
                        {
                            OldReconciliationStructure pat = new OldReconciliationStructure();
                            pat.PID = parts[0].Trim();
                            pat.Age = int.Parse(parts[5].Trim());

                            pat.MRN = parts[3].Trim();
                            pat.Gender = ParseGender(parts[4]);
                            pat.AddAdmission(Utilities.DateFromString(parts[7]));
                            string bmt = parts[18].Trim();
                            if (bmt != "")
                                pat.BMTDate = Utilities.DateFromString(bmt);

                            for (int i = 19; i <= 30; i++)
                            {
                                string admDate = parts[i].Trim();
                                if (admDate != "")
                                    pat.AddAdmission(Utilities.DateFromString(admDate));
                            }
                            pat.SortAdmissions();
                            if (!recTable.ContainsKey(pat.MRN))
                                recTable.Add(pat.MRN, pat);
                            else
                                recTable[pat.MRN] = pat;
                        }
                    }
                    lineCount++;
                }

                sr.Close();
            }



            return recTable; 
        }

        private Sex ParseGender(string toParse)
        {
            toParse = toParse.Trim();
            switch(toParse)
            {
                case "0":
                    return Sex.Female;
                case "1":
                    return Sex.Male;
                default:
                    return Sex.Unknown;
            }
        }


    }
}
