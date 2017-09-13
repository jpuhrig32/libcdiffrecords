using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.Data
{
    public class BinSummaryStatistics
    {
        int empty = 0;
        int posOnAdmission = 0;
        int posNoAdmSample = 0;
        int negNoAdmSample = 0;
        int negOnAdmTurnedPos = 0;
        int negOnAdmRemainedNegative = 0;
        int twoOrMoreSamples = 0;
        int twoOrMoreAdmits = 0;
        

        public int EmptySamples { get => empty; set => empty = value; }
        public int PositiveOnAdmission { get => posOnAdmission; set => posOnAdmission = value; }
        public int Positive_NoAdmissionSample { get => posNoAdmSample; set => posNoAdmSample = value; }
        public int Negative_NoAdmissionSample { get => negNoAdmSample; set => negNoAdmSample = value; }
        public int Negative_TurnedPositive { get => negOnAdmTurnedPos; set => negOnAdmTurnedPos = value; }
        public int Negative_RemainedNegative { get => negOnAdmRemainedNegative; set => negOnAdmRemainedNegative = value; }
        public int AdmissionsWithTwoOrMoreSamples { get => twoOrMoreSamples; set => twoOrMoreSamples = value; }

        public int PatientsWithTwoOrMoreAdmits { get => twoOrMoreAdmits; set => twoOrMoreAdmits = value; }

        public int Positive_RegardlessOfTiming { get => posOnAdmission + posNoAdmSample + negOnAdmTurnedPos; }
    }
}
