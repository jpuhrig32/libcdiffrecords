﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.Data
{
    /// <summary>
    /// Represents the data from a single line of the data
    /// in the surveillance file. This is a more "Flat" way 
    /// of representing the data.
    /// </summary>
    public class DataLine
    {
        string sampleID;
        string patientName;
        string mrn;
        Sex patientSex;
        DateTime dob;
        DateTime admDate;
        DateTime sampleDate;
        TestResult cdStatus;
        TestResult toxResult;
        string strain;
        string unit;
        string bed;
        Dictionary<string, object> fields = new Dictionary<string, object>();
   

        public void ParseDataLineFromOctFormat(string dataline, string[] header, int abxFieldsStart)
        {
         
            char[] tab = new char[] { '\t' };
            string[] parts = dataline.Split(tab);

            for(int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Trim();
            }
            if (parts.Length >= 12)
            {
                sampleID = parts[0];
                patientName = parts[1];
                mrn = parts[2].PadLeft(8, '0');
                patientSex = Utilities.ParseSexFromString(parts[3]);
                dob = DateTime.Parse(parts[4]);
                admDate = DateTime.Parse(parts[5]);
                sampleDate =DateTime.Parse(parts[6]);
                cdStatus = Utilities.ParseTestResult(parts[7]);
                toxResult = Utilities.ParseTestResult(parts[8]);
                strain = parts[9];
                unit = parts[10];
                bed = parts[11];
            }

            if(parts.Length > abxFieldsStart)
            {
                for(int i = 12; i < abxFieldsStart; i++)
                {
                    if(!fields.ContainsKey(header[i]))
                    {
                        fields.Add(header[i], parts[i]);
                    }   
                }
            }

        }
    }
}
