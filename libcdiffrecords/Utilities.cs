using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using libcdiffrecords.Data;

namespace libcdiffrecords
{
   public  class Utilities
    {
   
        /// <summary>
        /// Parses the patient gender from a given string
        /// </summary>
        /// <param name="toParse"></param>
        /// <returns></returns>
        public static Sex ParseSexFromString(string toParse)
        {
            toParse = toParse.Trim();
            if (toParse.Equals("M"))
                return Sex.Male;
            if (toParse.Equals("F"))
                return Sex.Female;
            return Sex.Unknown;
        }

        /// <summary>
        /// Takes a 3 letter code: JAN, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, or DEC
        /// and converts to an integer month value. Used for other parsing functions for
        /// producing DateTime values.
        /// </summary>
        /// <param name="toParse"></param>
        /// <returns></returns>
        private static int ParseMonthFromThreeLetterCode(string toParse)
        {
            int ret = 0;

            switch(toParse.ToUpper().Trim())
            {
                case "JAN":
                    ret = 1;
                    break;
                case "FEB":
                    ret = 2;
                    break;
                case "MAR":
                    ret = 3;
                    break;
                case "APR":
                    ret = 4;
                    break;
                case "MAY":
                    ret = 5;
                    break;
                case "JUN":
                    ret = 6;
                    break;
                case "JUL":
                    ret = 7;
                    break;
                case "AUG":
                    ret = 8;
                    break;
                case "SEP":
                    ret = 9;
                    break;
                case "OCT":
                    ret = 10;
                    break;
                case "NOV":
                    ret = 11;
                    break;
                case "DEC":
                    ret = 12;
                    break;
                default:
                    ret = 0;
                    break;
           }


            return ret;
        }

        public static String PatientSexToString(Sex s)
        {
               switch (s)
            {
                case Sex.Female:
                    return "F";
                case Sex.Male:
                    return "M";
                default:
                    return "";
            }

        }

        /// <summary>
        /// Parses a CDiff culture result - Pos, Neg, or UNK from a string
        /// </summary>
        /// <param name="toParse"></param>
        /// <returns></returns>
        public static TestResult ParseTestResult(string toParse)
        {
            toParse = toParse.ToUpper();
            if (toParse.Contains("POS"))
                return TestResult.Positive;
            if (toParse.Contains("NEG"))
                return TestResult.Negative;
            if (toParse.Contains("NA"))
                return TestResult.NotTested;
            if (toParse.Contains("UNK"))
                return TestResult.Indeterminate;
            return TestResult.NotTested;
        }

        public static string TestResultToString(TestResult tr)
        {
            switch(tr)
            {
                case TestResult.Positive:
                    return "Pos";
                case TestResult.Negative:
                    return "Neg";
                case TestResult.Indeterminate:
                    return "UNK";
                default:
                    return "NA";
            }
        }

        /// <summary>
        /// Parses the test type from a given field
        /// </summary>
        /// <param name="parse">A string containing a clinical test type</param>
        /// <returns>A value of type TestType corresponding to that test</returns>
        public static TestType ParseTestTypeFromString(string parse)
        {
            parse = parse.ToUpper();
            switch(parse)
            {
                case "CLINICAL_OUTPATIENT_CULTURE":
                    return TestType.Clinical_Outpatient_Culture;
                case "SURVEILLANCE_STOOL_CULTURE":
                    return TestType.Surveillance_Stool_Culture;
                case "SURVEILLANCE_SWAB_CULTURE":
                    return TestType.Surveillance_Swab_Culture;
                case "SURVEILLANCE_STOOL_NAAT":
                    return TestType.Surveillance_Stool_NAAT;
                case "SURVEILLANCE_SWAB_NAAT":
                    return TestType.Surveillance_Swab_NAAT;
                case "CLINICAL_INPATIENT_NAAT":
                    return TestType.Clinical_Inpatient_NAAT;
                case "CLINICAL_OUTPATIENT_NAAT":
                    return TestType.Clinical_Outpatient_NAAT;
                default:
                    return TestType.No_Test;
            }
        }
        public static string TestTypeToString(TestType tt)
        {
            switch(tt)
            {
                case TestType.Clinical_Outpatient_Culture:
                    return "CLINICAL_OUTPATIENT_CULTURE";
                case TestType.Surveillance_Stool_Culture:
                    return "SURVEILLANCE_STOOL_CULTURE";
                case TestType.Surveillance_Swab_Culture:
                    return "SURVEILLANCE_SWAB_CULTURE";
                case TestType.Clinical_Inpatient_NAAT:
                    return "CLINICAL_INPATIENT_NAAT";
                case TestType.Surveillance_Stool_NAAT:
                    return "SURVEILLANCE_STOOL_NAAT";
                case TestType.Surveillance_Swab_NAAT:
                    return "SURVEILLANCE_SWAB_NAAT";
                case TestType.Clinical_Outpatient_NAAT:
                    return "CLINICAL_OUTPATIENT_NAAT";
                default:
                    return "";
            }
        }
        /// <summary>
        /// Is the string empty or equal to NA?
        /// </summary>
        /// <param name="check">The string to verify</param>
        /// <returns></returns>
        public static bool IsNullString(string check)
        {
           string test = check.ToUpper().Trim();
            return test == "" || test == "NA";
        }

        /// <summary>
        /// If the year is less than 100, AKA, a two digit year, converts to 4 digit by comparing to this year's 2 digits - if higher, assume 1900's 
        /// else, assume 2000's. 
        /// 
        /// Returns the same number back if the number is already a 4 digit year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static int TwoDigitYearToFourDigit(int year)
        {
            if(year < 100)
            {
                if (year <= DateTime.Today.Year - 2000)
                    return year + 2000;
                else
                    return year + 1900;
            }

            return year;
        }
    }
}
