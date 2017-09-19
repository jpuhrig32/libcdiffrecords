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
    public partial class Form1 : Form
    {
        public Form1()
        {
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
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                dataGridView1.DataSource = AppData.BuildDataSetFromWorkingBin();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                dataGridView1.Show();
            }
        }
    }
}
