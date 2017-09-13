using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords
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

    public class StratificationAnalysis
    {
        int admWindow = 3; //We're defining an admission sample as being within 3 days of admission to start

        public StratificationAnalysis(int admissionWindow)
        {
            admWindow = admissionWindow;
        }

        public StratificationAnalysis()
        {
            admWindow = 3;
        }

        public Bin[] StratifyOnAge(DataPoint[] points, bool calcAgeByFirstAdm)
        {

            Bin[] bins = new Bin[17];
            bins[0] = new Bin("Age: 0-17", 0, 17);
            bins[1] = new Bin("Age: 18-30", 18, 30);
            bins[2] = new Bin("Age: 31-40", 31, 40);

            //Construction of bins
            for (int i = 3; i < bins.Length; i++)
            {
                int minAge = ((i - 1) * 5) + 31;
                int maxAge = minAge + 4;
                bins[i] = new Bin("Age: " + minAge + "-" + maxAge, minAge, maxAge);
            }


            Bin mainBin = CreateBinFromDataPoints(points);
            mainBin.SortBinData();



            foreach (string key in mainBin.DataByPatientAdmissionTable.Keys)
            {
                foreach (DataPointAdmission dpa in mainBin.DataByPatientAdmissionTable[key])
                {
                    int age = 0;
                    if (calcAgeByFirstAdm)
                    {
                        age = (int)((mainBin.DataByPatientAdmissionTable[key][0].admissionDate - dpa.points[0].dob).Days / 365.25);
                    }
                    else
                    {
                        age = (int)((dpa.points[0].admDate - dpa.points[0].dob).Days / 365.25);
                    }

                    for (int i = 0; i < bins.Length; i++)
                    {
                        if (age >= bins[i].min && age <= bins[i].max)
                        {
                            foreach (DataPoint dp in dpa.points)
                            {
                                bins[i].Add(dp);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < bins.Length; i++)
            {
                bins[i].SortBinData();
            }

            return bins;


        }

        public Bin[] StratifyOnAge(Bin[] bins, bool calcAgeByFirstAdm)
        {
            return StratifyOnAge(ExtractDataPointsFromBins(bins), calcAgeByFirstAdm);
        }

        public Bin[] StratifyOnUnits(Bin[] bins)
        {
            return StratifyOnUnits(ExtractDataPointsFromBins(bins));
        }

        public Bin[] StratifyOnUnits(DataPoint[] points)
        {
            Bin mainBin = CreateBinFromDataPoints(points);
            mainBin.SortBinData();

            Dictionary<string, Bin> bins = new Dictionary<string, Bin>();

            /*
            for (int i = 0; i < points.Length; i++)
            {
                if (!bins.ContainsKey(points[i].unit))
                {
                    bins.Add(points[i].unit, new Bin(points[i].unit));
                }

                bins[points[i].unit].Add(points[i]);
            }
            */
            foreach (string key in mainBin.DataByPatientAdmissionTable.Keys)
            {
                foreach (DataPointAdmission dpa in mainBin.DataByPatientAdmissionTable[key])
                {
                    if (!bins.ContainsKey(dpa.unit))
                    {
                        bins.Add(dpa.unit, new Bin(dpa.unit));
                    }

                    foreach (DataPoint dp in dpa.points)
                        bins[dpa.unit].Add(dp);

                }

            }

            List<Bin> binList = new List<Bin>();
            binList.AddRange(bins.Values);

            binList.Sort((x, y) => x.Label.CompareTo(y.Label));


            return binList.ToArray();

        }

        public Bin[] StratifyOnPositiveAdmissions(DataPoint[] points)
        {
            Bin mainBin = CreateBinFromDataPoints(points);

            Bin positives = new Bin("Positive Admissions");

            foreach (string key in mainBin.DataByPatientAdmissionTable.Keys)
            {
                foreach (DataPointAdmission dpa in mainBin.DataByPatientAdmissionTable[key])
                {
                    bool foundPos = false;
                    for (int i = 0; i < dpa.points.Count; i++)
                    {
                        if (dpa.points[i].cdResult == Cdiff.Positive)
                            foundPos = true;
                    }

                    if (foundPos)
                    {
                        foreach (DataPoint point in dpa.points)
                            positives.Add(point);
                    }
                }
            }

            Bin[] bins = new Bin[1];
            bins[0] = positives;

            return bins;
        }

        public Bin[] StratifyOnMultipleSampleAdmissions(DataPoint[] points)
        {
            Bin mainBin = CreateBinFromDataPoints(points);
            Bin[] retBin = new Bin[1];
            retBin[0] = new Bin("");
            foreach (string key in mainBin.DataByPatientAdmissionTable.Keys)
            {
                foreach (DataPointAdmission dpa in mainBin.DataByPatientAdmissionTable[key])
                {
                    if (dpa.points.Count > 1)
                        foreach (DataPoint dp in dpa.points)
                            retBin[0].Add(dp);
                }
            }

            retBin[0].SortBinData();
            return retBin;

        }

        public Bin[] StratifyOnPatientsWithMultipleSamples(DataPoint[] points)
        {
            Bin mainBin = CreateBinFromDataPoints(points);

            Bin[] retBin = new Bin[1];
            retBin[0] = new Bin("");

            foreach (string key in mainBin.DataByPatient.Keys)
            {
                if (mainBin.DataByPatient[key].Count > 1)
                {
                    foreach (DataPoint dp in mainBin.DataByPatient[key])
                    {
                        retBin[0].Add(dp);
                    }
                }
            }
            retBin[0].SortBinData();
            return retBin;
        }

    

        public Bin CreateBinFromDataPoints(DataPoint[] points)
        {
            Bin mainBin = new Bin("");
            for (int i = 0; i < points.Length; i++)
            {
                mainBin.Add(points[i]);
            }

            mainBin.SortBinData();
            return mainBin;
        }

        public Bin StratifyOnAdmissionType(Bin bin, AdmissionType type)
        {
            Bin ret = new Bin(bin.Label);

            foreach (string key in bin.DataByPatientAdmissionTable.Keys)
            {
                foreach (DataPointAdmission dpa in bin.DataByPatientAdmissionTable[key])
                {
                   
                    switch(type)
                    {
                        case AdmissionType.NegativeOnAdmStaysNegative:
                            if (!IsPositiveAdmission(dpa))
                            {
                                foreach (DataPoint dp in dpa.points)                         
                                  ret.Add(dp);
                            }
                            break;
                        case AdmissionType.NegativeOnAdm:
                            if (dpa.points[0].cdResult == Cdiff.Negative && (dpa.points[0].sampleDate - dpa.points[0].admDate).Days <= admWindow)
                            {
                                foreach (DataPoint dp in dpa.points)
                                    ret.Add(dp);
                            }
                            break;
                        case AdmissionType.NegativeOnAdmTurnsPositive:
                            if (IsPositiveAdmission(dpa) && dpa.points[0].cdResult == Cdiff.Negative && (dpa.points[0].sampleDate - dpa.points[0].admDate).Days <= admWindow)
                            {
                                foreach (DataPoint dp in dpa.points)
                                    ret.Add(dp);
                            }
                            break;
                        case AdmissionType.PositiveNoAdmSample:
                            if (IsPositiveAdmission(dpa)  && (dpa.points[0].sampleDate - dpa.points[0].admDate).Days <  admWindow)
                            {
                                foreach (DataPoint dp in dpa.points)
                                    ret.Add(dp);
                            }
                            break;
                        case AdmissionType.PositiveOnAdm:
                            if (IsPositiveAdmission(dpa) && dpa.points[0].cdResult == Cdiff.Positive && (dpa.points[0].sampleDate - dpa.points[0].admDate).Days <= admWindow) //Needs to have the first sample be positive, and on the same day 
                            {
                                foreach (DataPoint dp in dpa.points)
                                    ret.Add(dp);
                            }
                            break;
                        case AdmissionType.IndeterminateAdmission:
                            if ((dpa.points[0].sampleDate - dpa.points[0].admDate).Days > admWindow)
                            {
                                foreach (DataPoint dp in dpa.points)
                                    ret.Add(dp);
                            }
                            break;
                        default:
                            break;

                    }
                    



                }
            }

            return ret;

        }

        public Bin[] StratifyOnUnitsAndAdmissionType(DataPoint[] data, AdmissionType type)
        {
            Bin[] startBins = StratifyOnUnits(data);
            Bin[] retBins = new Bin[startBins.Length];
            for(int i = 0; i < startBins.Length; i++)
            {
                retBins[i] = StratifyOnAdmissionType(startBins[i], type);
            }

            return retBins;
        }
        

        public SurveillanceReportLine[] CreateSurveillanceReportFromBins(Bin[] bins)
        {
            SurveillanceReportLine[] lines = new SurveillanceReportLine[bins.Length];

            for(int i = 0; i < bins.Length; i++)
            {
                lines[i] = new SurveillanceReportLine(bins[i].Label);
                lines[i].SampleCount = bins[i].ItemsInBin;
                lines[i].PatientCount = bins[i].PatientCount;
                lines[i].PatientAdmissionsCount = PatientAdmissionCount(bins[i]);      

                foreach (string key in bins[i].DataByPatientAdmissionTable.Keys)
                {
                    foreach (DataPointAdmission dpa in bins[i].DataByPatientAdmissionTable[key])
                    {
                        if (IsPositiveAdmission(dpa))
                            lines[i].PositiveSamples++;

                      
                        if (dpa.points.Count > 0)
                        {
                            if ((dpa.points[0].sampleDate - dpa.admissionDate).Days <= admWindow) //This is an admission with a sample taken at admission
                            {
                                if (dpa.points[0].cdResult == Cdiff.Positive)
                                    lines[i].PositiveOnAdmission++;
                                else if(dpa.points.Count > 1)
                                {
                                    bool positive = false;

                                    for(int z = 1; z < dpa.points.Count; z++)
                                    {
                                        if (dpa.points[z].cdResult == Cdiff.Positive)
                                            positive = true;
                                    }
                                    if(positive)
                                        lines[i].PositiveDuringStay++;
                                }

                            }
                            else //No sample taken at admission
                            {
                                bool positive = false;
                                foreach(DataPoint dp in dpa.points)
                                {
                                    if (dp.cdResult == Cdiff.Positive)
                                        positive = true;
                                }
                                if (positive)
                                    lines[i].PositiveNoAdmissionSample++;
                            }
                        }
                    }
                }



            }

            return lines;
        }

        

        private int PositiveCount(Bin bin)
        {
            int pc = 0;
            for(int i =0; i < bin.ItemsInBin; i++)
            {
                if (bin.Data[i].cdResult == Cdiff.Positive)
                    pc++;
            }


            return pc;
        }

        private int PatientAdmissionCount(Bin bin)
        {
            int pc = 0;

            foreach(string key in bin.DataByPatientAdmissionTable.Keys)
            {
                foreach(DataPointAdmission dpa in bin.DataByPatientAdmissionTable[key])
                {
                    if (dpa.points.Count > 0)
                        pc++;
                }
            }
            return pc;
        }

        private int PositiveOnAdmission(Bin bin)
        {
            int ct = 0;

            foreach(string key in bin.DataByPatientAdmissionTable.Keys)
            {
                foreach(DataPointAdmission dpa in bin.DataByPatientAdmissionTable[key])
                {
                    if (dpa.points.Count >= 1 && (dpa.points[0].sampleDate - dpa.admissionDate).Days <= admWindow)
                        ct++;
                }
            }

            return ct;

        }

        public SurveillanceReportLine[] GenerateWeeklySurveillanceReport(DataPoint[] oldData, DataPoint[] currentData)
        {
            Dictionary<string, SurveillanceReportLine> lines = new Dictionary<string, SurveillanceReportLine>();

            Dictionary<string, bool> oldSamples = new Dictionary<string, bool>();

            List<DataPoint> newDPs = new List<DataPoint>();


            foreach(DataPoint dp in oldData)
            {
                string dpid = GenerateDatapointID(dp);
                if (!oldSamples.ContainsKey(dpid))
                {
                    oldSamples.Add(dpid, true);
                }
            }

            foreach(DataPoint dp in currentData)
            {
                if (!oldSamples.ContainsKey(GenerateDatapointID(dp)))
                    newDPs.Add(dp);
            }

            Bin oldDataBin = CreateBinFromDataPoints(oldData);
            


            foreach(DataPoint dp in newDPs)
            {
                if (!lines.ContainsKey(dp.unit))
                    lines.Add(dp.unit, new SurveillanceReportLine(dp.unit));
                if (dp.cdResult == Cdiff.Positive)
                {
                    if ((dp.sampleDate - dp.admDate).Days <= admWindow)
                    {
                        lines[dp.unit].PositiveOnAdmission++;

                    }
                    else
                    {
                        DataPointAdmission dpa;

                        if(oldDataBin.TryGetDataPointAdmission(dp, out dpa)) //Finding the admission this sample belongs to
                        {
                            if (dpa != null)
                            {
                                if (dpa.points[0].cdResult == Cdiff.Positive) 
                                {
                                    if((dpa.points[0].sampleDate - dpa.admissionDate).Days <= admWindow) //Checking to see if this admission was positive on admission
                                    {
                                        lines[dp.unit].PositiveOnAdmission++;
                                    }
                                    else
                                    {
                                        lines[dp.unit].PositiveNoAdmissionSample++;
                                    }
                                }
                                else
                                {
                                    if ((dpa.points[0].sampleDate - dpa.admissionDate).Days <= admWindow)
                                    {
                                        lines[dp.unit].PositiveDuringStay++;
                                    }
                                    else
                                        lines[dp.unit].PositiveNoAdmissionSample++;
                                }


                            }
                        }
                        else //Can't find an existing admission for this one, so it falls under "Positive, no admission"
                        {
                            lines[dp.unit].PositiveNoAdmissionSample++;
                        }
                    }

                }
            }

            Bin[] newDPBin = StratifyOnUnits(newDPs.ToArray());

            foreach(Bin b in newDPBin)
            {
                if(lines.ContainsKey(b.Label))
                {
                    lines[b.Label].PatientCount = b.PatientCount;
                    lines[b.Label].PatientAdmissionsCount = b.PatientAdmissionCount;
                    lines[b.Label].SampleCount = b.ItemsInBin;
                }
            }
            return lines.Values.ToArray();

        }


        private string GenerateDatapointID(DataPoint dp)
        {
            return dp.mrn + dp.admDate.ToShortDateString() + dp.unit + dp.sampleDate.ToShortDateString();
        }

        private bool IsPositiveAdmission(DataPointAdmission dpa)
        {
            for(int i =0; i < dpa.points.Count; i++)
            {
                if (dpa.points[i].cdResult == Cdiff.Positive)
                    return true;
            }

            return false;

        }

        private DataPoint[] ExtractDataPointsFromBins(Bin[] bins)
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
    }

   
   
}
