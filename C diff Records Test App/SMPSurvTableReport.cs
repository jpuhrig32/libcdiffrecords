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
            
            main = DataFilter.RemovePatientsWithNoSurveillanceSamples(main);
            DatabaseFileIO.WriteDataToFile(main, outputPath + "mainBin prior to exclusions.csv");
            List<Bin> sr01Bins = new List<Bin>();
            sr01Bins.Add(main);
            sr01Bins.AddRange(DataFilter.StratifyOnCommonUnits(main));
            SummaryReport sr01 = new SummaryReport(sr01Bins.ToArray());
            sr01.WriteReport(outputPath + " mainBin prior to exclusions summary report.csv");


            Bin main2 = main.Clone();

            main = DataFilter.RemovePatientsWithPriorPosClinTest(main, naats);
            DatabaseFileIO.WriteDataToFile(main, outputPath + "mainBin after exclusions.csv");
            Bin excludedPatients = main2 - main;
            DatabaseFileIO.WriteDataToFile(excludedPatients, outputPath + "excluded_patients.csv");

           
            Bin indexes = DataFilter.FilterIndexAdmissions(main);
            DatabaseFileIO.WriteDataToFile(indexes, outputPath + "IndexBin.csv");
            DatabaseFileIO.WriteDatabaseAdmissions(indexes, outputPath + "IndexBinAdmissions.csv");

            List<Bin> sr02Bins = new List<Bin>();
            sr02Bins.Add(indexes);
            sr02Bins.AddRange(DataFilter.StratifyOnCommonUnits(indexes));
            SummaryReport sr02 = new SummaryReport(sr02Bins.ToArray());
            sr02.WriteReport(outputPath + " Index Bin summary report.csv");


            Bin[] splitInd = DataFilter.SplitOnNAATDate(indexes, naats, new DateTime(2017, 2, 1), 90);
            DatabaseFileIO.WriteDataToFile(splitInd[0], outputPath + "Index Bin Prior to Reflex.csv");
            DatabaseFileIO.WriteDataToFile(splitInd[1], outputPath + "Index Bin After Reflex.csv");

            List<Bin> stratInd = new List<Bin>();
            stratInd.Add(splitInd[0]);
            stratInd.AddRange(DataFilter.StratifyOnCommonUnits(splitInd[0]));
            SummaryReport sr03 = new SummaryReport(stratInd.ToArray());
            sr03.WriteReport(outputPath + "Summary Report Index Before Reflex.csv");

            List<Bin> strat2 = new List<Bin>();
            stratInd.Add(splitInd[1]);
            Bin[] splitInd1Bins = DataFilter.StratifyOnCommonUnits(splitInd[1]);

            stratInd.AddRange(splitInd1Bins);
            strat2.Add(splitInd[1]);
            strat2.AddRange(splitInd1Bins);
            SummaryReport sr04 = new SummaryReport(strat2.ToArray());
            sr04.WriteReport(outputPath + "Summary Report Index After Reflex.csv");


            List<Bin> stratInd2 = new List<Bin>();
            stratInd2.Add(indexes);
            stratInd2.AddRange(DataFilter.StratifyOnCommonUnits(indexes));

            DataTap.data = new Bin("NAATData");
            NAATComparisonReport ncr = new NAATComparisonReport(stratInd.ToArray(), naats, NAATComparisonReportType.DayRange);
            ncr.WriteReport(outputPath + "Index NAAT Comparison Report.csv");
            DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "IndexNAAT Sample Data.csv");

            DataTap.data = new Bin("NAATData");
            NAATComparisonReport ncr2 = new NAATComparisonReport(stratInd2.ToArray(), naats, NAATComparisonReportType.DayRange);
            ncr2.WriteReport(outputPath + "Index NAAT Comparison Report - no splitting.csv");
            DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "IndexNAAT Sample Data no splitting.csv");

            DataTap.data = new Bin("IndexData");
            NAATComparisonReport ncr3 = new NAATComparisonReport(stratInd2.ToArray(), naats, NAATComparisonReportType.ByAdmission);
            ncr3.WriteReport(outputPath + "Index NAAT Comparison Report - byAdmission.csv");
            DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "IndexNAAT Sample Data naat compared by admission.csv");

            DataTap.data = new Bin("IndexData");
            NAATComparisonReport ncr4 = new NAATComparisonReport(stratInd.ToArray(), naats, NAATComparisonReportType.ByAdmission);
            ncr4.WriteReport(outputPath + "Index NAAT Comparison Report - byAdmission and split.csv");
            DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "IndexNAAT Sample Data naat compared by admission and split.csv");

            /*
            //main = DataFilter.FilterFirstAdmissions(main);
            //main = DataFilter.RemovePatientsWithPriorPosClinTest(main, naats);
            main = DataFilter.RemovePatientsWithNoSurveillanceSamples(main);
            DatabaseFileIO.WriteDataToFile(main, outputPath + "MainBin.csv");

            Bin indexes = DataFilter.FilterIndexAdmissions(main);
            DatabaseFileIO.WriteDataToFile(indexes, outputPath + "IndexBin.csv");
            DatabaseFileIO.WriteDatabaseAdmissions(indexes, outputPath + "IndexBinAdmissions.csv");

            Bin[] mainSplit = DataFilter.SplitOnNAATDate(main, naats, new DateTime(2017, 2, 1), 90);
            List<Bin> stratMain = new List<Bin>();
            stratMain.Add(main);
            stratMain.AddRange(DataFilter.StratifyOnCommonUnits(mainSplit[0]));
            stratMain.AddRange(DataFilter.StratifyOnCommonUnits(mainSplit[1]));


            Bin[] splitInd = DataFilter.SplitOnNAATDate(indexes, naats, new DateTime(2017, 2, 1), 90);


            List<Bin> stratInd = new List<Bin>();
            stratInd.Add(indexes);
            stratInd.AddRange(DataFilter.StratifyOnCommonUnits(splitInd[0]));
            stratInd.AddRange(DataFilter.StratifyOnCommonUnits(splitInd[1]));

            List<Bin> stratInd2 = new List<Bin>();
            stratInd2.Add(indexes);
            stratInd2.AddRange(DataFilter.StratifyOnCommonUnits(indexes));
     

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

            DataTap.data = new Bin("NAATData");
            NAATComparisonReport ncr2 = new NAATComparisonReport(stratInd2.ToArray(), naats);
            ncr2.WriteReport(outputPath + "Index NAAT Comparison Report - no splitting.csv");
            DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "IndexNAAT Sample Data no splitting.csv");


            DataTap.data = new Bin("Main Bin");
            NAATComparisonReport mainNCR = new NAATComparisonReport(stratMain.ToArray(), naats);
            mainNCR.WriteReport(outputPath + "Main Bin NAAT Comparison Report.csv");
            DatabaseFileIO.WriteDataToFile(DataTap.data, outputPath + "Main Bin NAAT Sample Data.csv");


            Bin cohort = DataFilter.FilterPatientsBasedOnCohort(main, DataTap.data.PatientMRNList);
            cohort = DataFilter.RemoveAdmissionsWithOneSample(cohort);
            */





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


        private void CreateAndWriteReport(Bin report, DataPoint[] naat, ReportType type, NAATComparisonReportType ncrtype, bool ignoreIndeterminates, int naatWindow, ComparisonType ct, NAATCountingType nt, string outputFile)
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
                        lines[i] = new SummaryReportLine(unitBins[i]);
                        break;
                    case ReportType.NAATAnalysis:
                        lines[i] = new NAATComparisonReportLine(unitBins[i], naat, naatWindow, ncrtype, ct, ignoreIndeterminates, nt);
                        break;

                }
            }
            ReportWriter.WriteReport(outputFile, lines);
        }


    }
}
