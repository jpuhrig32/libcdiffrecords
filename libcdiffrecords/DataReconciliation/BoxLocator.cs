using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace libcdiffrecords.DataReconciliation
{
    public class BoxLocator
    {

        Dictionary<SampleID, List<BoxLocation>> lkpTable = new Dictionary<SampleID, List<BoxLocation>>();

        /// <summary>
        /// Loads a box data file containing a 9x9 grid of sample IDs and dates.
        /// Enters the data into a lookup hashtable, using a list of boxlocations
        /// to allow for redundancy in box entries. 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="assumeYear"></param>
        public void LoadBoxData(string filename, int assumeYear)
        {
            if (File.Exists(filename))
            {
                StreamReader sr = new StreamReader(filename);

                int lineCount = 0;
                string line = "";
                char[] tab = new char[] { '\t' };
                char[] space = new char[] { ' ' };
                Box box = Box.Donskey;
                int boxNum = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    if (lineCount == 0)
                    {
                        string[] parts = line.Split(tab);
                        if (parts.Length >= 2)
                        {
                            string[] bxParts = parts[1].Trim().Split(space);
                            if (bxParts.Length >= 2)
                            {
                                if (bxParts[0].Contains("Donskey"))
                                    box = Box.Donskey;
                                if (bxParts[0].Contains("CD"))
                                    box = Box.CDiff;
                                boxNum = int.Parse(bxParts[1].Trim());
                            }
                        }
                    }
                    if (lineCount > 1 && lineCount <= 10)
                    {
                        string[] parts = line.Split(tab);
                        if (parts.Length > 1)
                        {
                            for (int x = 1; x < parts.Length; x++)
                            {
                                parts[x] = parts[x].Trim();
                                if (parts[x] != "NA" && parts[x] != "")
                                {
                                    string[] samid = parts[x].Split(space);
                                    if (samid.Length > 1)
                                    {
                                        if(lineCount == 4 && x == 7)
                                        {
                                            int ms = 1;
                                        }
                                        if (samid[1].Length <= 5)
                                        {
                                            samid[1] = samid[1] + "/" + assumeYear;
                                        }
                                        SampleID sam = new SampleID(Utilities.DateFromString(samid[1]), samid[0]);
                                        BoxLocation loc = new BoxLocation(x, lineCount - 1, box, boxNum);

                                        if (lkpTable.ContainsKey(sam))
                                        {
                                            lkpTable[sam].Add(loc);
                                        }
                                        else
                                        {
                                            lkpTable.Add(sam, new List<BoxLocation>());
                                            lkpTable[sam].Add(loc);
                                        }
                                    }
                                }

                            }
                        }

                    }

                    lineCount++;
                }

                sr.Close();
            }


        }



        /// <summary>
        /// Adds the currently loaded box data to the patient samples, from the lookup table of loaded box data
        /// </summary>
        /// <param name="patients">An array of patients to update their samples</param>
        /// <returns>The same patients, with additional location data</returns>
        public Patient[] AddBoxData(Patient[] patients)
        {
            
            for(int i = 0; i < patients.Length; i++)
            {
                for(int k = 0; k < patients[i].PatientSamples.Length; k++)
                {
                    SampleID sam = new SampleID(patients[i].PatientSamples[k].SampleDate, patients[i].PatientSamples[k].SampleID);
                    if (lkpTable.ContainsKey(sam))
                    {
                        patients[i].PatientSamples[k].SampleLocations.AddRange(lkpTable[sam]);
                        patients[i].PatientSamples[k].SampleLocations = DeduplicateBoxLocations(patients[i].PatientSamples[k].SampleLocations);
                    }
                }
            }

            return patients;
        }

        private List<BoxLocation> DeduplicateBoxLocations(List<BoxLocation> locs)
        {
            Dictionary<BoxLocation, bool> dict = new Dictionary<BoxLocation, bool>();
            List<BoxLocation> deduped = new List<BoxLocation>();

            for(int i = 0; i < locs.Count; i++)
            {
                if(!dict.ContainsKey(locs[i]))
                {
                    deduped.Add(locs[i]);
                    dict[locs[i]] = true;
                }
            }
            return deduped;
        }

        private void DeduplicateBoxDictionary()
        {
           foreach(List<BoxLocation> val in lkpTable.Values)
            {
              //  val = DeduplicateBoxLocations(val);
            }

        }
    }
}
