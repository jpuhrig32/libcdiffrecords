using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using libcdiffrecords.Data;

namespace libcdiffrecords.Storage
{
    public class StorageBox
    {
        public int BoxSize { get; set; }
        

        public StorageBox(string name)
        {
            BoxSize = 81;
            SampleTubes = new List<Tube>();
            Name = name;
        }



      public List<Tube> SampleTubes { get; set; }

        public int SampleCount { get => SampleTubes.Count; }
   



       public string Name { get; set; }

       public int[] GetEmptyTubeSpots()
        {
            throw new NotImplementedException();
            List<int> empties = new List<int>();


            return empties.ToArray();
        }
    }

}
