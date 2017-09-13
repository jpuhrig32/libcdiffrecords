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
            boxSampleCount = boxWidth * boxHeight;
            sampletubes = new Tube[width * height];
        }

        public Tube[] SampleTubes
        {
            get { return sampletubes; }
            set { sampletubes = value; }
        }

        public int Width
        {
            get { return boxWidth; }
        }

        public int Height
        {
            get { return boxHeight; }
        }

        public int SampleCount
        {
            get { return boxSampleCount; }
        }

        public string Name
        {
            get { return boxName; }
            set { boxName = value; }
        }

        public void AttachBoxLocationDataToTubes()
        {
            for(int i = 0; i < SampleTubes.Length; i++)
            {
                SampleTubes[i].TubeLocation = new BoxLocation(boxName, i / boxWidth, i % boxWidth);
            }
        }
    }

}
