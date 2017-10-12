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
using System.Data.SQLite;
using System.Data.Common;

namespace ThinkDiff
{
    public partial class ThinkDiffMainForm : Form
    {

        private Queue<IFilter> FilterQueue { get; set; }
        public ThinkDiffMainForm()
        {
            Settings.LoadSettings();
            FilterQueue = new Queue<IFilter>();
            AppData.Initialize();
            InitializeComponent();
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void loadDatafileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openDatabaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bin b = new Bin(openDatabaseFileDialog.SafeFileName, DatabaseFileIO.ReadDatabaseFile(openDatabaseFileDialog.FileName));
                AppData.AddNewData(b);
              //  AppData.WriteBinToDatabaseAsync(b); //We're assuming that this data is new, so we'll write it to the database.
                
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                statusLabel.Text = "Processing data...";
                //dataGridView1.DataSource = AppData.BuildDatasetFromWorkingBinAsync(this);
                FillDataGridViewDefault();
                DisplayBinContents();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                dataGridView1.Visible = true;
              

            }
        }
        
        private void DisplayBinContents()
        {
           statusLabel.Text = "Patient data loaded - " + AppData.WorkingBin.DataByPatientAdmissionTable.Count + " patients, " + AppData.WorkingBin.Data.Count + " samples in current dataset";
        }

        private void FillDataGridViewDefault()
        {

           List<DataPoint> pts = AppData.WorkingBin.Data;
            dataGridView1.DataSource = pts;

        }

        private void loadStorageBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openBoxFileDialog.ShowDialog() == DialogResult.OK)
            {
                if(openBoxFileDialog.FileNames.Length >= 1)
                {
                    AppData.LoadStorageData(openBoxFileDialog.FileName);
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void ThinkDiffMainForm_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            dataGridView1.Hide();
            dataGridView1.Show();
        }

        private void indexAdmissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterQueue.Enqueue(new Filters.FilterForIndexAdmission());
            StartFilterWorker();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (AppData.WorkingBin)
            {
                int ct = 0;
                int startFilterCount = FilterQueue.Count;
                while (FilterQueue.Count > 0)
                {
                    IFilter working = FilterQueue.Dequeue();
                    Bin b = working.FilterData(AppData.WorkingBin);
                    AppData.WorkingBin = b;
                    filterBackgroundWorker.ReportProgress(ct / startFilterCount);
                    ct++;
                }
            }
            

        }

        private void admissionsWithAdmissionSamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterQueue.Enqueue(new Filters.FilterForAdmissionsWithAdmissionSamples());
            StartFilterWorker();
        }

        private void StartFilterWorker()
        {
            filterBackgroundWorker.RunWorkerAsync();
            
        }

        private void filterBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FillDataGridViewDefault();
            DisplayBinContents();
        }

        private void exportPatientDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(exportBinDataDialog.ShowDialog()== DialogResult.OK)
            {
                DatabaseFileIO.WriteDataToFile(AppData.WorkingBin, exportBinDataDialog.FileName, ',');
            }
        }

        private void openDatabaseFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
