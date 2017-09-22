using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.Data
{
    public enum DataFlag
    {
        MissingAdmissionDate,
        MissingSampleDate,
        MissingUnit,
        MissingRoom,
        MissingDOB,
        MissingCdiffResult,
        MissingTestType,
        MissingMRN,
        MRNLongerThan8Digits_LikelyNonFMLH,
        DifferentNamesAttachedToMRN,
        DifferentSamplesAttachedToSampleID,
        TubeDoesNotHaveCorrespondingSampleRecord,
        
    };
}
