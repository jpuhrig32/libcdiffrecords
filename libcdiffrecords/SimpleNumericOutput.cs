using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace libcdiffrecords.Output
{
    public struct SimpleNumericDataStruct
    {
       public int Age;
        public int DaysSinceAdmit;
        public int Sex;
        public int CDResult;

        public SimpleNumericDataStruct(int pAge, int pDaysSinceAdmit, int pSex, int pCdstatus)
        {
            Age = pAge;
            DaysSinceAdmit = pDaysSinceAdmit;
            Sex = pSex;
            CDResult = pCdstatus;
        }
    }
    public class SimpleNumericOutput
    {

        public static void WriteOutput(Patient[] patients, string output)
        {

            SimpleNumericDataStruct[] snds = GetData(patients);
            try
            {
                StreamWriter sw = new StreamWriter(output);
                sw.WriteLine("Patient_Age, Days_Since_Admit, Sex, CDiff_Result");
                for(int i = 0; i < snds.Length; i++)
                {
                    sw.WriteLine(snds[i].Age.ToString() + "," + snds[i].DaysSinceAdmit.ToString() + "," + snds[i].Sex.ToString() + "," + snds[i].CDResult.ToString() + ",");
                }


                sw.Close();
            }
            catch(Exception e)
            {
                throw e;
            }

        }
    
        
        public static SimpleNumericDataStruct[] GetData(Patient[] patients)
        {
            List<SimpleNumericDataStruct> samples = new List<SimpleNumericDataStruct>();

            for (int i = 0; i < patients.Length; i++)
            {
                for (int k = 0; k < patients[i].Admissions.Length; k++)
                {
                    for (int q = 0; q < patients[i].Admissions[k].PatientSamples.Length; q++)
                    {
                        SimpleNumericDataStruct snds = new SimpleNumericDataStruct((int)((patients[i].Admissions[k].PatientSamples[q].SampleDate - patients[i].DOB).Days / 365.25),
                                                                                   (patients[i].Admissions[k].PatientSamples[q].SampleDate - patients[i].Admissions[k].AdmissionDate).Days,
                                                                                   (int)patients[i].Gender,
                                                                                   (int)patients[i].Admissions[k].PatientSamples[q].CDResult);
                        samples.Add(snds);
                    }
                }
            }

            return samples.ToArray();
        }    
    }

    
}
