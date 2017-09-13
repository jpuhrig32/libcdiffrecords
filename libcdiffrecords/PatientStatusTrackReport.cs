using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords
{
    public class PatientStatusTrackReport
    {

        public static String[] CreateTrackReportWithDates(Bin[] bins)
        {
            return CreateTrackReportWithDates(bins, true);
        }

        public static String[] CreateTrackReportWithDates(Bin[] bins, bool printBinLabels)
        {
            List<string> reportLines = new List<string>();

            string topLine = "Patient Number\tPatient Name\tMRN\tSex\tAge\tTransition Count\tSampleDate\tCdiff Result\t";
            reportLines.Add(topLine);

            foreach (Bin bin in bins)
            {
                if (printBinLabels)
                {
                    reportLines.Add(bin.Label);
                }
                foreach (string key in bin.DataByPatient.Keys)
                {
                    StringBuilder sb = new StringBuilder();

                    if (bin.DataByPatient.Count > 0)
                    {
                        sb.Append(bin.DataByPatient[key][0].SampleID);
                        sb.Append("\t");
                        sb.Append(bin.DataByPatient[key][0].PatientName);
                        sb.Append("\t");
                        sb.Append(bin.DataByPatient[key][0].MRN);
                        sb.Append("\t");

                        if (bin.DataByPatient[key][0].PatientSex == Sex.Male)
                            sb.Append("M");
                        else
                            sb.Append("F");
                        sb.Append("\t");

                        sb.Append(bin.DataByPatient[key][0].Age);
                        sb.Append("\t");

                        sb.Append(CountTransitions(bin.DataByPatient[key]));
                        sb.Append("\t");

                        foreach (DataPoint pt in bin.DataByPatient[key])
                        {
                            sb.Append(pt.SampleDate.ToShortDateString());
                            sb.Append("\t");

                            if (pt.CdiffResult == TestResult.Positive)
                                sb.Append("Pos");
                            else if (pt.CdiffResult == TestResult.Negative)
                                sb.Append("Neg");
                            else
                                sb.Append("UNK");

                            sb.Append("\t");
                        }
                        reportLines.Add(sb.ToString());
                    }
                }
            }

            return reportLines.ToArray();
        }


        public static String[] CreateTrackReportWithDaysBetweenSamples(Bin[] bins, bool printBinLabels)
        {
            List<string> reportLines = new List<string>();

            string topLine = "Patient ID\tPatient Name\tMRN\tSex\tAge\tTransitions\tSampleDate\tCdiff Result\t";
            reportLines.Add(topLine);

            foreach (Bin bin in bins)
            {
                if (printBinLabels)
                {
                    reportLines.Add(bin.Label);
                }
                foreach (string key in bin.DataByPatient.Keys)
                {
                    StringBuilder sb = new StringBuilder();

                    if (bin.DataByPatient.Count > 0)
                    {
                        sb.Append(bin.DataByPatient[key][0].SampleID);
                        sb.Append("\t");
                        sb.Append(bin.DataByPatient[key][0].PatientName);
                        sb.Append("\t");
                        sb.Append(bin.DataByPatient[key][0].MRN);
                        sb.Append("\t");

                        if (bin.DataByPatient[key][0].PatientSex == Sex.Male)
                            sb.Append("M");
                        else
                            sb.Append("F");
                        sb.Append("\t");

                        sb.Append(bin.DataByPatient[key][0].Age);
                        sb.Append("\t");

                        sb.Append(CountTransitions(bin.DataByPatient[key]));
                        sb.Append("\t");

                        for (int i = 0; i < bin.DataByPatient[key].Count; i++)
                        {
                            if (i == 0)
                            {
                                sb.Append("0");
                            }
                            else
                            {
                                sb.Append((bin.DataByPatient[key][i].SampleDate - bin.DataByPatient[key][i - 1].SampleDate).Days);
                            }
                            sb.Append("\t");

                            if (bin.DataByPatient[key][i].CdiffResult == TestResult.Positive)
                                sb.Append("Pos");
                            else if (bin.DataByPatient[key][i].CdiffResult == TestResult.Negative)
                                sb.Append("Neg");
                            else
                                sb.Append("UNK");

                            sb.Append("\t");
                        }
                        reportLines.Add(sb.ToString());
                    }
                }
            }

            return reportLines.ToArray();
        }

        private static int CountTransitions(List<DataPoint> patientData)
        {

            int tCount = 0;

            TestResult prev = patientData[0].CdiffResult;

            if(patientData.Count > 1)
            {
                for(int i = 1; i < patientData.Count; i++)
                {
                    if(patientData[i].CdiffResult != prev)
                    {
                        tCount++;
                    }
                    prev = patientData[i].CdiffResult;
                }
            }

            return tCount;
        }
    }
}

     

