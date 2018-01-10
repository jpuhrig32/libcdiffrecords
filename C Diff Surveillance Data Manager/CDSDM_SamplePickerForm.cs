using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_Diff_Surveillance_Data_Manager
{
    public partial class CDSDM_SamplePickerForm : Form
    {
        public DialogResult Result { get; set; }
        public string[] AccessionNumbers { get; set; }
        public CDSDM_SamplePickerForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            AccessionNumbers = GetAccessionNumbers();
            this.Hide();
            DialogResult = DialogResult.OK;
        }


        private string[] GetAccessionNumbers()
        {
            List<string> accession = new List<string>();

            char[] splitChar = new char[5] { ' ', ',', '\n', '\t', '\r'};
            string[] acc = samplePickerTextBox.Text.Split(splitChar);

            for(int i = 0; i < acc.Length; i++)
            {
                if (acc[i].Length >= 10)
                    accession.Add(acc[i]);
            }

            return accession.ToArray();

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            DialogResult = DialogResult.Cancel;
        }
    }
}
