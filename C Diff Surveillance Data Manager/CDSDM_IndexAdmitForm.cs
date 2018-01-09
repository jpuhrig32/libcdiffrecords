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


namespace C_Diff_Surveillance_Data_Manager
{
    public partial class CDSDM_IndexAdmitForm : Form
    {
        Bin bin;
        bool binSet = false;

        public CDSDM_IndexAdmitForm()
        {
            InitializeComponent();
            
        }
        public void SetBin(Bin b)
        {
            bin = b;
            binSet = true;
        }

        private void exportDataButton_Click(object sender, EventArgs e)
        {
          if(!binSet)
            {
                MessageBox.Show("No Bin Data has been set");

            }
          else
            {
                Bin indexAdmits = DataFilter.FilterIndexAdmissions(bin);

                if(exportDataPointsRadioButton.Checked)
                {
                    if(saveDataPointsDialog.ShowDialog() == DialogResult.OK)
                    {
                        DatabaseFileIO.WriteDataToFile(indexAdmits, saveDataPointsDialog.FileName);
                    }
                }

                if(exportAdmDataRadioButton.Checked)
                {
                    if(saveAdmissionsDialog.ShowDialog() == DialogResult.OK)
                    {
                        DatabaseFileIO.WriteDatabaseAdmissions(indexAdmits, saveAdmissionsDialog.FileName);
                    }
                }
                if(exportBothRadioButton.Checked)
                {
                    if (saveDataPointsDialog.ShowDialog() == DialogResult.OK)
                    {
                        DatabaseFileIO.WriteDataToFile(indexAdmits, saveDataPointsDialog.FileName);
                    }
                    if (saveAdmissionsDialog.ShowDialog() == DialogResult.OK)
                    {
                        DatabaseFileIO.WriteDatabaseAdmissions(indexAdmits, saveAdmissionsDialog.FileName);
                    }
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
