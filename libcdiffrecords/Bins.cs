using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords
{
    public class Bin
    {
        string label;
        List<DataPoint> data;
        Dictionary<string, List<DataPoint>> patients;
        Dictionary<string, List<DataPointAdmission>> admissionsByPatient;
        public int min = 0;
        public int max = 1;

        public int ItemsInBin
        {
            get { return data.Count; }
        }

        public int PatientCount
        {
            get { return patients.Count; }
        }

        public string Label
        {
            get { return label; }
        }
        public int PatientAdmissionCount
        {
            get
            {
                int ct = 0;
                foreach (string key in admissionsByPatient.Keys)
                {
                    ct += admissionsByPatient[key].Count;
                }
                return ct;

            }
        }

        public List<DataPoint> Data
        {
            get { return data; }
        }

        public Dictionary<string, List<DataPoint>> DataByPatient
        {
            get { return patients; }
        }

        public Dictionary<string, List<DataPointAdmission>> DataByPatientAdmissions
            {
                get { return admissionsByPatient; }
            }



        public Bin(string binLabel)
        {
            data = new List<libcdiffrecords.DataPoint>();
            patients = new Dictionary<string, List<DataPoint>>();
            admissionsByPatient = new Dictionary<string, List<libcdiffrecords.DataPointAdmission>>();
            label = binLabel;
        }

        public Bin(string binLabel, int bmin, int bmax)
        {
            data = new List<libcdiffrecords.DataPoint>();
            patients = new Dictionary<string, List<DataPoint>>();
            admissionsByPatient = new Dictionary<string, List<libcdiffrecords.DataPointAdmission>>();
            label = binLabel;
            min = bmin;
            max = bmax;

        }

        public void Add(DataPoint point)
        {
            data.Add(point);

            if (!patients.ContainsKey(point.mrn))
            {
                patients.Add(point.mrn, new List<DataPoint>());
            }
            patients[point.mrn].Add(point);

            if (!admissionsByPatient.ContainsKey(point.mrn))
            {
                admissionsByPatient.Add(point.mrn, new List<DataPointAdmission>());

                DataPointAdmission dpa = new DataPointAdmission();
                dpa.admissionDate = point.admDate;
                dpa.MRN = point.mrn;
                dpa.unit = point.unit.Trim();
                dpa.points.Add(point);

                admissionsByPatient[point.mrn].Add(dpa);

            }
            else
            {

                bool found = false;
                for(int i = 0; i < admissionsByPatient[point.mrn].Count; i++)
                {
                        if(admissionsByPatient[point.mrn][i].admissionDate == point.admDate && admissionsByPatient[point.mrn][i].unit == point.unit)
                    {
                        found = true;
                        admissionsByPatient[point.mrn][i].points.Add(point);
                    }
                    
                }
                if(!found)
                {
                    DataPointAdmission dpa = new DataPointAdmission();
                    dpa.admissionDate = point.admDate;
                    dpa.MRN = point.mrn;
                    dpa.unit = point.unit.Trim();
                    dpa.points.Add(point);
                    admissionsByPatient[point.mrn].Add(dpa);
                }
            }
            
        }
        public void SortBinData()
        {
            data.Sort((x,y) => x.sampleDate.CompareTo(y.sampleDate));
            foreach(string key in patients.Keys)
            {
                patients[key].Sort((x,y) => x.sampleDate.CompareTo(y.sampleDate));
            }

            foreach(string key in admissionsByPatient.Keys)
            {
                admissionsByPatient[key].Sort((x,y) => x.admissionDate.CompareTo(y.admissionDate));

                for(int i = 0; i < admissionsByPatient[key].Count; i++)
                {
                    admissionsByPatient[key][i].SortData();
                }

            }
        }

        public bool TryGetDataPointAdmission(DataPoint dp, out DataPointAdmission result)
        {
            if(admissionsByPatient.ContainsKey(dp.mrn))
            {
                foreach(DataPointAdmission dpa in admissionsByPatient[dp.mrn])
                {
                    if (dpa.admissionDate == dp.admDate && dpa.unit == dp.unit)
                    {
                        result = dpa;
                        return true;
                    }
                }

            }
            result = null;
            return false;
        }
    }

   

   public class DataPointAdmission
    {
        public DateTime admissionDate;
        public string MRN;
        public List<DataPoint> points = new List<DataPoint>();

        public string unit;


        public void SortData()
        {
            points.Sort((x, y) => x.sampleDate.CompareTo(y.sampleDate));
        }
    }
}
