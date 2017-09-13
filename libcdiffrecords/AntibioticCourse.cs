using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords
{
    public enum Route
    { Oral, IV, IP, IM, Catheter, Irrigation, JT, HHN, NJ, OG, NG, None, Unknown, Other};

    public enum SortMode
    { TreatmentStart, TreatmentEnd, Drug, Frequency,};

    public class AntibioticCourse:IComparable
    {
        DateTime start;
        DateTime end;
        Drug drug;
        double frequency;
        Route roa;

        string dose;
        bool ended;
        int doseCount;
        SortMode sortBy = SortMode.TreatmentStart;
        bool prn = false;

        /// <summary>
        /// Gets or sets the Treatment Start date
        /// Additionally, sets the TreatmentEnded Flag
        /// depending on the relationship of these dates
        /// </summary>
        public DateTime TreatmentStart
        {
           get { return start; }
           set {
                    start = value;
                if ((end >= start) && (end <= DateTime.Today))
                    ended = true;
                else
                    ended = false;


            }
        }

        /// <summary>
        /// Gets or sets the Treatment End Date
        /// Additionally, sets the TreatmentEnded flag,
        /// depending on the relationship of start to end
        /// </summary>
        public DateTime TreatmentEnd
        {
          get { return end; }
          set {
                end = value;
                if ((end >= start) && (end <= DateTime.Today))
                    ended = true;
                else
                    ended = false;
              }
        }

        /// <summary>
        /// Is the drug an As Needed (usually denoted by PRN in a record).
        /// If so, we can likely ignore the frequency value.
        /// </summary>
        public bool AsNeeded
        {
            get { return prn; }
            set { prn = value; }
        }

        /// <summary>
        /// Gets or sets the antibiotic associated with this treatment.
        /// Please use AntibioticManager.GetAntibiotic(string) to
        /// get the actual Antibiotic object.
        /// </summary>
        public Drug Drug
        {
          get { return drug; }
          set { drug = value;}
        }

        public SortMode SortBy
        {
            get { return sortBy; }
            set { sortBy = value; }
        }

        /// <summary>
        /// Gets or sets the treatment frequency
        /// Frequency of treatment, expressed in treatments / day
        /// 
        /// 6hr = 4, 8 hr = 3, 12hr = 2, Daily = 1, and Once every MWF. is 3/7 (or 0.4)
        /// </summary>
        public double TreatmentFrequency
        {
            get { return frequency; }
            set {
                frequency = value;
                }
        }

        /// <summary>
        /// Gets or sets the Route of administration
        /// </summary>
        public Route RouteOfAdministration
        {
            get { return roa; }
            set { roa = value; }
        }
        public string DoseInMG
        {
            get { return dose; }
            set { dose = value; }
        }

        /// <summary>
        /// Gets a boolean indicating whether or not the treatment has ended. 
        /// This value is set whenever the treatment start or end dates change
        /// </summary>
        public bool TreatmentEnded
        {
            get { return ended; }
        }

        public int DoseCount
        {
            get { return doseCount; }
            set { doseCount = value; } 
        }


        /// <summary>
        /// Creates a default antibiotic treatment
        /// Treatment date is 1/1/2000,
        /// Drug is the "None" object
        /// Frequency and dose count is 1 and 0
        /// Ended = true;
        /// </summary>
        public AntibioticCourse()
        {
            start = new DateTime(2000, 1, 1);
            end = new DateTime(2000, 1, 1);
            drug = AntibioticManager.GetAntibiotic("None");
            frequency = 1;
            roa = Route.None;
            dose = "";
            ended = true;
            doseCount = 0;
        }

        public int CompareTo(object obj)
        {

            switch(sortBy)
            {
                case SortMode.Drug:
                    return drug.CompareTo(((AntibioticCourse)obj).drug);
                case SortMode.Frequency:
                    return frequency.CompareTo(((AntibioticCourse)obj).frequency);                 
                case SortMode.TreatmentEnd:
                    return TreatmentEnd.CompareTo(((AntibioticCourse)obj).TreatmentEnd);                
                case SortMode.TreatmentStart:
                    return TreatmentStart.CompareTo(((AntibioticCourse)obj).TreatmentStart);                
                default:
                    return TreatmentStart.CompareTo(((AntibioticCourse)obj).TreatmentStart);         
            }
           
        }
    }
}
