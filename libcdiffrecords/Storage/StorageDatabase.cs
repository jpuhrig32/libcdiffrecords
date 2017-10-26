using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace libcdiffrecords.Storage
{
    public class StorageData
    {
        public Dictionary<string, List<Tube>> TubesBySampleID { get; set; }
        public Dictionary<string, List<Tube>> TubesByContainer { get; set; }

      //  public Dictionary<string, List<Tube>> TubesByPatient { get; set; }

        public StorageData()
        {
      //      TubesByPatient = new Dictionary<string, List<Tube>>();
            TubesBySampleID = new Dictionary<string, List<Tube>>();
            TubesByContainer = new Dictionary<string, List<Tube>>();
        }
        
        public void Add(Tube t)
        {
            if(!TubesBySampleID.ContainsKey(t.SampleID))
                TubesBySampleID.Add(t.SampleID, new List<Tube>());
            TubesBySampleID[t.SampleID].Add(t);

            if (!TubesByContainer.ContainsKey(t.ParentBox))
                TubesByContainer.Add(t.ParentBox, new List<Tube>());
            TubesByContainer[t.ParentBox].Add(t);
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
    }

}
