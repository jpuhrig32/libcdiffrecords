using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using libcdiffrecords.Data;

namespace libcdiffrecords
{
    class Utilities
    {
        //Date regexes - stripping of invalid characters and formats.
        static Regex cdMonthFirst = new Regex("[A-Z][A-Z][A-Z]-[0-9][0-9]");
        static Regex cdMonthLast = new Regex("[0-9][0-9]-[A-Z][A-Z][A-Z]");
        static Regex shortDate = new Regex("[0-9]+/[0-9]+");
        static Regex longDate = new Regex("[0-9]+/[0-9]+/([0-9][0-9])+");
        static Regex strippingRegex = new Regex("[^0-9/]");
        static Regex numbersOnly = new Regex("[^0-9]");

        //Splitting arrays - for use in String.Split() function calls.
        static char[] tab = new char[] { '\t' };
        static char[] dash = new char[] { '-' };
        static char[] slash = new char[] { '/' };
        static char[] comma = new char[] { ',' };

        /// <summary>
        /// Old date-parsing function. Now just passes the call to the new one - ParseDate();
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime DateFromString(String date)
        {
            /*
            if(date.Equals("NA") || date == "")
            {
                return DateTime.MaxValue;
            }
            char[] commatest = new char[] { ',' };
            string[] test = date.Split(commatest);
            
            if (test.Length > 1)
            {
                date = test[0];
            }


            date = date.ToUpper();
            if(cdMonthLast.IsMatch(date))
            {
                char[] dashChars = new char[] { '-' };
                string[] parts = date.Split(dashChars);
                int dy = int.Parse(parts[0]);
                int mo = ParseMonthFromThreeLetterCode(parts[1].Trim());
                int yr = DateTime.Today.Year; //The assumption here, is that with the short date, it's the same year as today. However, if that is not possible, it was likely last year.
                DateTime ret = new DateTime(yr, mo, dy);
                if (ret > DateTime.Today)
                {
                    ret = new DateTime(yr - 1, mo, dy);
                }
                return ret;

            }
            if(cdMonthFirst.IsMatch(date))
            {
                char[] dashChars = new char[] { '-' };
                string[] parts = date.Split(dashChars);
                int dy = 1; //No day is given, so the assumption is that it is the first day of the month.
                int mo = ParseMonthFromThreeLetterCode(parts[0].Trim()); 
                int yr = int.Parse(parts[1].Trim());
                DateTime ret = new DateTime(yr, mo, dy);
                if (ret > DateTime.Today)
                {
                    ret = new DateTime(yr - 1, mo, dy);
                }
                return ret;
            }


            

            date = Regex.Replace(date, "[^0-9/]", "");
            if (date.Equals("NA") || date == "")
            {
                return DateTime.MaxValue;
            }
            char[] delimChars = new char[]{ '/' };

            string[] dateParts = date.Split(delimChars);
     
            int year = 1900;
            int month = 1;
            int day = 1;

            Regex rep = new Regex("[^0-9]");
            for (int i = 0; i < dateParts.Length; i++)
            {
                dateParts[i] = rep.Replace(dateParts[i], "");
            }

            if (dateParts.Length == 3)
            {
                month = int.Parse(dateParts[0]);
                 day = int.Parse(dateParts[1]);
                year = int.Parse(dateParts[2]);
            }
            else if (dateParts.Length == 2)
            {
               month = int.Parse(dateParts[0]);
                year = int.Parse(dateParts[1]);
            }
            else if (dateParts.Length == 1)
            {
                
                year = int.Parse(dateParts[0]);
            }

            
            

            if(day > 31)
            {
                day = 31;
            }
            if (ShortMonth(month) && day >= 30)
            {
                if (month == 2)
                {
                    day = 28;
                }
                else
                    day = 30;
            }

            return new DateTime(year, month, day);
            */

            return ParseDate(date);
        }

        /// <summary>
        /// Parses the date from a given string. Can currently handle dates in the following formats:
        /// Mon-yr
        /// dd-Mon
        /// m/d
        /// mm/dd
        /// mm/dd/yy
        /// mm/dd/yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ParseDate(string date)
        {
            date = date.Trim();
            if(Multidate(date)) //Detection of multi-date format, and just takes the first one.
            {
                string[] parts = date.Split(comma);
                if(parts.Length > 1)
                {
                    date = parts[0].Trim();
                }
            }
            //Determining the type of date this is, and calls the appropriate parsing sub-function.
            if (cdMonthFirst.IsMatch(date))
                return ParseMonthFirstDate(date);
            if (cdMonthLast.IsMatch(date))
                return ParseMonthLastDate(date);
            if (shortDate.IsMatch(date) || longDate.IsMatch(date))
                return ParseNumberSlashDate(date);

            return DateTime.MaxValue;
        }

        public static DateTime ParseDateTime(string date)
        {
            string[] parts = date.Split(' ');
            if(parts.Length >= 1)
            {
                return ParseDate(parts[0]);
            }
            return DateTime.MaxValue;
        }

        /// <summary>
        /// Parses a "month first" style date - a 3 letter month,
        /// followed by a two digit year. Assumes the first day of 
        /// the month is the day.
        /// </summary>
        /// <param name="date">The string containing the date</param>
        /// <returns>A date time representing the first day of the month, with the given three letter code, and the two number year</returns>
        private static DateTime ParseMonthFirstDate(string date)
        {
            string[] parts = date.Split(dash);
            if(parts.Length >= 2)
            {
                int month = ParseMonthFromThreeLetterCode(parts[0]);
                int year =  int.Parse(numbersOnly.Replace(parts[1], ""));
                year = TwoDigitYearToFourDigit(year);
                int day = 1;

                return new DateTime(year, month, day);
            }   
            return DateTime.MaxValue;
        }

        /// <summary>
        /// Parses out a DateTime object from a date of the format
        /// "Mar-23", where the 3 letters represent the month,
        /// the 2 numbers are the day, and the year is usually
        /// assumed to be this year (if not possible, then it
        /// is last year).
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static DateTime ParseMonthLastDate(string date)
        {
            string[] parts = date.Split(dash);
            if (parts.Length >= 2)
            {
                int day = int.Parse(numbersOnly.Replace(parts[0], ""));
                int month = ParseMonthFromThreeLetterCode(parts[1]);
                int year = DateTime.Today.Year;

                if(ShortMonth(month))
                {
                    if (month == 2 && day > 28)
                        day = 28;
                    if (month != 2 && day > 30)
                        day = 30;
                }
                DateTime test = new DateTime(DateTime.Today.Year, month, day);
                if (test > DateTime.Today)
                    year -= 1;

                return new DateTime(year, month, day);
            }

            return DateTime.MaxValue;
        }

        /// <summary>
        /// Parses dates of the formats:
        /// m/d
        /// mm/dd
        /// mm/dd/yy
        /// mm/dd/yyyy
        /// As long as month day and year are parsable as ints, and
        /// separated by slashes - this is the proper parser
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static DateTime ParseNumberSlashDate(string date)
        {
            date = strippingRegex.Replace(date, ""); //Remove all non numeric or slash characters
            string[] parts = date.Split(slash);

            if (parts.Length >= 2 && !IsNullString(parts[0]) && !IsNullString(parts[1]))
            {
                int day = DateTime.MaxValue.Day;
                int month = DateTime.MaxValue.Month;
                int year = DateTime.MaxValue.Year;

                if (parts.Length == 2) //Short date - month / day
                {
                    month = int.Parse(parts[0]);
                    day = int.Parse(parts[1]);
                    year = DateTime.Today.Year;

                    DateTime test = new DateTime(year, month, day);
                    if (test > DateTime.Today)
                        year -= 1;

                }
                if (parts.Length >= 3 && !IsNullString(parts[2]))
                {
                    int.TryParse(parts[0], out month);
                    int.TryParse(parts[1], out day);
                    int.TryParse(parts[2], out year);

                    if (day <= 0)
                        day = 1;
                    year = TwoDigitYearToFourDigit(year);
                }

                return new DateTime(year, month, day);               
            }

            return DateTime.MaxValue;
        }

        /// <summary>
        /// Is this a multi-date entry? Some of Tami's data contains dates in the format of 10/23,10/24.
        /// This determines if it is in this format.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static bool Multidate(string date)
        {
            char[] commatest = new char[] { ',' };
            string[] test = date.Split(commatest);

            return test.Length > 1;

        }

        /// <summary>
        /// Is it a month with 30 days or fewer?
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static bool ShortMonth(int m)
        {
            return (m == 2 || m == 4 || m == 6 || m == 9 || m == 11);
        }

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
