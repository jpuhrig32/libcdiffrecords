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

namespace C_Diff_Surveillance_Data_Manager
{
    public partial class CDSDM_SummaryReportForm : Form
    {
        Bin bin;
        bool binInit = false;

        public CDSDM_SummaryReportForm()
        {
            InitializeComponent();
           
        }

        public void SetBin(Bin b)
        {
            bin = b;
            binInit = true;
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            if(!binInit)
            {
                MessageBox.Show("Bin Data not Set!");
            }
            else
            {
                if (saveReportDialog.ShowDialog() == DialogResult.OK)
                {
                    bin.Label = "All Units";
                    List<Bin> lineBins = new List<Bin>();
                    lineBins.Add(bin);
                    if (RBIncludeAll.Checked)
                    {
                        lineBins.AddRange(DataFilter.StratifyOnUnits(bin));
                    }
                    if (RBCommonOnly.Checked)
                    {
                        lineBins.AddRange(DataFilter.StratifyOnCommonUnits(bin));
                    }
                    SummaryReport mr = new SummaryReport(lineBins.ToArray());
                    mr.WriteReport(saveReportDialog.FileName);
                }
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
