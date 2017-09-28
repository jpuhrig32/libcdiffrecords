using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.Storage
{
    public class StorageData
    {
        public Dictionary<string, List<Tube>> TubesBySampleID { get; set; }
      //  public Dictionary<string, List<Tube>> TubesByPatient { get; set; }

        public StorageData()
        {
      //      TubesByPatient = new Dictionary<string, List<Tube>>();
            TubesBySampleID = new Dictionary<string, List<Tube>>();
        }
        
        public void Add(Tube t)
        {
            if(!TubesBySampleID.ContainsKey(t.SampleID))
                TubesBySampleID.Add(t.SampleID, new List<Tube>());
            TubesBySampleID[t.SampleID].Add(t);
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
    }

}
