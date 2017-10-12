using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThinkDiff.Filters;

namespace ThinkDiff
{
    public partial class Form1 : Form
    {
        private Dictionary<string, IFilter> filterdict;
        List<string> left;
        List<string> right;
        public Form1()
        {
            filterdict = new Dictionary<string, IFilter>();
            PopulateFilterDict();
            left = new List<string>();
            foreach(string key in filterdict.Keys)
                left.Add(key);
            right = new List<string>();
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PopulateFilterDict()
        {
            filterdict.Add(FilterForAdmissionsWithAdmissionSamples.Name, new
                FilterForAdmissionsWithAdmissionSamples());
            filterdict.Add(FilterForIndexAdmission.Name, new FilterForIndexAdmission());


        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            foreach(int i in leftListBox.SelectedIndices)
            {

            }
        }
    }
}
