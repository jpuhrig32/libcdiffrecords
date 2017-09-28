using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace ThinkDiff.Filters
{
    class FilterForAdmissionsWithAdmissionSamples : IFilter
    {
        public Bin FilterData(Bin input)
        {
            return DataFilter.RemoveAdmissionsWithNoAdmissionSample(input, Settings.AdmissionWindow);
        }

        public Bin FilterUsingDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
