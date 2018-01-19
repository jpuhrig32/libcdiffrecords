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


        public CDSDM_MainForm()
        {
            DataManager.StartingBin = new Bin("Data");
            DataManager.WorkingBin= new Bin("Data");
            DataManager.Storage= new StorageData();
            DataManager.BinInit= false;
            DataManager.StorageInit = false;
            InitializeComponent();
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            if(DataManager.BinInit)
            {
                int sampleCount = DataManager.WorkingBin.Data.Count;
                int ptCount = DataManager.WorkingBin.DataByPatientAdmissionTable.Keys.Count;
                int admCt = DataManager.WorkingBin.PatientAdmissionCount;
                databaseStatusLabel.Text = "C Diff Database Status: " + ptCount + " patients, " + admCt + " admissions, " + sampleCount + " samples loaded";
           
            }
            else
            {
                databaseStatusLabel.Text = "C Diff Database Status: Not Loaded";
            }
            if(DataManager.StorageInit)
            {
                int samCt = DataManager.Storage.TubesBySampleID.Keys.Count;
                int tubeCount = DataManager.Storage.Tubes.Count;
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
                    DataManager.Storage= BoxLoader.LoadStorageData(openStorageFileDialog.FileName);
                    DataManager.StorageInit = true;
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
                    DataManager.StartingBin = new Bin("Data", dps);
                    DataManager.WorkingBin= new Bin("Data", dps);
                    DataManager.BinInit= true;
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
                    DatabaseFileIO.WriteDatabaseAdmissions(DataManager.WorkingBin, saveAdmissionsDataFileDialog.FileName);
                }
            }
        }

        private void pickIndexAdmissionButton_Click(object sender, EventArgs e)
        {
            if (CheckBin())
            {
                DataManager.WorkingBin = DataFilter.FilterIndexAdmissions(DataManager.WorkingBin);
                UpdateLabels();
            }
        }

        private void masterReportButton_Click(object sender, EventArgs e)
        {
           if(CheckBin())
            {
                CDSDM_SummaryReportForm mrForm = new CDSDM_SummaryReportForm();
                mrForm.SetBin(DataManager.WorkingBin);
                mrForm.Show();
            }
        }

        private bool CheckBin()
        {
            if (!DataManager.BinInit)
            {
                MessageBox.Show("No C. Diff Data has been loaded yet");
            }
            return DataManager.BinInit;
        }

        private bool CheckStorage()
        {
            if (!DataManager.StorageInit)
            {
                MessageBox.Show("No Sample Storage Data has been loaded yet");
            }
            return DataManager.StorageInit;
        }

        private void surveillanceReportButton_Click(object sender, EventArgs e)
        {
            if(CheckBin())
            {
                CDSDM_SurveillanceReportForm survForm = new CDSDM_SurveillanceReportForm();
                survForm.SetBin(DataManager.WorkingBin);
                survForm.Show();
            }
        }

        private void filterSelectedButton_Click(object sender, EventArgs e)
        {
            if(CheckBin())
            {
                CDSDM_SamplePickerForm spf = new CDSDM_SamplePickerForm();
                if(spf.ShowDialog() == DialogResult.OK)
                {
                    DataManager.WorkingBin = DataFilter.FilterBySampleID(DataManager.WorkingBin, spf.AccessionNumbers);
                    UpdateLabels();
                }
            }
        }

        private void filterStoredButton_Click(object sender, EventArgs e)
        {
            if(CheckBin() && CheckStorage())
            {
                DataManager.WorkingBin = DataFilter.FilterAvailableSamples(DataManager.WorkingBin, DataManager.Storage);
                UpdateLabels();
            }
        }

        private void exportCurrentDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(CheckBin())
            {
                if(saveCurrentDatabaseDialog.ShowDialog() == DialogResult.OK)
                {
                    DatabaseFileIO.WriteDataToFile(DataManager.WorkingBin, saveCurrentDatabaseDialog.FileName);
                }
            }
        }

        private void resetDatasetToOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(CheckBin())
            {
                DataManager.WorkingBin = DataManager.StartingBin;
                UpdateLabels();
            }
        }

        private void sampleFinderAllButton_Click(object sender, EventArgs e)
        {
            if(CheckBin() && CheckStorage())
            {
                if(saveStorageDialog.ShowDialog() == DialogResult.OK)
                {
                    Tube[] selected = DataFilter.FilterAllAvailableSampleTubesFromBin(DataManager.WorkingBin, DataManager.Storage);
                    BoxLoader.WriteTubeDataToFile(selected, saveStorageDialog.FileName, ',');
                }
            }
        }

        private void sampleFinderByIDButton_Click(object sender, EventArgs e)
        {
            if (CheckStorage())
            {
                CDSDM_SamplePickerForm spf = new CDSDM_SamplePickerForm();
                if(spf.ShowDialog() == DialogResult.OK)
                {
                    if(spf.AccessionNumbers.Length <= 0)
                    {
                        MessageBox.Show("Sorry - no accession numbers were found!");
                    }
                    else if (saveStorageDialog.ShowDialog() == DialogResult.OK)
                    {
                        Tube[] selected = DataFilter.FilterSampleTubesOnAccessionList(DataManager.Storage, spf.AccessionNumbers);
                        if (selected.Length == 0)
                        {
                            MessageBox.Show("Sorry - No samples with those IDs were found!");
                        }
                        else
                        {
                            BoxLoader.WriteTubeDataToFile(selected, saveStorageDialog.FileName, ',');
                        }
                    }
                }
              
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openDatabaseDialog.ShowDialog() == DialogResult.OK)
            {

                DataPoint[] dps = DatabaseFileIO.ReadDatabaseFile(openDatabaseDialog.FileName);
                DataManager.StartingBin = new Bin("Data", dps);
                DataManager.WorkingBin = new Bin("Data", dps);
                DataManager.BinInit = true;
                UpdateLabels();

                /*
                catch (Exception exe)
                {
                    MessageBox.Show("An error occurred while loading sample data\n " + exe.Message);
                }
                */
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openStorageFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataManager.Storage = BoxLoader.LoadStorageData(openStorageFileDialog.FileName);
                    DataManager.StorageInit = true;
                    UpdateLabels();
                }
                catch (Exception exe)
                {
                    MessageBox.Show("An error occurred while loading storage data\n " + exe.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (CheckBin())
            {
                if (saveCurrentDatabaseDialog.ShowDialog() == DialogResult.OK)
                {
                    DatabaseFileIO.WriteDataToFile(DataManager.WorkingBin, saveCurrentDatabaseDialog.FileName);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (CheckBin())
            {
                DataManager.WorkingBin = DataManager.StartingBin;
                UpdateLabels();
            }
        }
    }
}
