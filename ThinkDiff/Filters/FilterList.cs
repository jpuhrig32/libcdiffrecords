using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkDiff.Filters
{
    class FilterList
    { 
        public List<IFilter> Filters { get; set; }

       public FilterList()
        {
            Filters= new List<IFilter>();
            Filters.Add(new FilterForAdmissionsWithAdmissionSamples());
            Filters.Add(new FilterForIndexAdmission());



        }
    }
}
