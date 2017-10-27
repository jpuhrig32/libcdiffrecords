using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using libcdiffrecords.Data;

namespace libcdiffrecords.Storage
{
    public class StorageData
    {
        public Dictionary<string, List<Tube>> TubesBySampleID { get; set; }
        public Dictionary<string, List<Tube>> TubesByContainer { get; set; }
        public List<Tube> Tubes { get; set; }

      //  public Dictionary<string, List<Tube>> TubesByPatient { get; set; }

        public StorageData()
        {
      //      TubesByPatient = new Dictionary<string, List<Tube>>();
            TubesBySampleID = new Dictionary<string, List<Tube>>();
            TubesByContainer = new Dictionary<string, List<Tube>>();
            Tubes = new List<Tube>();
        }
        

        public StorageData(string storageDataFile)
        {
           StorageData sd = BoxLoader.LoadStorageData(storageDataFile);
            TubesByContainer = sd.TubesByContainer;
            TubesBySampleID = sd.TubesBySampleID;
            Tubes = sd.Tubes;
        }

        public void Add(Tube t)
        {
            if(!TubesBySampleID.ContainsKey(t.SampleID))
                TubesBySampleID.Add(t.SampleID, new List<Tube>());
            TubesBySampleID[t.SampleID].Add(t);

            if (!TubesByContainer.ContainsKey(t.ParentBox))
                TubesByContainer.Add(t.ParentBox, new List<Tube>());
            TubesByContainer[t.ParentBox].Add(t);

            Tubes.Add(t);
        }

        public void Add(StorageBox box)
        {
            for(int i = 0; i < box.SampleTubes.Count; i++)
            {
                Add(box.SampleTubes[i]);
            }
        }

        public void Add(StorageBox[] boxes)
        {
            for(int i = 0; i < boxes.Length; i++)
            {
                Add(boxes[i]);
            }
        }

        public string[] GetEmptyTubeSlots()
        {
            List<string> emptyPos = new List<string>();

            foreach (string key in TubesByContainer.Keys)
            {
                TubesByContainer[key].Sort((x, y) => x.BoxPosition.CompareTo(y.BoxPosition));

                bool[] empties = BuildEmptyTable(key);
                string prefix = "CDIF_";
                if (key.Contains("SBG"))
                    prefix = "SWAB_";
                for(int i = 0; i < empties.Length; i++)
                {
                    if(!empties[i])
                    {
                        emptyPos.Add(prefix + key.Substring(4) + "_" + (i + 1).ToString().PadLeft(2, '0'));  
                    }
                }
           

            }
            return emptyPos.ToArray();
        }

        public void AssignSampleIDsToTubes(Bin b)
        {
            Dictionary<string, List<Tube>> empties = new Dictionary<string, List<Tube>>();
            char[] split = new char[1] { ' ' };
            for(int i = 0; i < Tubes.Count; i++)
            {
                if(Tubes[i].SampleID.Equals("") && !Tubes[i].TubeLabel.Equals(""))
                {
                    string[] lparts = Tubes[i].TubeLabel.Split(split);

                    Tubes[i].SampleDate = DateTime.Parse(lparts[1].Trim());
                    lparts[0] = lparts[0].Trim();

                    string code = lparts[0].Substring(1);
                    code = code.PadLeft(4, '0');
                    lparts[0] = lparts[0][0] + code;

                    string legacyID = lparts[0];

                    if (!empties.ContainsKey(legacyID))
                        empties.Add(legacyID, new List<Tube>());

                    empties[legacyID].Add(Tubes[i]);
                }
            }

            for (int x = 0; x < b.Data.Count; x++)
            {
                if (empties.ContainsKey(b.Data[x].LegacyID))
                {
                    for (int z = 0; z < empties[b.Data[x].LegacyID].Count; z++)
                    {
                        if(empties[b.Data[x].LegacyID][z].SampleDate == b.Data[x].SampleDate)
                        {
                            empties[b.Data[x].LegacyID][z].SampleID = b.Data[x].SampleID;
                        }
                    }
                }
            }
        }


        public void WriteEmptyTubeList(string filename, char delim)
        {
            string[] empties = GetEmptyTubeSlots();

            StreamWriter sw = new StreamWriter(filename);

            for(int i = 0; i < empties.Length; i++)
            {
                sw.WriteLine(empties[i] + delim);
            }

            sw.Close();
        }

        private bool[] BuildEmptyTable(string key)
        {
            bool swabs = false;
            int max = 81;
            if (key.Contains("Box"))
                max = 81;
            else
            {
                max = 30;
                swabs = true;
            }
            bool[] empties = new bool[max];

            for (int i = 0; i < empties.Length; i++)
                empties[i] = false;
            if (!swabs)
            {
                for (int i = 0; i < TubesByContainer[key].Count; i++)
                {
                    empties[TubesByContainer[key][i].BoxPosition - 1] = true;
                }
            }
            else
            {
                for(int i = 0; i < TubesByContainer[key].Count; i++)
                {
                    empties[i] = true;
                }
            }

            return empties;
        }

        public void WriteStorageData(string filename)
        {
            WriteStorageData(filename, ',');
        }

        public void WriteStorageData(string filename, char delim)
        {
            BoxLoader.WriteStorageData(this, filename, delim);
        }
    }

}
