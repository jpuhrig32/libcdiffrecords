using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords.Reports
{
    public enum ComparisonType
    {
        ByAdmissionStatus,
        ByEndResult,
    };

    public enum NAATCountingType
    {
        OncePerPatient,
        PerSample,
    };

    public class NAATComparisonReportLine : IReportLine
    {
        Bin unitSurvBin;
        DataPoint[] naatBin;
        int naatWindow = 90;
        bool ignoreIndeterminates = false;
        ComparisonType compType;
        int blockCount;
        NAATCountingType naatType;
     

        private Dictionary<string, List<DataPoint>> naatTable;

        public NAATComparisonReportLine(Bin unit, DataPoint[] naat, int naatDistInDays, ComparisonType ct, bool ignoreIndeterminedAdm, NAATCountingType nt)
        {
            unitSurvBin = unit;
            naatBin = naat;
            naatWindow = naatDistInDays;
            ignoreIndeterminates = ignoreIndeterminedAdm;
            naatType = nt;
            compType = ct;
            if (ct == ComparisonType.ByEndResult)
                blockCount = 5;
            else
            {
                blockCount = 3;
                if (ignoreIndeterminedAdm)
                    blockCount = 2;
                    
            }
        }
        
        public Bin ReportBin { get => unitSurvBin; set => unitSurvBin = value; }

        public string[] GenerateReportHeaderLine()
        {
            List<string> header = new List<string>();
            header.Add("Unit");
            header.Add("Total Unique Patients");
            header.Add("Total Admissions");
            
            if(compType == ComparisonType.ByAdmissionStatus)
            {
                header = GenerateBlockForTopRow(header, "Negative On Admission");
                header = GenerateBlockForTopRow(header, "Positive On Admission");
                if(!ignoreIndeterminates)
                 header = GenerateBlockForTopRow(header, "Indeterminate On Admission");
            }
            else if(compType == ComparisonType.ByEndResult)
            {
                header = GenerateBlockForTopRow(header, "Remained Negative");
                header = GenerateBlockForTopRow(header, "Positive (regardless of timing)");
                header = GenerateBlockForTopRow(header, "Positive on Admission");
                header = GenerateBlockForTopRow(header, "Negative on Admission- turned Positive");
                header = GenerateBlockForTopRow(header, "Positive- no admission sample");

            }
            return header.ToArray();
        }

        private List<String> GenerateBlockForTopRow(List<string> header, string label)
        {
            
            header.Add(label);
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            header.Add("");
            return header;
        }

        public string[] GenerateReportSubHeaderLine()
        {
            List<string> header = new List<string>();
            header.Add("");
            header.Add("");
            header.Add("");
            for (int i = 0; i < blockCount; i++)
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
            return header.ToArray();
        }

        public string[] GenerateReportLine()
        {
            List<string> reportParts = new List<string>();

           naatTable = BuildNAATByPatientLookupTable(naatBin);
            NAATAnalysis globals = new NAATAnalysis();

            globals.admCount = unitSurvBin.PatientAdmissionCount;
            globals.sampleCount = unitSurvBin.ItemsInBin;
            globals.uniquePt = unitSurvBin.PatientCount;

            reportParts.Add(unitSurvBin.Label);
            reportParts.Add(globals.uniquePt.ToString());
            reportParts.Add(globals.admCount.ToString());

            Bin[] splitBins = new Bin[1];
            if (compType == ComparisonType.ByAdmissionStatus)
            {
                if (!ignoreIndeterminates)
                {
                    splitBins = new Bin[3];
                    splitBins[2] = DataFilter.FilterByAdmissionType(unitSurvBin, AdmissionStatus.IndeterminateAdmission);
                }
                else
                {
                    splitBins = new Bin[2];
                }

                splitBins[0] = DataFilter.FilterByAdmissionType(unitSurvBin, AdmissionStatus.NegativeOnAdmission);
                splitBins[1] = DataFilter.FilterByAdmissionType(unitSurvBin, AdmissionStatus.PositiveOnAdmission);
            }
            if(compType == ComparisonType.ByEndResult)
            {
                splitBins = new Bin[5];
                splitBins[0] = DataFilter.FilterByAdmissionType(unitSurvBin, AdmissionStatus.NegativeAdmission);
                splitBins[1] = DataFilter.FilterByAdmissionType(unitSurvBin, AdmissionStatus.PositiveAdmission);
                splitBins[2] = DataFilter.FilterByAdmissionType(unitSurvBin, AdmissionStatus.PositiveOnAdmission);
                splitBins[3] = DataFilter.FilterByAdmissionType(unitSurvBin, AdmissionStatus.NegativeOnAdmission_TurnedPositive);
                splitBins[4] = DataFilter.FilterByAdmissionType(unitSurvBin, AdmissionStatus.PositiveNoAdmitSample);
            }
           

            NAATAnalysis[] na = new NAATAnalysis[splitBins.Length];

            for(int i =0; i < splitBins.Length; i++)
            {
                if(naatType == NAATCountingType.OncePerPatient)
                    na[i] = AnalyzeNAATPerPatient(splitBins[i], na[i]);
                if (naatType == NAATCountingType.PerSample)
                    na[i] = AnalyzeNAATPerSample(splitBins[i], na[i]);


                //Here's where we start printing the line stats
                reportParts.Add(na[i].uniquePt.ToString());
                reportParts.Add(na[i].admCount.ToString());
                reportParts.Add(na[i].sampleCount.ToString());
                reportParts.Add(na[i].naatPosEiaPos.ToString());
                reportParts.Add(na[i].naatPosEiaNeg.ToString());
                reportParts.Add(na[i].naatPosEiaInd.ToString());
                reportParts.Add(na[i].naatNeg.ToString());
                reportParts.Add(na[i].notTested.ToString());
            }

            
               Bin b = AnalyzeNAATPerSample(DataFilter.RemoveUnsavedSamples(ReportBin), "./9NTPatientSurvSamples.csv");
                 

                TabDelimWriter.WriteBinData("./" + ReportBin.Label + "Samples.txt", b);
            
            return reportParts.ToArray();

        }

        Dictionary<string, int> ntTable = new Dictionary<string, int>();
        private NAATAnalysis AnalyzeNAATPerPatient(Bin b, NAATAnalysis na)
        {
            na.admCount = b.PatientAdmissionCount;
            na.uniquePt = b.PatientCount;
            na.sampleCount = b.ItemsInBin;

            foreach (string key in b.DataByPatientAdmissionTable.Keys)
            {
                NAATStatus ns = NAATStatus.NotTested;
                if (naatTable.ContainsKey(key))
                {

                    foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                    {

                        foreach (DataPoint dp in naatTable[key])
                        {
                            if ((dpa.AdmissionDate < dp.SampleDate) && (((dp.SampleDate - dpa.AdmissionDate).Days < naatWindow) || naatWindow <= 0))
                            {
                                
                                if (dp.CdiffResult == TestResult.Positive)
                                {
                                    if(!ntTable.ContainsKey(dpa.MRN + dpa.AdmissionDate.ToShortDateString()))
                                    {
                                        //DataTap.data.Add(dp);
                                        DataPoint dp2 = dpa.Points[0];
                                        dp2.Notes = dpa.AdmissionStatus.ToString() +  " Result:" + dp.CdiffResult.ToString() + " / " + dp.ToxinResult.ToString();
                                         DataTap.data.Add(dp2);
                                        DataTap.data.Add(dp);
                                        ntTable.Add(dpa.MRN + dpa.AdmissionDate.ToShortDateString(), 1);
                                    }
                                    
                                    switch (dp.ToxinResult)
                                    {
                                        case TestResult.Positive:
                                            ns = CompareNAATStatus(ns, NAATStatus.PosPos);
                                            break;
                                        case TestResult.Negative:
                                            ns = CompareNAATStatus(ns, NAATStatus.PosNeg);
                                            break;
                                        default:
                                            ns = CompareNAATStatus(ns, NAATStatus.PosInd);
                                            break;

                                    }
                                }
                                else
                                {
                                    ns = CompareNAATStatus(ns, NAATStatus.Neg);
                                }

                            }

                        }
                    }

                }
                na = AddNAATPatient(na, ns);

            }
            return na;
        }

        private NAATAnalysis AnalyzeNAATPerSample(Bin b, NAATAnalysis na)
        {
            na.admCount = b.PatientAdmissionCount;
            na.uniquePt = b.PatientCount;
            na.sampleCount = b.ItemsInBin;
            
            foreach(DataPoint np in naatBin)
            {
                if(naatTable.ContainsKey(np.MRN))
                {
                    foreach (string key in b.DataByPatientAdmissionTable.Keys)
                    {
                        foreach (Admission dpa in b.DataByPatientAdmissionTable[key])
                        {
                            foreach (DataPoint dp in dpa.Points)
                            {
                                if(IsNAATInRange(dp, np, naatWindow))
                                {
                                    if (dp.CdiffResult == TestResult.Positive)
                                    {
                                        switch (dp.ToxinResult)
                                        {
                                            case TestResult.Positive:
                                                na = AddNAATPatient(na, NAATStatus.PosPos);
                                                break;
                                            case TestResult.Negative:
                                                na = AddNAATPatient(na, NAATStatus.PosNeg);
                                                break;
                                            default:
                                                na = AddNAATPatient(na, NAATStatus.PosInd);
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        na = AddNAATPatient(na, NAATStatus.Neg);
                                    }
                                }
                            }
                        }
                    }
                }
            }
           

            return na;
        }

        private bool IsNAATInRange(DataPoint samplePoint, DataPoint naatPoint, int naatWindow)
        {
            if(samplePoint.Test != naatPoint.Test) //This isn't a NAAT Sample, so we should run the comparison
            {
                return ((samplePoint.SampleDate > naatPoint.SampleDate) && (samplePoint.SampleDate - naatPoint.SampleDate).Days <= naatWindow);
            }
            ///If this is a Clinical inpatient NAAT, we're only including it in the list if it is already positive, so, if we hit this, then we already have a positive clinical NAAT. 
            return true;
        }
        public Bin AnalyzeNAATPerSample(Bin bin, string filename)
        {
            NAATAnalysis na = new NAATAnalysis();

            na.admCount = bin.PatientAdmissionCount;
            na.uniquePt = bin.PatientCount;
            na.sampleCount = bin.ItemsInBin;
            Bin b = new Bin("SurvWithPreeceedingNAATS");
            Dictionary<string, int> counted = new Dictionary<string, int>();

            foreach (DataPoint np in naatBin)
            {
                if (naatTable.ContainsKey(np.MRN) && bin.DataByPatientAdmissionTable.ContainsKey(np.MRN))
                {

                    foreach (Admission dpa in bin.DataByPatientAdmissionTable[np.MRN])
                    {
                        if (dpa.Points[0].SampleDate < np.SampleDate)
                        {
                            foreach (DataPoint dp in dpa.Points)
                            {
                                if ((dp.SampleDate < np.SampleDate) && ((dp.SampleDate - np.SampleDate).Days <= naatWindow))
                                {

                                    //counted.Add(dp.mrn + dp.sampleDate.ToShortDateString(), 1);
                                    DataPoint dp2 = dp;
                                    dp2.Unit = np.SampleDate.ToShortDateString();
                                    // dp2.SampleID = (dp2.SampleID + "_" + dp2.MRN + "_" + dp2.SampleDate.ToShortDateString());
                                    dp2.CdiffResult = np.CdiffResult;
                                    dp2.ToxinResult = np.ToxinResult;
                                    b.Add(dp2);





                                }
                            }
                        }
                    }
                    
                }
            }
          //  TabDelimWriter.WriteBinData(filename, b);

            return b;
        }
       
        private NAATAnalysis AddNAATPatient(NAATAnalysis na, NAATStatus ns)
        {
            switch(ns)
            {
                case NAATStatus.Neg:
                    na.naatNeg++;
                    break;
                case NAATStatus.NotTested:
                    na.notTested++;
                    break;
                case NAATStatus.PosInd:
                    na.naatPosEiaInd++;
                    break;
                case NAATStatus.PosNeg:
                    na.naatPosEiaNeg++;
                    break;
                case NAATStatus.PosPos:
                    na.naatPosEiaPos++;
                    break;
            }
            return na;
        }
        private NAATStatus CompareNAATStatus(NAATStatus ns1, NAATStatus ns2)
        {
            if (ns1 == NAATStatus.PosPos || ns2 == NAATStatus.PosPos)
                return NAATStatus.PosPos;
            if (ns1 == NAATStatus.PosNeg || ns2 == NAATStatus.PosNeg)
                return NAATStatus.PosNeg;
            if (ns1 == NAATStatus.PosInd || ns2 == NAATStatus.PosInd)
                return NAATStatus.PosInd;
            if (ns1 == NAATStatus.Neg || ns2 == NAATStatus.Neg)
                return NAATStatus.Neg;
            return NAATStatus.NotTested;
        }
        

        private Dictionary<string, List<DataPoint>> BuildNAATByPatientLookupTable(DataPoint[] naats)
        {
            Dictionary<string, List<DataPoint>> naatlkup = new Dictionary<string, List<DataPoint>>();

            for (int i = 0; i < naats.Length; i++)
            {
                if (!naatlkup.ContainsKey(naats[i].MRN))
                {
                    naatlkup.Add(naats[i].MRN, new List<DataPoint>());
                }
                naatlkup[naats[i].MRN].Add(naats[i]);
            }

            return naatlkup;
        }


    }

    struct NAATAnalysis
    {
        public int uniquePt;
        public int sampleCount;
        public int admCount;
        public int naatPosEiaPos;
        public int naatPosEiaNeg;
        public int naatPosEiaInd;
        public int naatNeg;
        public int notTested;
    }

    enum NAATStatus
    {
        PosPos,
        PosNeg,
        PosInd,
        Neg,
        NotTested,
    };

}
