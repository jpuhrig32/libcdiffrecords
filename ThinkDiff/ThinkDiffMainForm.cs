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

namespace ThinkDiff
{
    public partial class ThinkDiffMainForm : Form
    {
        public ThinkDiffMainForm()
        {
            Settings.LoadSettings();
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
                AppData.WriteBinToDatabaseAsync(b); //We're assuming that this data is new, so we'll write it to the database.
                DisplayBinContents();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                statusLabel.Text = "Processing data...";
                dataGridView1.DataSource = AppData.BuildDatasetFromWorkingBinAsync(this);
                DisplayBinContents();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                dataGridView1.Show();
            }
        }
        
        private void DisplayBinContents()
        {
           statusLabel.Text = "Patient data loaded - " + AppData.WorkingBin.DataByPatientAdmissionTable.Count + " patients, " + AppData.WorkingBin.Data.Count + " samples in current dataset";
        }

        private void loadStorageBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openBoxFileDialog.ShowDialog() == DialogResult.OK)
            {
                if(openBoxFileDialog.FileNames.Length >= 1)
                {

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
    }
}
