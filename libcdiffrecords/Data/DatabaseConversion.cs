using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Storage;

namespace libcdiffrecords.Data
{
    public class DatabaseConversion
    {

        Dictionary<string, int> legacyLookupTable;
        List<DataPoint> data;

        public DatabaseConversion()
        {
            legacyLookupTable = new Dictionary<string, int>();
            data = new List<DataPoint>();
        }



        public void AddData(DataPoint[] dataPts)
        {
            int startIndex = data.Count;
            for(int i =0; i < dataPts.Length; i++)
            {
                if (dataPts[i].Tubes == null)
                    dataPts[i].Tubes = new List<Tube>();
                data.Add(dataPts[i]);
            }
          
            int endIndex = data.Count;

            for(int i = startIndex; i < endIndex; i++)
            {
                string legacyID = BuildLegacyID(data[i]);
                if(!legacyLookupTable.ContainsKey(legacyID))
                    legacyLookupTable.Add(BuildLegacyID(data[i]), i);
            }
            
        }

        private string BuildLegacyID(DataPoint dp)
        {
            return dp.LegacyID + "_" + dp.SampleDate.ToShortDateString();
        }

        public void AddStorageBox(StorageBox storageBox)
        {
            for(int i =0; i < storageBox.SampleTubes.Length; i++)
            {
                int sampleIndex = 0;
                string lkpKey = storageBox.SampleTubes[i].LegacyID + "_" + storageBox.SampleTubes[i].SampleDate.ToShortDateString();
                if(legacyLookupTable.ContainsKey(lkpKey))
                {
                    sampleIndex = legacyLookupTable[lkpKey];
                    data[sampleIndex].Tubes.Add(storageBox.SampleTubes[i]);
                }

            }
        }

      
        public DataPoint[] AttachData()
        {
            return data.ToArray();
        }


       
    }
}
