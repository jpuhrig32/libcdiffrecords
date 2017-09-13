using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.Data
{
    public class DataPointAdmission
    {
        public int admissionWindow = 3;
        public DateTime admissionDate;
        public string MRN;
        public List<DataPoint> points = new List<DataPoint>();

        public AdmissionStatus AdmissionStatus
        {
            get
            {
                if(points.Count > 0)
                {
                    bool admitSamplePresent = ((points[0].SampleDate - admissionDate).Days <= admissionWindow);
                    if (points[0].CdiffResult == TestResult.Positive)
                    {
                        if (admitSamplePresent)
                            return AdmissionStatus.PositiveOnAdmission;
                        else
                            return AdmissionStatus.PositiveNoAdmitSample;
                    }
                    else
                    {
                        
                        for(int i = 0; i < points.Count; i++)
                        {
                            if(points[i].CdiffResult == TestResult.Positive)
                            {
                                if (admitSamplePresent)
                                    return AdmissionStatus.NegativeOnAdmission_TurnedPositive;
                                else if(i > 0)
                                    return AdmissionStatus.NegativeFirstSample_TurnedPositive;
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
            points.Sort((x, y) => x.SampleDate.CompareTo(y.SampleDate));
        }

        public static DataPointAdmission MergeAdmissions(DataPointAdmission[] dpas)
        {
            DataPointAdmission ret = new DataPointAdmission();
            List<DataPointAdmission> data = new List<DataPointAdmission>();
            data.AddRange(dpas);

            data.Sort((x, y) => x.admissionDate.CompareTo(y.admissionDate));
            dpas = data.ToArray();

            for (int i = 0; i < dpas.Length; i++)
            {
                for (int j = 0; j < dpas[i].points.Count; j++)
                {
                    DataPoint temp = new DataPoint();
                    temp = dpas[i].points[j];
                    temp.AdmissionDate = dpas[0].admissionDate;
                    temp.Unit = dpas[0].points[0].Unit;

                    ret.points.Add(temp);
                }
            }
            ret.MRN = dpas[0].MRN;
            ret.admissionDate = dpas[0].admissionDate;
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
