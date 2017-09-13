using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace libcdiffrecords
{
    public class SurveillanceReportLine
    {

        string label;
        int numberSamples;
        int numberPatients;
        int numberPositive;
        int numPosOnAdm;
        int numPosAfterStay;
        int numPosNoAdm;
        int numAdmissions;

        float percentPos;
        float percentPosAdm;
        float percentPosStay;
        float percentPosNoAdm;

        public String Label
        {
            get { return label; }
        }

        public int SampleCount
        {
            get { return numberSamples; }
            set { numberSamples = value; }
        }

        public int PatientCount
        {
            get { return numberPatients; }
            set { numberPatients = value; }
        }

        public int PositiveSamples
        {
            get { return numberPositive; }
            set { numberPositive = value; }
        }

        public int PositiveOnAdmission
        {
            get { return numPosOnAdm; }
            set { numPosOnAdm = value; }
        }

        public int PositiveDuringStay
        {
            get { return numPosAfterStay; }
            set { numPosAfterStay = value; }
        }

        public int PositiveNoAdmissionSample
        {
            get { return numPosNoAdm; }
            set { numPosNoAdm = value; }
        }

        public int PatientAdmissionsCount
        {
            get { return numAdmissions; }
            set { numAdmissions = value; }
        }

        public float PercentPositive
        {
            get { UpdatePercentages(); return percentPos; }
        }

        public float PercentPositiveOnAdmission
        {
            get { UpdatePercentages(); return percentPosAdm; }
        }

        public float PercentPositiveDuringStay
        {
            get { UpdatePercentages(); return percentPosStay; }
        }

        public float PercentPositiveNoAdmission
        {
            get { UpdatePercentages(); return percentPosNoAdm; }
        }

  




        public SurveillanceReportLine(string reportLabel)
        {
            label = reportLabel;
            numberSamples = 0;
            numberPositive = 0;
            numPosOnAdm = 0;
            numPosAfterStay = 0;
            numPosNoAdm = 0;
            numberPatients = 0;
            numAdmissions = 0;


            percentPos = 0.00f;
            percentPosAdm = 0.00f;
            percentPosStay = 0.00f;
            percentPosNoAdm = 0.00f;
        }

        public void UpdatePercentages()
        {
            if (numberSamples != 0)
            {
                percentPos = (float)(numberPositive / numberSamples * 100);
                percentPosAdm = (float)(numPosOnAdm / numberSamples * 100);
                percentPosStay = (float)(numPosAfterStay / numberSamples * 100);
                percentPosNoAdm = (float)(numPosNoAdm / numberSamples * 100);
            }
        }

    }

}
