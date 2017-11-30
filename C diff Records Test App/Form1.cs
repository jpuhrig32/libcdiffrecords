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
using System.IO;


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
            if (openInputFileDialog.FileNames.Length > 0)
            {
                Application.DoEvents();
                Bin data = DatabaseFileIO.ReadDatabaseFileToBin(openInputFileDialog.FileName);
                data.Label = "Global";
                StorageData sd = BoxLoader.LoadStorageData(openBoxLocFileDialog.FileName);

                data = DataFilter.RemoveAdmissionsWithNoAdmissionSample(data, 3);
                TestType[] types = new TestType[3] { TestType.Clinical_Inpatient_NAAT, TestType.Surveillance_Stool_NAAT, TestType.Surveillance_Swab_NAAT };
                data = DataFilter.FilterByTestType(data, types);
                DatabaseFileIO.WriteDatabaseAdmissions(data, openInputFileDialog.FileName + "_admissions.csv");

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

        private List<Admission> GetAdmissionsFromBins(Bin[] bins)
        {

            List<Admission> dpa = new List<Admission>();

            foreach(Bin b in bins)
            {
                foreach(string key in b.DataByPatientAdmissionTable.Keys)
                {
                    foreach(Admission dp in b.DataByPatientAdmissionTable[key])
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
        private int CountPositiveOnAdmissions(List<Admission> dpa, int window)
        {
            int ct = 0;

            for (int i = 0; i < dpa.Count; i++)
            {
                if (dpa[i].Points[0].CdiffResult ==TestResult.Positive && (dpa[i].Points[0].SampleDate - dpa[i].AdmissionDate).Days <= window)
                    ct++;
            }
            return ct;
        }

        private int CountPositiveWithNoAdmissionSample(List<Admission> dpa, int window)
        {
            int ct = 0;

            foreach (Admission dp in dpa)
            {
                if (dp.Points[0].CdiffResult == TestResult.Positive && (dp.Points[0].SampleDate - dp.AdmissionDate).Days > window)
                    ct++;
                else
                {
                    if(dp.Points.Count > 1 && (dp.Points[0].SampleDate - dp.AdmissionDate).Days > window)
                    {
                        for(int i =1; i < dp.Points.Count; i++)
                        {
                            if (dp.Points[i].CdiffResult ==TestResult.Positive)
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

        private void boxLocTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
