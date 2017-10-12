using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThinkDiff
{
    public partial class AdvancedQueryDialog : Form
    {
        public string Query { get; set; }
        public DialogResult Result { get; set; }
        public AdvancedQueryDialog()
        {
            Result = DialogResult.None;
            Query = "";
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Query = textBox1.Text;
            this.Visible = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
