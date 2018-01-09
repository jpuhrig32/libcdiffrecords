using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.Data
{
    public class Admission
    {
        public int AdmissionWindow { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime DischargeDate { get; set; }
        private string mrn;
        public string MRN { get => mrn; set => mrn = value.PadLeft(8, '0'); }
        public List<DataPoint> Points { get; set; }
        public string PatientName { get; set; }

       


        public Admission()
        {
            AdmissionWindow = 3;
            AdmissionDate = DateTime.MaxValue;
            MRN = "00000000";
            Points = new List<DataPoint>();
            PatientName = "John Doe";
        }

        public bool AdmissionSamplePresent
        {
            get
            {
                return ((Points[0].SampleDate - AdmissionDate).Days <= AdmissionWindow);
            }
        }

        public AdmissionStatus AdmissionStatus
        {
            get
            {
                if(Points.Count > 0)
                {
                    bool admitSamplePresent = ((Points[0].SampleDate - AdmissionDate).Days <= AdmissionWindow);
                    if (Points[0].CdiffResult == TestResult.Positive)
                    {
                        if (admitSamplePresent)
                            return AdmissionStatus.PositiveOnAdmission;
                        else
                            return AdmissionStatus.PositiveNoAdmitSample;
                    }
                    else
                    {
                        
                        for(int i = 0; i < Points.Count; i++)
                        {
                            if(Points[i].CdiffResult == TestResult.Positive)
                            {
                                if (admitSamplePresent)
                                    return AdmissionStatus.NegativeOnAdmission_TurnedPositive;
                                else
                                    return AdmissionStatus.PositiveNoAdmitSample;
                            }
                        }
                        if (admitSamplePresent)
                            return AdmissionStatus.NegativeOnAdmission_RemainedNegative;
                        else
                            return AdmissionStatus.NegativeNoAdmissionSample;
                    }
                }
                return AdmissionStatus.EmptyAdmit;
            }
        }

        public string unit;


        public void SortData()
        {
            Points.Sort((x, y) => x.SampleDate.CompareTo(y.SampleDate));
        }

        public static Admission MergeAdmissions(Admission[] dpas)
        {
            Admission ret = new Admission();
            List<Admission> data = new List<Admission>();
            data.AddRange(dpas);

            data.Sort((x, y) => x.AdmissionDate.CompareTo(y.AdmissionDate));
            dpas = data.ToArray();

            for (int i = 0; i < dpas.Length; i++)
            {
                for (int j = 0; j < dpas[i].Points.Count; j++)
                {
                    DataPoint temp = new DataPoint();
                    temp = dpas[i].Points[j];
                    temp.AdmissionDate = dpas[0].AdmissionDate;
                    temp.Unit = dpas[0].Points[0].Unit;

                    ret.Points.Add(temp);
                }
            }
            ret.MRN = dpas[0].MRN;
            ret.AdmissionDate = dpas[0].AdmissionDate;
            ret.unit = dpas[0].unit;
            ret.SortData();

            return ret;
        }

    }

    public enum AdmissionStatus
    {
        PositiveOnAdmission,
        NegativeFirstSample_TurnedPositive,

        NegativeOnAdmission_RemainedNegative,
        NegativeOnAdmission_TurnedPositive,
        PositiveNoAdmitSample,
        NegativeNoAdmissionSample,
        EmptyAdmit,
        PositiveAdmission,
        NegativeOnAdmission,
        NegativeAdmission,
        IndeterminateAdmission,
    };


}
