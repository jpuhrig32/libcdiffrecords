using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;
using System.IO;

namespace libcdiffrecords.Storage
{
    struct TubeData
    {
        public int count;
        public DateTime date;
        public string id;
        public bool use;
    }

    class BoxLoader
    {
        DataPoint[] data;
        Dictionary<string, List<DataPoint>> sampleTable;
       const int width = 9;

        public BoxLoader(DataPoint[] dps)
        {
            data = dps;
            PopulateSampleTable();
        }

        private void PopulateSampleTable()
        {
            sampleTable = new Dictionary<string, List<DataPoint>>();
            for (int i =0; i < data.Length; i++)
            {
                if (!sampleTable.ContainsKey(data[i].SampleID))
                    sampleTable.Add(data[i].SampleID, new List<DataPoint>());

                sampleTable[data[i].SampleID].Add(data[i]);
            }
        }



        public static StorageBox LoadStorageBox(string filename)
        {
          
       
                StreamReader sr = new StreamReader(filename);
                char[] separator = new char[1];
                separator[0] = ',';
                string line = sr.ReadLine();
                if(line != null)
                {
                    string[] parts = line.Split(separator);
                    StorageBox box = new StorageBox(parts[1].Trim());
                    box.SampleTubes = EmptyBoxTubes(81);

                    switch (parts[3].Trim())
                    {
                        case "Short3":
                            box = LoadShort3BoxFile(sr, box);
                            break;
                        case "Short4":
                            box = LoadShort4BoxFile(sr, box);
                            break;
                        case "Long6":
                            box = LoadLong6BoxFile(sr, box);
                            break;
                        default:
                            return null;
                    }
                    box.AttachBoxLocationDataToTubes();
                    sr.Close();
                    return box;
                }

               
              
            

           

            return null;

        }

        private static Tube[] EmptyBoxTubes(int tubeCount)
        {
            Tube[] t = new Tube[tubeCount];
            for(int i =0; i < t.Length; i++)
            {
                t[i] = Tube.EmptyTube;
            }
            return t;
        }

        private static StorageBox LoadShort3BoxFile(StreamReader sr, StorageBox box)
        {
            DateTime glyDate = new DateTime(2017, 6, 28);
            char[] separator = new char[1];
            separator[0] = ',';
            int lineCount = 1;
            string line = "";
            while ((line = sr.ReadLine()) != null && lineCount <= 11)
            {
                string[] lineParts = line.Split(separator);

                if (lineCount > 1)
                {

                    TubeData[] td = new TubeData[3];

                    for (int i = 0; i < td.Length; i++)
                    {
                        int ind = (i * 3) + 1;
                        if (!lineParts[ind].Equals(""))
                        {
                            td[i].id = PadIdentifier(lineParts[ind].Trim());
                            td[i].date = Utilities.ParseDate(lineParts[ind + 1].Trim());
                            if (!int.TryParse(lineParts[ind + 2].Trim(), out td[i].count))
                                td[i].count = 3;
                            td[i].use = true;

                        }

                    }

                    for (int x = 0; x < td.Length; x++)
                    {
                        if (td[x].use)
                        {
                            for (int i = 0; i < td[x].count; i++)
                            {
                                int ind = (lineCount - 2) * width + (x * 3) + i; // Calculating the index - (lineCount -2) is the row, (x*3) is which block of 3 we're in, and i is the position in that block.

                                box.SampleTubes[ind].LegacyID = td[x].id;
                                box.SampleTubes[ind].SampleDate = td[x].date;
                                if (i < 2 && (td[x].date >= glyDate))
                                    box.SampleTubes[ind].Additives = TubeAdditive.Glycerol;

                            }
                        }
                    }

                }
                       
                lineCount++;
            }
            return box;
        }

        private static StorageBox LoadShort4BoxFile(StreamReader sr, StorageBox box)
        {
    
                DateTime glyDate = new DateTime(2017, 6, 28);
                char[] separator = new char[1];
                separator[0] = ',';
                int lineCount = 1;
                string line = "";
                while ((line = sr.ReadLine()) != null && lineCount <= 11)
                {
                    string[] lineParts = line.Split(separator);

                    if(lineCount > 1)
                    {

                    TubeData[] td = new TubeData[2];

                    for (int i = 0; i < td.Length; i++)
                    {
                        int ind = (i * 3) + 1;
                        if (!lineParts[ind].Equals(""))
                        {
                            td[i].id = PadIdentifier(lineParts[ind].Trim());
                            td[i].date = Utilities.ParseDate(lineParts[ind + 1].Trim());
                            if (!int.TryParse(lineParts[ind + 2].Trim(), out td[i].count))
                                td[i].count = 4;
                            td[i].use = true;

                        }

                    }
                    int[] inds = new int[4];

                    for(int x = 0; x < td.Length; x++)
                    {
                        if(td[x].use)
                        {
                            for(int i = 0; i < td[x].count; i++)
                            {
                                int ind;
                                if(lineCount < 11)
                                {
                                    ind = (lineCount - 2) * width + (x * 4) + i;
                                }
                                else
                                {
                                    ind = (((x*4)+i) * width) +8;
                                }
                                box.SampleTubes[ind].LegacyID = td[x].id;
                                box.SampleTubes[ind].SampleDate = td[x].date;
                                if (i < 2 && (td[x].date) >= glyDate)
                                    box.SampleTubes[ind].Additives = TubeAdditive.Glycerol;
                            }
                        }
                    }
                                           
                    }

                    lineCount++;
                }
                return box;
        }

        private static StorageBox LoadLong6BoxFile(StreamReader sr, StorageBox box)
        {
            DateTime glyDate = new DateTime(2017, 6, 28);
            char[] separator = new char[1];
            separator[0] = ',';
            int lineCount = 1;
            string line = "";
            while ((line = sr.ReadLine()) != null && lineCount <= 15)
            {
                if(lineCount > 1)
                {
                   string[] parts = line.Split(separator);
                    if(!parts[1].Equals(""))
                    {
                        string id = PadIdentifier(parts[1].Trim());
                        DateTime dt = Utilities.ParseDate(parts[2].Trim());
                        int count;
                        if(!int.TryParse(parts[3].Trim(), out count))
                        {
                            count = 6;
                        }

                        int[] indices = new int[6];
                        if(lineCount <= 10)
                        {
                            for (int i = 0; i < indices.Length; i++)
                                indices[i] = (lineCount - 2) * width + i; // Rows 1-9 are all horizontal
                        }
                       if(lineCount >= 11 && lineCount <= 13)
                        {
                            for (int i = 0; i < indices.Length; i++)
                                indices[i] = (i * width) + (lineCount - 5); //Columns 7, 8, and 9, or 6, 7, 8 in array numbering are vertical
                        }
                        if(lineCount == 14)
                        {
                            indices = new int[] { 60, 61, 62, 69, 70, 71};
                        }
                        for(int i =0; i < count; i++)
                        {
                            box.SampleTubes[indices[i]].LegacyID = id;
                            box.SampleTubes[indices[i]].SampleDate = dt;
                            if (i < 2 && (dt >= glyDate))
                                box.SampleTubes[indices[i]].Additives = TubeAdditive.Glycerol;

                        }
                    }
                }
                lineCount++;
            }
                return box;
        }

        private static string PadIdentifier(string id)
        {
            if (char.IsLetter(id[0]) && id[0] != 'F')
            {
                string idnum = id.Substring(1);
                idnum = idnum.PadLeft(4, '0');
                return (id[0] + idnum).ToUpper();
            }
            return id;
        }

        private DataPoint LookupSampleData(string paddedID, DateTime dt)
        {
            paddedID = paddedID.ToUpper();
            if(sampleTable.ContainsKey(paddedID))
            {
                for(int i = 0; i < sampleTable[paddedID].Count; i++)
                {
                    if (sampleTable[paddedID][i].SampleDate == dt)
                        return sampleTable[paddedID][i];

                }
            }

            return new DataPoint();
        }







       
    }
}
