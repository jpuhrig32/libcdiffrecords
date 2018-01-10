using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using libcdiffrecords.Data;


namespace libcdiffrecords
{
    public class NAATAnalysis
    {
   
        public void CompareSurveillanceDataToNAATs(DataPoint[] surv, DataPoint[] naats, int naatDistance, string aggregateOutput)
        {
            //Building NAAT patient database
            Dictionary<string, List<DataPoint>> naatTable = BuildNAATByPatientLookupTable(naats);

          

            Bin[] negOnAdm = DataFilter.StratifyOnUnitsAndAdmissionType(surv, AdmissionStatus.NegativeOnAdmission);
            Bin[] posOnAdm = DataFilter.StratifyOnUnitsAndAdmissionType(surv, AdmissionStatus.PositiveAdmission);
            Bin[] indAdm = DataFilter.StratifyOnUnitsAndAdmissionType(surv, AdmissionStatus.IndeterminateAdmission);


            Dictionary<string, NAATAnalysisUnitReport> reportsByUnit = new Dictionary<string, NAATAnalysisUnitReport>();

            foreach(Bin b in negOnAdm)
            { 
            if (!reportsByUnit.ContainsKey(b.Label))
                {
                    reportsByUnit.Add(b.Label, new NAATAnalysisUnitReport());

                }
               
                reportsByUnit[b.Label].uniquePatientCount += b.PatientCount;
                reportsByUnit[b.Label].sampleCount += b.ItemsInBin;
                reportsByUnit[b.Label].admissionCount += b.PatientAdmissionCount;
                reportsByUnit[b.Label].NAPA = b.PatientAdmissionCount;
                reportsByUnit[b.Label].NASAM = b.ItemsInBin;
                reportsByUnit[b.Label].NAUP = b.PatientCount;

                foreach (string key in b.DataByPatientAdmissionTable.Keys)
                {
                    
                    if(naatTable.ContainsKey(key)) //We don't care if the patient isn't even in the NAAT table
                    {
                        foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                        {
                            foreach(DataPoint dp in naatTable[key])
                            {
                                if((dpa.AdmissionDate <= dp.SampleDate) && (dp.SampleDate - dpa.AdmissionDate).Days <= naatDistance) //We've found a matching NAAT nearby
                                {
                                    if(dp.CdiffResult == TestResult.Positive)
                                    {
                                        switch (dp.ToxinResult)
                                        {
                                            case TestResult.Positive:
                                                reportsByUnit[b.Label].NANPEP++;
                                                break;
                                            case TestResult.Negative:
                                                reportsByUnit[b.Label].NANPEN++;
                                                break;
                                            default:
                                                reportsByUnit[b.Label].NANPEI++;
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        reportsByUnit[b.Label].NANN++;
                                    }
                                }
                                else
                                {
                                    reportsByUnit[b.Label].NANT++;
                                }
                            }
                        }

                    }
                    else
                    {
                        reportsByUnit[b.Label].NANT += b.DataByPatientAdmissionTable[key].Count;
                    }
                }

            }

            foreach (Bin b in posOnAdm)
            {
                if (!reportsByUnit.ContainsKey(b.Label))
                {
                    reportsByUnit.Add(b.Label, new NAATAnalysisUnitReport());

                }

                reportsByUnit[b.Label].uniquePatientCount += b.PatientCount;
                reportsByUnit[b.Label].sampleCount += b.ItemsInBin;
                reportsByUnit[b.Label].admissionCount += b.PatientAdmissionCount;
                reportsByUnit[b.Label].PAPA = b.PatientAdmissionCount;
                reportsByUnit[b.Label].PASAM = b.ItemsInBin;
                reportsByUnit[b.Label].PAUP = b.PatientCount;

                foreach (string key in b.DataByPatientAdmissionTable.Keys)
                {
                    if (naatTable.ContainsKey(key)) //We don't care if the patient isn't even in the NAAT table
                    {
                        foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                        {
                            foreach (DataPoint dp in naatTable[key])
                            {
                                if ((dpa.AdmissionDate <= dp.SampleDate) && (dp.SampleDate - dpa.AdmissionDate).Days <= naatDistance) //We've found a matching NAAT nearby
                                {
                                    if (dp.CdiffResult == TestResult.Positive)
                                    {
                                        switch (dp.ToxinResult)
                                        {
                                            case TestResult.Positive:
                                                reportsByUnit[b.Label].PANPEP++;
                                                break;
                                            case TestResult.Negative:
                                                reportsByUnit[b.Label].PANPEN++;
                                                break;
                                            default:
                                                reportsByUnit[b.Label].PANPEI++;
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        reportsByUnit[b.Label].PANN++;
                                    }
                                }
                                else
                                {
                                    reportsByUnit[b.Label].PANT++;
                                }
                            }
                        }

                    }
                    else
                    {
                        reportsByUnit[b.Label].PANT += b.DataByPatientAdmissionTable[key].Count;
                    }
                }

            }

            foreach (Bin b in indAdm)
            {
                if (!reportsByUnit.ContainsKey(b.Label))
                {
                    reportsByUnit.Add(b.Label, new NAATAnalysisUnitReport());

                }

                reportsByUnit[b.Label].uniquePatientCount += b.PatientCount;
                reportsByUnit[b.Label].sampleCount += b.ItemsInBin;
                reportsByUnit[b.Label].admissionCount += b.PatientAdmissionCount;
                reportsByUnit[b.Label].IAPA = b.PatientAdmissionCount;
                reportsByUnit[b.Label].IASAM = b.ItemsInBin;
                reportsByUnit[b.Label].IAUP = b.PatientCount;

                foreach (string key in b.DataByPatientAdmissionTable.Keys)
                {
                    if (naatTable.ContainsKey(key)) //We don't care if the patient isn't even in the NAAT table
                    {
                        foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                        {
                            foreach (DataPoint dp in naatTable[key])
                            {
                                if ((dpa.AdmissionDate <= dp.SampleDate) && (dp.SampleDate - dpa.AdmissionDate).Days <= naatDistance) //We've found a matching NAAT nearby
                                {
                                    if (dp.CdiffResult == TestResult.Positive)
                                    {
                                        switch (dp.ToxinResult)
                                        {
                                            case TestResult.Positive:
                                                reportsByUnit[b.Label].IANPEP++;
                                                break;
                                            case TestResult.Negative:
                                                reportsByUnit[b.Label].IANPEN++;
                                                break;
                                            default:
                                                reportsByUnit[b.Label].IANPEI++;
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        reportsByUnit[b.Label].IANN++;
                                    }
                                }
                                else
                                {
                                    reportsByUnit[b.Label].IANT++;
                                }
                            }
                        }

                    }
                    else
                    {
                        reportsByUnit[b.Label].IANT += b.DataByPatientAdmissionTable[key].Count;
                    }
                }

            }


            StreamWriter sw = new StreamWriter(aggregateOutput);

            List<string> header = new List<string>();

            header.Add("Unit");
            header.Add("Negative On Admission");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("Positive On Admission");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("Indeterminate On Admission");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");



            StringBuilder topline = new StringBuilder();
            for (int i = 0; i < header.Count; i++)
            {
                topline.Append(header[i] + "\t");
            }
            sw.WriteLine(topline);

            header = new List<string>();

            header.Add("");
            for (int i = 0; i < 3; i++)
            {
                header.Add("Unique Patients");
                header.Add("Patient Admissions");
                header.Add("Patient Samples");
                header.Add("NAAT + / EIA +");
                header.Add("NAAT + / EIA -");
                header.Add("NAAT + / No EIA");
                header.Add("NAAT -");
                header.Add("Not Tested");
            }

           topline = new StringBuilder();
            for (int i = 0; i < header.Count; i++)
            {
                topline.Append(header[i] + "\t");
            }
            sw.WriteLine(topline);

            foreach(string key in reportsByUnit.Keys)
            {
                List<string> lineParts = new List<string>();

                lineParts.Add(key);

                lineParts.Add(reportsByUnit[key].NAUP.ToString());
                lineParts.Add(reportsByUnit[key].NAPA.ToString());
                lineParts.Add(reportsByUnit[key].NASAM.ToString());
                lineParts.Add(reportsByUnit[key].NANPEP.ToString());
                lineParts.Add(reportsByUnit[key].NANPEN.ToString());
                lineParts.Add(reportsByUnit[key].NANPEI.ToString());
                lineParts.Add(reportsByUnit[key].NANN.ToString());
                lineParts.Add(reportsByUnit[key].NANT.ToString());

                lineParts.Add(reportsByUnit[key].PAUP.ToString());
                lineParts.Add(reportsByUnit[key].PAPA.ToString());
                lineParts.Add(reportsByUnit[key].PASAM.ToString());
                lineParts.Add(reportsByUnit[key].PANPEP.ToString());
                lineParts.Add(reportsByUnit[key].PANPEN.ToString());
                lineParts.Add(reportsByUnit[key].PANPEI.ToString());
                lineParts.Add(reportsByUnit[key].PANN.ToString());
                lineParts.Add(reportsByUnit[key].PANT.ToString());

                lineParts.Add(reportsByUnit[key].IAUP.ToString());
                lineParts.Add(reportsByUnit[key].IAPA.ToString());
                lineParts.Add(reportsByUnit[key].IASAM.ToString());
                lineParts.Add(reportsByUnit[key].IANPEP.ToString());
                lineParts.Add(reportsByUnit[key].IANPEN.ToString());
                lineParts.Add(reportsByUnit[key].IANPEI.ToString());
                lineParts.Add(reportsByUnit[key].IANN.ToString());
                lineParts.Add(reportsByUnit[key].IANT.ToString());

               StringBuilder line = new StringBuilder();
                for (int i = 0; i < lineParts.Count; i++)
                {
                    line.Append(lineParts[i] + "\t");
                }
                sw.WriteLine(line);
            }
            sw.Close();
        }

        private Dictionary<string, List<DataPoint>> BuildNAATByPatientLookupTable(DataPoint[] naats)
        {
            Dictionary<string, List<DataPoint>> naatlkup = new Dictionary<string, List<DataPoint>>();

            for(int i =0; i < naats.Length; i++)
            {
                if(!naatlkup.ContainsKey(naats[i].MRN))
                {
                    naatlkup.Add(naats[i].MRN, new List<DataPoint>());
                }
                naatlkup[naats[i].MRN].Add(naats[i]);
            }

            return naatlkup;
        }

        public void CompareSurveillanceDataToNAATsSameAdmission(DataPoint[] surv, DataPoint[] naats, string aggregateOutput)
        {
            //Building NAAT patient database
            Dictionary<string, List<DataPoint>> naatTable = BuildNAATByPatientLookupTable(naats);



            Bin[] negOnAdm = DataFilter.StratifyOnUnitsAndAdmissionType(surv, AdmissionStatus.NegativeOnAdmission);
            Bin[] posOnAdm = DataFilter.StratifyOnUnitsAndAdmissionType(surv, AdmissionStatus.PositiveAdmission);
            Bin[] indAdm = DataFilter.StratifyOnUnitsAndAdmissionType(surv, AdmissionStatus.IndeterminateAdmission);


            Dictionary<string, NAATAnalysisUnitReport> reportsByUnit = new Dictionary<string, NAATAnalysisUnitReport>();

            foreach (Bin b in negOnAdm)
            {
                if (!reportsByUnit.ContainsKey(b.Label))
                {
                    reportsByUnit.Add(b.Label, new NAATAnalysisUnitReport());

                }

                reportsByUnit[b.Label].uniquePatientCount += b.PatientCount;
                reportsByUnit[b.Label].sampleCount += b.ItemsInBin;
                reportsByUnit[b.Label].admissionCount += b.PatientAdmissionCount;
                reportsByUnit[b.Label].NAPA = b.PatientAdmissionCount;
                reportsByUnit[b.Label].NASAM = b.ItemsInBin;
                reportsByUnit[b.Label].NAUP = b.PatientCount;

                foreach (string key in b.DataByPatientAdmissionTable.Keys)
                {

                    if (naatTable.ContainsKey(key)) //We don't care if the patient isn't even in the NAAT table
                    {
                        foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                        {
                            foreach (DataPoint dp in naatTable[key])
                            {
                                if ((dpa.AdmissionDate <= dp.SampleDate) && dp.SampleDate <= dpa.DischargeDate) //We've found a matching NAAT nearby
                                {
                                    if (dp.CdiffResult == TestResult.Positive)
                                    {
                                        switch (dp.ToxinResult)
                                        {
                                            case TestResult.Positive:
                                                reportsByUnit[b.Label].NANPEP++;
                                                break;
                                            case TestResult.Negative:
                                                reportsByUnit[b.Label].NANPEN++;
                                                break;
                                            default:
                                                reportsByUnit[b.Label].NANPEI++;
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        reportsByUnit[b.Label].NANN++;
                                    }
                                }
                                else
                                {
                                    reportsByUnit[b.Label].NANT++;
                                }
                            }
                        }

                    }
                    else
                    {
                        reportsByUnit[b.Label].NANT += b.DataByPatientAdmissionTable[key].Count;
                    }
                }

            }

            foreach (Bin b in posOnAdm)
            {
                if (!reportsByUnit.ContainsKey(b.Label))
                {
                    reportsByUnit.Add(b.Label, new NAATAnalysisUnitReport());

                }

                reportsByUnit[b.Label].uniquePatientCount += b.PatientCount;
                reportsByUnit[b.Label].sampleCount += b.ItemsInBin;
                reportsByUnit[b.Label].admissionCount += b.PatientAdmissionCount;
                reportsByUnit[b.Label].PAPA = b.PatientAdmissionCount;
                reportsByUnit[b.Label].PASAM = b.ItemsInBin;
                reportsByUnit[b.Label].PAUP = b.PatientCount;

                foreach (string key in b.DataByPatientAdmissionTable.Keys)
                {
                    if (naatTable.ContainsKey(key)) //We don't care if the patient isn't even in the NAAT table
                    {
                        foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                        {
                            foreach (DataPoint dp in naatTable[key])
                            {
                                if ((dpa.AdmissionDate <= dp.SampleDate) && dp.SampleDate <= dpa.DischargeDate) //We've found a matching NAAT nearby
                                {
                                    if (dp.CdiffResult == TestResult.Positive)
                                    {
                                        switch (dp.ToxinResult)
                                        {
                                            case TestResult.Positive:
                                                reportsByUnit[b.Label].PANPEP++;
                                                break;
                                            case TestResult.Negative:
                                                reportsByUnit[b.Label].PANPEN++;
                                                break;
                                            default:
                                                reportsByUnit[b.Label].PANPEI++;
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        reportsByUnit[b.Label].PANN++;
                                    }
                                }
                                else
                                {
                                    reportsByUnit[b.Label].PANT++;
                                }
                            }
                        }

                    }
                    else
                    {
                        reportsByUnit[b.Label].PANT += b.DataByPatientAdmissionTable[key].Count;
                    }
                }

            }

            foreach (Bin b in indAdm)
            {
                if (!reportsByUnit.ContainsKey(b.Label))
                {
                    reportsByUnit.Add(b.Label, new NAATAnalysisUnitReport());

                }

                reportsByUnit[b.Label].uniquePatientCount += b.PatientCount;
                reportsByUnit[b.Label].sampleCount += b.ItemsInBin;
                reportsByUnit[b.Label].admissionCount += b.PatientAdmissionCount;
                reportsByUnit[b.Label].IAPA = b.PatientAdmissionCount;
                reportsByUnit[b.Label].IASAM = b.ItemsInBin;
                reportsByUnit[b.Label].IAUP = b.PatientCount;

                foreach (string key in b.DataByPatientAdmissionTable.Keys)
                {
                    if (naatTable.ContainsKey(key)) //We don't care if the patient isn't even in the NAAT table
                    {
                        foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                        {
                            foreach (DataPoint dp in naatTable[key])
                            {
                                if ((dpa.AdmissionDate <= dp.SampleDate) && dp.SampleDate <= dpa.DischargeDate) //We've found a matching NAAT nearby
                                {
                                    if (dp.CdiffResult == TestResult.Positive)
                                    {
                                        switch (dp.ToxinResult)
                                        {
                                            case TestResult.Positive:
                                                reportsByUnit[b.Label].IANPEP++;
                                                break;
                                            case TestResult.Negative:
                                                reportsByUnit[b.Label].IANPEN++;
                                                break;
                                            default:
                                                reportsByUnit[b.Label].IANPEI++;
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        reportsByUnit[b.Label].IANN++;
                                    }
                                }
                                else
                                {
                                    reportsByUnit[b.Label].IANT++;
                                }
                            }
                        }

                    }
                    else
                    {
                        reportsByUnit[b.Label].IANT += b.DataByPatientAdmissionTable[key].Count;
                    }
                }

            }


            StreamWriter sw = new StreamWriter(aggregateOutput);

            List<string> header = new List<string>();

            header.Add("Unit");
            header.Add("Negative On Admission");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("Positive On Admission");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("Indeterminate On Admission");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");



            StringBuilder topline = new StringBuilder();
            for (int i = 0; i < header.Count; i++)
            {
                topline.Append(header[i] + "\t");
            }
            sw.WriteLine(topline);

            header = new List<string>();

            header.Add("");
            for (int i = 0; i < 3; i++)
            {
                header.Add("Unique Patients");
                header.Add("Patient Admissions");
                header.Add("Patient Samples");
                header.Add("NAAT + / EIA +");
                header.Add("NAAT + / EIA -");
                header.Add("NAAT + / No EIA");
                header.Add("NAAT -");
                header.Add("Not Tested");
            }

            topline = new StringBuilder();
            for (int i = 0; i < header.Count; i++)
            {
                topline.Append(header[i] + "\t");
            }
            sw.WriteLine(topline);

            foreach (string key in reportsByUnit.Keys)
            {
                List<string> lineParts = new List<string>();

                lineParts.Add(key);

                lineParts.Add(reportsByUnit[key].NAUP.ToString());
                lineParts.Add(reportsByUnit[key].NAPA.ToString());
                lineParts.Add(reportsByUnit[key].NASAM.ToString());
                lineParts.Add(reportsByUnit[key].NANPEP.ToString());
                lineParts.Add(reportsByUnit[key].NANPEN.ToString());
                lineParts.Add(reportsByUnit[key].NANPEI.ToString());
                lineParts.Add(reportsByUnit[key].NANN.ToString());
                lineParts.Add(reportsByUnit[key].NANT.ToString());

                lineParts.Add(reportsByUnit[key].PAUP.ToString());
                lineParts.Add(reportsByUnit[key].PAPA.ToString());
                lineParts.Add(reportsByUnit[key].PASAM.ToString());
                lineParts.Add(reportsByUnit[key].PANPEP.ToString());
                lineParts.Add(reportsByUnit[key].PANPEN.ToString());
                lineParts.Add(reportsByUnit[key].PANPEI.ToString());
                lineParts.Add(reportsByUnit[key].PANN.ToString());
                lineParts.Add(reportsByUnit[key].PANT.ToString());

                lineParts.Add(reportsByUnit[key].IAUP.ToString());
                lineParts.Add(reportsByUnit[key].IAPA.ToString());
                lineParts.Add(reportsByUnit[key].IASAM.ToString());
                lineParts.Add(reportsByUnit[key].IANPEP.ToString());
                lineParts.Add(reportsByUnit[key].IANPEN.ToString());
                lineParts.Add(reportsByUnit[key].IANPEI.ToString());
                lineParts.Add(reportsByUnit[key].IANN.ToString());
                lineParts.Add(reportsByUnit[key].IANT.ToString());

                StringBuilder line = new StringBuilder();
                for (int i = 0; i < lineParts.Count; i++)
                {
                    line.Append(lineParts[i] + "\t");
                }
                sw.WriteLine(line);
            }
            sw.Close();
        }



    }

    class NAATAnalysisUnitReport
    {
        public int uniquePatientCount = 0;
        public int sampleCount = 0;
        public int admissionCount = 0;

        public int NANPEP = 0; //Neg admission, NAAT Pos, EIA Pos
        public int NANPEN = 0;
        public int NANPEI = 0;
        public int NANN = 0;
        public int NAPA = 0;
        public int NAUP = 0;
        public int NASAM = 0;
        public int NANT = 0;

        public int PANPEP = 0;
        public int PANPEN = 0;
        public int PANPEI = 0;
        public int PANN = 0;
        public int PAPA = 0;
        public int PAUP = 0;
        public int PASAM = 0;
        public int PANT = 0;

        public int IANPEP = 0;
        public int IANPEN = 0;
        public int IANPEI = 0;
        public int IANN = 0;
        public int IAPA = 0;
        public int IAUP = 0;
        public int IASAM = 0;
        public int IANT = 0;


    }


    
}
