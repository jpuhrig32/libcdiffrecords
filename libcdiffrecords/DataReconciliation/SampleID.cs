using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.DataReconciliation
{
    /// <summary>
    /// Used as an identifier for a sample - it contains
    /// the sample's ID string (usually a 3-4 digit number, or F and 3 digits,
    /// along with the date the sample was taken.
    /// Comparisons are made based on the string value of the sample,
    /// and the date component of the sample date only.
    /// </summary>
    struct SampleID
    {
        DateTime date;
        string id;

        public SampleID(DateTime samDate, string samID)
        {
            date = samDate;
            id = samID;
        }

       public DateTime Date
        {
            get { return date; }
        }

        public string ID
        {
            get { return id; }
        }

        public override bool Equals(object obj)
        {
            if(obj is SampleID)
            {
                return (this.Date.Date == ((SampleID)obj).Date.Date) && (this.ID == ((SampleID)obj).ID);
            }
            return false;
        }


        public override int GetHashCode()
        {
            return (this.date.Date.GetHashCode() * 7) + id.GetHashCode();
        }

        public override string ToString()
        {
            return id + " " + date.ToShortDateString();
        }
    }
}

