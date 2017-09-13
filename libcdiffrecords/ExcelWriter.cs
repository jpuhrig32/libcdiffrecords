using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;
using System.Reflection;


namespace libcdiffrecords
{
    public class ExcelWriter
    {

        Application xlApp;
        _Workbook book;
        _Worksheet sheet;

        private void Initialize()
        {
            xlApp = new Application();
            xlApp.Visible = true;

            book = (_Workbook)(xlApp.Workbooks.Add(""));
            sheet = (_Worksheet)book.ActiveSheet;

        }

        public void WriteWeeklySurveillanceReport(SurveillanceReportLine[] lines)
        {
            Initialize();
            SetupSurveillanceFormatting(lines.Length +1);

            SurveillanceReportLine all = BuildSummationLine(lines);

            List<SurveillanceReportLine> lineList = new List<SurveillanceReportLine>();

            lineList.Add(all);
            lineList.AddRange(lines);

            int baseRow = 3;
             for(int i = 0; i < lineList.Count; i++)
             {
                sheet.Cells[baseRow + i, 1] = lineList[i].Label;
                sheet.Cells[baseRow + i, 2] = lineList[i].PatientAdmissionsCount;
                sheet.Cells[baseRow + i, 3] = lineList[i].PositiveSamples;
                sheet.Cells[baseRow + i, 4] = lineList[i].PositiveSamples / lineList[i].SampleCount * 100;
                sheet.Cells[baseRow + i, 5] = lineList[i].PositiveOnAdmission;
                sheet.Cells[baseRow + i, 6] = lineList[i].PositiveOnAdmission / lineList[i].SampleCount * 100;
                sheet.Cells[baseRow + i, 7] = lineList[i].PositiveDuringStay;
                sheet.Cells[baseRow + i, 8] = lineList[i].PositiveDuringStay / lineList[i].SampleCount * 100;
                sheet.Cells[baseRow + i, 9] = lineList[i].PositiveNoAdmissionSample;
                sheet.Cells[baseRow + i, 10] = lineList[i].PositiveNoAdmissionSample / lineList[i].SampleCount * 100;
            }


           
        }

        private SurveillanceReportLine BuildSummationLine(SurveillanceReportLine[] lines)
        {
            SurveillanceReportLine all = new SurveillanceReportLine("All Units");

            for(int i =0; i < lines.Length; i++)
            {
                all.PatientAdmissionsCount += lines[i].PatientAdmissionsCount;
                all.PatientCount += lines[i].PatientCount;
                all.PositiveDuringStay += lines[i].PositiveDuringStay;
                all.PositiveNoAdmissionSample += lines[i].PositiveNoAdmissionSample;
                all.PositiveOnAdmission += lines[i].PositiveOnAdmission;
                all.PositiveSamples += lines[i].PositiveSamples;
                all.SampleCount += lines[i].SampleCount;
            }

            return all;
        }
       private void SetupSurveillanceFormatting(int lineCount)
        {
            //Top Row
            sheet.Cells[1, 1] = "Unit";
            sheet.Cells[1, 2] = "Number of Stool Samples";
            sheet.Cells[1, 3] = "C. difficile positive (regardless of timing)";
            sheet.Cells[1, 5] = "C.difficile positive upon\n admission";
            sheet.Cells[1, 7] = "C. difficile initially negative and turned";
            sheet.Cells[1,9] = "C. difficile positive, no admission sample";

            //Merge Top Row Cells
            sheet.Cells[1,2].WrapText = true;

            sheet.Range["C1", "D1"].Merge();
            sheet.Range["C1", "D1"].WrapText = true;
            sheet.Range["E1", "F1"].Merge();
            sheet.Range["E1", "F1"].WrapText = true;
            sheet.Range["G1", "H1"].Merge();
            sheet.Range["G1", "H1"].WrapText = true;
            sheet.Range["I1", "J1"].Merge();
            sheet.Range["I1", "J1"].WrapText = true;

            sheet.Cells[2, 3] = "Number";
            sheet.Cells[2, 4] = "%";

            sheet.Cells[2, 5] = "Number";
            sheet.Cells[2, 6] = "%";

            sheet.Cells[2, 7] = "Number";
            sheet.Cells[2, 8] = "%";

            sheet.Cells[2, 9] = "Number";
            sheet.Cells[2, 10] = "%";

            sheet.Range["A1", "J2"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet.Range["A1", "J2"].Interior.Color = XlRgbColor.rgbLightGray;
            sheet.Range["A1", "J2"].Font.Underline = XlUnderlineStyle.xlUnderlineStyleSingle;
            sheet.Range["A1", "J3"].Font.Bold = true;
            sheet.Columns.ColumnWidth = 15;
            sheet.Range["A1", "J1"].RowHeight = 30;

            int maxIndex = 2 + lineCount;

            sheet.Range[sheet.Cells[1,1], sheet.Cells[maxIndex, 10]].Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            sheet.Range[sheet.Cells[1, 1], sheet.Cells[maxIndex, 10]].Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            sheet.Range[sheet.Cells[1, 1], sheet.Cells[maxIndex, 10]].Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            sheet.Range[sheet.Cells[1, 1], sheet.Cells[maxIndex, 10]].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            sheet.Range[sheet.Cells[1, 1], sheet.Cells[maxIndex, 10]].Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
            sheet.Range[sheet.Cells[1, 1], sheet.Cells[maxIndex, 10]].Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
            sheet.Range[sheet.Cells[1, 1], sheet.Cells[maxIndex, 10]].Borders.Color = XlRgbColor.rgbBlack;
        }


    }
}
