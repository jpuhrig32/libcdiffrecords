using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;

namespace ThinkDiff.Filters
{
    class FilterForIndexAdmission : IFilter
    {
        public string DatabaseQuery { get; set; }
        public int Priority { get; set; }
        public const string Name = "Pick Index Admissions"; 

        public Bin FilterData(Bin input)
        {
            return DataFilter.FilterIndexAdmissions(input);
        }

        /// <summary>
        /// TODO - Implement this.
        /// Right now - not really something I want to write a query for.
        /// </summary>
        /// <returns></returns>
        public Bin FilterUsingDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
