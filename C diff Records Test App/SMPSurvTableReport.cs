using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;
using libcdiffrecords.Reports;
using libcdiffrecords;

namespace C_diff_Records_Test_App
{
    public enum ReportType
    {
        Master,
        NAATAnalysis,
    };
    public class SMPSurvTableReport
    {

        public void GenerateReport(DataPoint[] survData, DataPoint[] naats, string outputPath)
        {
            outputPath = outputPath + "\\";
            Bin main = new Bin( "Surv", survData);
            main = DataFilter.RemovePatientsWithUnknownDOB(main);
            main = DataFilter.RemoveDataWithoutCDiffResult(main);
            //main = DataFilter.FilterFirstAdmissions(main);
            main = DataFilter.RemovePatientsWithPriorPosClinTest(main, naats);
            DatabaseFileIO.WriteDataToFile(main, outputPath + "MainBin.csv");

            Bin indexes = DataFilter.FilterIndexAdmissions(main);


            List<Bin> stratMain = new List<Bin>();
            stratMain.Add(main);
            stratMain.AddRange(DataFilter.StratifyOnCommonUnits(main));

            List<Bin> stratInd = new List<Bin>();
            stratInd.Add(indexes);
            stratInd.AddRange(DataFilter.StratifyOnCommonUnits(indexes));
            
            MasterReport mrMain = new MasterReport(stratMain.ToArray());
            MasterReport mrIndex = new MasterReport(stratInd.ToArray());

            mrMain.WriteReport(outputPath + "Main bin summary.csv");
            mrIndex.WriteReport(outputPath + "Index bin summary.csv");

            Bin[] mains = DataFilter.StratifyOnPatientFate(main);
            Bin[] indets = DataFilter.StratifyOnPatientFate(indexes);

            DatabaseFileIO.WriteDataToFile(mains[2], outputPath + "Main N to P.csv");
            DatabaseFileIO.WriteDataToFile(indets[2], outputPath + "Indexes N to P.csv");

            DataTap.data = new Bin("NAATData");
            NAATComparisonReport ncr = new NAATComparisonReport(stratInd.ToArray(), naats);
            ncr.WriteReport(outputPath + "Index NAAT Comparison Report.csv");
            DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "IndexNAAT Sample Data.csv");


            DataTap.data = new Bin("Main Bin");
            NAATComparisonReport mainNCR = new NAATComparisonReport(stratMain.ToArray(), naats);
            mainNCR.WriteReport(outputPath + "Main Bin NAAT Comparison Report.csv");
            DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "Main Bin NAAT Sample Data.csv");


            Bin cohort = DataFilter.FilterPatientsBasedOnCohort(main, DataTap.data.PatientMRNList);
            cohort = DataFilter.RemoveAdmissionsWithOneSample(cohort);


           
            

            /*
            main = DataFilter.RemovePatientsWithUnknownDOB(main);
            main = DataFilter.RemoveDataWithoutCDiffResult(main);
        //    main = DataFilter.FilterByTestType(main, TestType.Surveillance_Test);

            main.AssignAdmissionIDsToBin();
            Bin[] mainFates = DataFilter.StratifyOnPatientFate(main);
            DatabaseFileIO.WriteDataToFile(mainFates[2], outputPath + "Main Fates Neg to Pos.csv");


            CreateAndWriteReport(main, null, ReportType.Master, false, 0, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "Table 1 - Summary Statistics.csv");
            Bin indetsRmvd = DataFilter.RemoveAdmissionsWithNoAdmissionSample(main, 3);
            DatabaseFileIO.WriteDataToFile(indetsRmvd, outputPath + "indeterminates_Removed.csv", ',');

            Bin indexAdmissions = DataFilter.FilterIndexAdmissions(main);
            CreateAndWriteReport(indexAdmissions, null, ReportType.Master, false, 0, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "Summary Stats Indexes NAATS excluded from Index Picking.csv");
            Bin[] indexAdmitFates = DataFilter.StratifyOnPatientFate(indexAdmissions);
            DatabaseFileIO.WriteDataToFile(mainFates[2], outputPath + "Index Fates Neg to Pos.csv");

            DatabaseFileIO.WriteDataToFile(indexAdmissions, outputPath + "Index admits.csv", ',');

            Bin main2 = main.Clone();

            for (int i = 0; i < naats.Length; i++)
            {
                naats[i].AdmissionDate = naats[i].SampleDate;
                naats[i].Test = TestType.Clinical_Inpatient_NAAT;
            }

            main2 = AddNAATs(main2, naats);
            main2.AssignAdmissionIDsToBin();
            Bin indexAdmissions2 = DataFilter.FilterIndexAdmissions(main2);
            DatabaseFileIO.WriteDataToFile(indexAdmissions2, outputPath + "Index admits with NAATs.csv", ',');


        //    CreateAndWriteReport(indexAdmissions, null, ReportType.Master, false, 0, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "Summary Stats Indexes NAATS excluded from Index Picking.csv");
            
            CreateAndWriteReport(indexAdmissions2, null, ReportType.Master, false, 0, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "Summary Stats Indexes NAATS included Index Picking.csv");
            

            CreateAndWriteReport(indexAdmissions, naats, ReportType.NAATAnalysis, false, 90, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath+ "NAAT Analysis - NAATs excluded from index picking.csv");
            DataTap.data = new Bin("No NAATs In Index");
            CreateAndWriteReport(indexAdmissions, naats, ReportType.NAATAnalysis, false, 90, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "NAAT Analysis - NAATs Includedfrom index picking.csv");
           // DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "NAAT Analysis Bin - Surv Indexing Only.csv");      
           TabDelimWriter.WriteBinData(outputPath + "NAAT Analysis Bin - Surv Indexing Only.txt", DataTap.data);

            CreateAndWriteReport(indexAdmissions2, naats, ReportType.NAATAnalysis, false, 90, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "NAAT Analysis - NAATs excluded from index picking.csv");
            DataTap.data = new Bin("Clin NAATs In Index");
            CreateAndWriteReport(indexAdmissions2, naats, ReportType.NAATAnalysis, false, 90, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "NAAT Analysis - NAATs Includedfrom index picking.csv");
            TabDelimWriter.WriteBinData(outputPath + "NAAT Analysis Bin - Surv and ClinNAAT Indexing.txt", DataTap.data);
            //DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "NAAT Analysis Bin - Surv and ClinNAAT Indexing.csv");

            //Filtering by Test Type
            Bin cultureOnlyToxPosOnly = DataFilter.FilterByTestType(main2, TestType.Surveillance_Culture_Test);
            cultureOnlyToxPosOnly = DataFilter.FilterPositivesByToxinResult(cultureOnlyToxPosOnly, TestResult.Positive);


            cultureOnlyToxPosOnly = AddNAATs(cultureOnlyToxPosOnly, naats);
            cultureOnlyToxPosOnly = DataFilter.FilterIndexAdmissions(cultureOnlyToxPosOnly);
            Bin survNAATOnly = DataFilter.FilterByTestType(main2, TestType.Surveillance_NAAT_Test);
            survNAATOnly = AddNAATs(survNAATOnly, naats);
            survNAATOnly = DataFilter.FilterIndexAdmissions(survNAATOnly);

            CreateAndWriteReport(cultureOnlyToxPosOnly, null, ReportType.Master, false, 0, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "Summary Stats Indexes Culture Surv Tox Neg Excluded.csv");
            DataTap.data = new Bin("NAAT Analysis");
            CreateAndWriteReport(cultureOnlyToxPosOnly, naats, ReportType.NAATAnalysis, false, 90, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "NAAT Analysis Indexes Culture Surv Tox Neg Excluded.csv");
            TabDelimWriter.WriteBinData(outputPath + "NAAT Analysis Bin -Culture Surv Tox Neg Excluded.txt", DataTap.data);
            DatabaseFileIO.WriteDataToFile(cultureOnlyToxPosOnly, outputPath + "Index admits with NAATs cx surv tox pos only.csv", ',');

            CreateAndWriteReport(survNAATOnly, null, ReportType.Master, false, 0, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "Summary Stats Indexes NAAT Surv .csv");
            DataTap.data = new Bin("NAAT Analysis");
            CreateAndWriteReport(survNAATOnly, naats, ReportType.NAATAnalysis, false, 90, ComparisonType.ByEndResult, NAATCountingType.OncePerPatient, outputPath + "NAAT Analysis Indexes NAAT Surv Tox Neg Excluded.csv");
            TabDelimWriter.WriteBinData(outputPath + "NAAT Analysis Bin -NAAT Surv .txt", DataTap.data);
            DatabaseFileIO.WriteDataToFile(survNAATOnly, outputPath + "Index admits with NAATs naat-based surv only.csv", ',');

    */


        }

        private Bin AddNAATs(Bin b, DataPoint[] naat)
        {
            for(int i =0; i < naat.Length; i++)
            {

                if (naat[i].CdiffResult == TestResult.Positive && b.DataByPatientAdmissionTable.ContainsKey(naat[i].MRN))
                {

                    b.Add(naat[i]);
                }
            }
            return b;
        }


        private void CreateAndWriteReport(Bin report, DataPoint[] naat, ReportType type, bool ignoreIndeterminates, int naatWindow, ComparisonType ct, NAATCountingType nt, string outputFile)
        {
            Bin[] unitBins = DataFilter.StratifyOnCommonUnits(report, 50);

            List<Bin> bins = new List<Bin>();
            report.Label = "Global";
            bins.Add(report);
            bins.AddRange(unitBins);

            unitBins = bins.ToArray();

            IReportLine[] lines = new IReportLine[unitBins.Length];

            for(int i = 0; i < unitBins.Length; i++)
            {
                switch(type)
                {
                    case ReportType.Master:
                        lines[i] = new MasterReportLine(unitBins[i]);
                        break;
                    case ReportType.NAATAnalysis:
                        lines[i] = new NAATComparisonReportLine(unitBins[i], naat, naatWindow, ct, ignoreIndeterminates, nt);
                        break;

                }
            }
            ReportWriter.WriteReport(outputFile, lines);
        }


    }
}
