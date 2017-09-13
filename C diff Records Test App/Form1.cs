using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libcdiffrecords;
using libcdiffrecords.DataReconciliation;
using libcdiffrecords.Reports;


using libcdiffrecords.Data;
using libcdiffrecords.Storage;


namespace C_diff_Records_Test_App
{
    public enum AdmType
    {
        PosOnAdm,
        StayedNeg,
        BecamePositive,
    };
    public partial class Form1 : Form
    {
      
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = openInputFileDialog.ShowDialog();
            if ( dr == DialogResult.OK)
            {
                inputFilePathTextBox.Text = openInputFileDialog.FileName;
            }
        }

        private void loadFileButton_Click(object sender, EventArgs e)
        {
            if(openInputFileDialog.FileNames.Length > 0)
            {
                DataPoint[] data = DatabaseFileIO.ReadDatabaseFile(openInputFileDialog.FileName);

                Bin b = new Bin("Global", data);
                List<Bin> bins = new List<Bin>();
                
                b = StratificationAnalysis.FilterByTestType(b, TestType.Surveillance_Test);
                Bin[] unitBins = StratificationAnalysis.StratifyOnUnits(b);
                bins.Add(b);
                bins.AddRange(unitBins);

                IReportLine[] lines = new IReportLine[bins.Count];

                for(int i = 0; i < bins.Count; i++)
                {
                    lines[i] = new MasterReportLine(bins[i]);
                }

                ReportWriter.WriteReport(openInputFileDialog.FileName + "Report.csv", ReportFormat.CSV, lines);

                


                

                int z = 0;
                //TabLoader tl = new TabLoader();
               /// DataPoint[] data = tl.LoadPatientDataToPoints(openInputFileDialog.FileName);
                
//                SampleDatabase sd = new SampleDatabase();

                //sd.ConvertDatabase(openInputFileDialog.FileName, openBoxLocFileDialog.FileNames, openInputFileDialog.FileName + "new");
              

            }
            else
            {
                MessageBox.Show("Please choose patient data to load.");
            }
        }

        private void recFileBrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
              recFileTextBox.Text = saveFileDialog1.FileName;
            }
        }

        private List<DataPointAdmission> GetAdmissionsFromBins(Bin[] bins)
        {

            List<DataPointAdmission> dpa = new List<DataPointAdmission>();

            foreach(Bin b in bins)
            {
                foreach(string key in b.DataByPatientAdmissionTable.Keys)
                {
                    foreach(DataPointAdmission dp in b.DataByPatientAdmissionTable[key])
                    {
                        dpa.Add(dp);
                    }
                }
            }
            return dpa;
        }

        private void boxLocBrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = openBoxLocFileDialog.ShowDialog();
            if(dr == DialogResult.OK)
            {
                boxLocTextBox.Text = openBoxLocFileDialog.FileName;
            }
            
        }
        private int CountPositiveOnAdmissions(List<DataPointAdmission> dpa, int window)
        {
            int ct = 0;

            for (int i = 0; i < dpa.Count; i++)
            {
                if (dpa[i].points[0].CdiffResult ==TestResult.Positive && (dpa[i].points[0].SampleDate - dpa[i].admissionDate).Days <= window)
                    ct++;
            }
            return ct;
        }

        private int CountPositiveWithNoAdmissionSample(List<DataPointAdmission> dpa, int window)
        {
            int ct = 0;

            foreach (DataPointAdmission dp in dpa)
            {
                if (dp.points[0].CdiffResult == TestResult.Positive && (dp.points[0].SampleDate - dp.admissionDate).Days > window)
                    ct++;
                else
                {
                    if(dp.points.Count > 1 && (dp.points[0].SampleDate - dp.admissionDate).Days > window)
                    {
                        for(int i =1; i < dp.points.Count; i++)
                        {
                            if (dp.points[i].CdiffResult ==TestResult.Positive)
                                ct++;
                        }
                    }
                }
            }

            return ct;
        }
     

        private void button1_Click(object sender, EventArgs e)
        {
          

        }

        private void recFileTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}
