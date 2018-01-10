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
    public partial class CDSDM_SurveillanceReportForm : Form
    {
        Bin repData;
        bool binInit = false;

        public CDSDM_SurveillanceReportForm()
        {
            InitializeComponent();
        }
        public void SetBin(Bin b)
        {
            repData = b;
            binInit = true;
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            if (!binInit)
            {
                MessageBox.Show("Bin data not set!");

            }
            else
            {
                repData = DataFilter.FilterByTestType(repData, TestType.Surveillance_Test);
                if (saveReportDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!ignoreDateRangeCB.Checked)
                    {
                        int sy = startDatePicker.Value.Year;
                        int sm = startDatePicker.Value.Month;
                        int sd = startDatePicker.Value.Day;
                        int ey = endDatePicker.Value.Year;
                        int em = endDatePicker.Value.Month;
                        int ed = endDatePicker.Value.Day;
                        DateTime start = new DateTime(sy, sm, sd);
                        DateTime end = new DateTime(ey, em, ed);
                        repData = DataFilter.FilterAdmissionsWithASampleInDateRange(repData, start, end);

                    }

                    SurveillanceReport sr = new SurveillanceReport(repData);
                    sr.WriteReport(saveReportDialog.FileName);
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
