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
using libcdiffrecords.Data;

namespace C_diff_Records_Test_App
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void genReportButton_Click(object sender, EventArgs e)
        {
            bool cont = true;
            if(survDataDialog.FileNames.Length < 1)
            {
                MessageBox.Show("Please select a surveillance data file");
                cont = false;
            }
            if(naatDataDialog.FileNames.Length < 1)
            {
                MessageBox.Show("Please select a NAAT data file");
                cont = false;
            }
            if(reportSaveDialog.SelectedPath.Equals(""))
            {
                MessageBox.Show("Please select a location to save reports to");
                cont = false;
            }

            if(cont)
            {
                TabLoader tl = new TabLoader();
                DataPoint[] survData = DatabaseFileIO.ReadDatabaseFile(survDataDialog.FileName);
                DataPoint[] naatData = tl.LoadNAATResults(naatDataDialog.FileName);
                SMPSurvTableReport rep = new SMPSurvTableReport();
                rep.GenerateReport(survData, naatData, reportSaveDialog.SelectedPath);

            }
        }

        private void survDataBrowse_Click(object sender, EventArgs e)
        {
            if(survDataDialog.ShowDialog() == DialogResult.OK)
            {
                survDataText.Text = survDataDialog.FileName;
            }
        }

        private void naatDataBrowse_Click(object sender, EventArgs e)
        {
            if(naatDataDialog.ShowDialog() == DialogResult.OK)
            {
                naatDataText.Text = naatDataDialog.FileName;
            }
        }

        private void saveToBrowse_Click(object sender, EventArgs e)
        {
            if(reportSaveDialog.ShowDialog() == DialogResult.OK)
            {
                saveToText.Text = reportSaveDialog.SelectedPath;
            }
        }
    }
}
