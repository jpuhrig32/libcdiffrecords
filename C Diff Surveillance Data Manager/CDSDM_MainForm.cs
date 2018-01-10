using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libcdiffrecords.Data;
using libcdiffrecords.Reports;
using libcdiffrecords.Storage;

namespace C_Diff_Surveillance_Data_Manager
{
    public partial class CDSDM_MainForm : Form
    {
        private Bin mainBin;
        private StorageData sd;
        bool binInit;
        bool storageInit;

        public CDSDM_MainForm()
        {
            mainBin = new Bin("Data");
            sd = new StorageData();
            binInit = false;
            storageInit = false;
            InitializeComponent();
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            if(binInit)
            {
                int sampleCount = mainBin.Data.Count;
                int ptCount = mainBin.DataByPatientAdmissionTable.Keys.Count;
                int admCt = mainBin.PatientAdmissionCount;
                databaseStatusLabel.Text = "C Diff Database Status: " + ptCount + " patients, " + admCt + " admissions, " + sampleCount + " samples loaded";
           
            }
            else
            {
                databaseStatusLabel.Text = "C Diff Database Status: Not Loaded";
            }
            if(storageInit)
            {
                int samCt = sd.TubesBySampleID.Keys.Count;
                int tubeCount = sd.Tubes.Count;
                storageStatusLabel.Text = "Storage Database Status: " + samCt + " samples, " + tubeCount + " sample locations loaded";
            }
            else
            {
                storageStatusLabel.Text = "Storage Database Status: Not Loaded";
            }
        }

        private void loadStorageDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openStorageFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sd = BoxLoader.LoadStorageData(openStorageFileDialog.FileName);
                    storageInit = true;
                    UpdateLabels();
                }
                catch (Exception exe)
                {
                    MessageBox.Show("An error occurred while loading storage data\n " + exe.Message);
                }
            }
        }

        private void loadDatabaseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openDatabaseDialog.ShowDialog() == DialogResult.OK)
            {
               
                    DataPoint[] dps = DatabaseFileIO.ReadDatabaseFile(openDatabaseDialog.FileName);
                    mainBin = new Bin("Data", dps);
                    binInit = true;
                    UpdateLabels();
               
                /*
                catch (Exception exe)
                {
                    MessageBox.Show("An error occurred while loading sample data\n " + exe.Message);
                }
                */
            }
        }

        private void exportAdmDataButton_Click(object sender, EventArgs e)
        {
            if (CheckBin())
            {
                if(saveAdmissionsDataFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DatabaseFileIO.WriteDatabaseAdmissions(mainBin, saveAdmissionsDataFileDialog.FileName);
                }
            }
        }

        private void pickIndexAdmissionButton_Click(object sender, EventArgs e)
        {
            if (CheckBin())
            {
                CDSDM_IndexAdmitForm indexForm = new CDSDM_IndexAdmitForm();
                indexForm.SetBin(mainBin);
                indexForm.Show();
                
            }
        }

        private void masterReportButton_Click(object sender, EventArgs e)
        {
           if(CheckBin())
            {
                CDSDM_SummaryReportForm mrForm = new CDSDM_SummaryReportForm();
                mrForm.SetBin(mainBin);
                mrForm.Show();
            }
        }

        private bool CheckBin()
        {
            if (!binInit)
            {
                MessageBox.Show("No C. Diff Data has been loaded yet");
            }
            return binInit;
        }

        private bool CheckStorage()
        {
            if (!storageInit)
            {
                MessageBox.Show("No Sample Storage Data has been loaded yet");
            }
            return storageInit;
        }

        private void surveillanceReportButton_Click(object sender, EventArgs e)
        {
            if(CheckBin())
            {
                CDSDM_SurveillanceReportForm survForm = new CDSDM_SurveillanceReportForm();
                survForm.SetBin(mainBin);
                survForm.Show();
            }
        }
    }
}
