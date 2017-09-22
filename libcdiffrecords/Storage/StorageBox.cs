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
        int boxWidth = 9;
        int boxHeight = 9;
        string boxName = "";
        int boxSampleCount = 81;
        Tube[] sampletubes;

        public StorageBox(string name)
        {
            boxWidth = 9;
            boxHeight = 9;
            boxSampleCount = boxWidth * boxHeight;

            boxName = name;
        }

        public StorageBox(int width, int height, string name)
        {
            boxWidth = width;
            boxHeight = height;
            boxName = name;

            SampleTubes = new List<Tube>();
        }

      public List<Tube> SampleTubes { get; set; }

        public int SampleCount { get => SampleTubes.Count; }
   



       public string Name { get; set; }

        public void AttachBoxLocationDataToTubes()
        {
            for(int i = 0; i < SampleTubes.Count; i++)
            {
                SampleTubes[i].TubeLocation = new BoxLocation(boxName, i / boxWidth, i % boxWidth);
            }
        }
    }

}
