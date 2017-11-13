using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libcdiffrecords.Data;
using System.IO;

namespace libcdiffrecords.Storage
{
    public enum BoxWritingStyle
    {
        AllBoxesToOneFile,
        OneBoxPerFile,
    };
    struct TubeData
    {
        public int count;
        public DateTime date;
        public string id;
        public bool use;
    }

    public class BoxLoader
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
            for (int i = 0; i < data.Length; i++)
            {
                if (!sampleTable.ContainsKey(data[i].SampleID))
                    sampleTable.Add(data[i].SampleID, new List<DataPoint>());

                sampleTable[data[i].SampleID].Add(data[i]);
            }
        }


        public static StorageData LoadStorageData(string filename)
        {
            StorageData sd = new StorageData();
            StreamReader sr = new StreamReader(filename) ;
            char[] separator = new char[1] { ',' };
            string line = sr.ReadLine();

            while((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(separator);
                Tube t = new Tube();
                t.ParentBox = parts[0].Trim();
                t.BoxPosition = ConvertR1C1ToPosition(parts[1].Trim());
                t.TubeAccession = parts[2].Trim();
                t.TubeLabel = parts[3].Trim();
                t.SampleID = parts[4].Trim();
                t.Additives = parts[5].Trim();
                t.SampleType = parts[6].Trim();
                t.Notes = parts[7].Trim();
                sd.Add(t);

                
            }

            sr.Close();

            return sd;
        }

        private static int ConvertR1C1ToPosition(string pos)
        {
            int row = 0;
            int col = 0;
            if(pos.Length > 1)
            {
                if(pos[0] >= 65)
                {
                    row = pos[0] - 65;
                }
                col = pos[1]-48;

            }
            return row * 9 + col;
        }
        public static StorageBox LoadStorageBox(string filename)
        {
          
       
                StreamReader sr = new StreamReader(filename);
                char[] separator = new char[1] { ',' };
                string line = sr.ReadLine();
                if(line != null)
                {
                    string[] parts = line.Split(separator);
                    StorageBox box = new StorageBox(parts[1].Trim());
                  
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
                    case "SwabBag30":
                        box = LoadSwabBag30(sr, box);
                        break;
                   
                    case "Box_Accession":
                        box = LoadBoxAccessionFormat(sr, box);
                        break;
                    case "SwabBag_Accession":
                        box = LoadSwabBagAccessionFormat(sr, box);
                        break;
                        
                    default:
                        return null;
                    }
                   
                    sr.Close();
                    return box;
                }

               
              
            

           

            return null;

        }



        private static StorageBox LoadShort3BoxFile(StreamReader sr, StorageBox box)
        {
            DateTime glyDate = new DateTime(2017, 6, 28);
            string boxAcc = box.Name.Substring(4, 5);
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
                            td[i].date =DateTime.Parse(lineParts[ind + 1].Trim());
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

                                bool useGly = false;
                                if (i < 2 && (td[x].date) >= glyDate)
                                    useGly = true;

                                box.SampleTubes.Add(BuildTubeFromTubeData(td[x], ind, box.Name, useGly, x, false));
                             
                               

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
               string boxAcc = box.Name.Substring(4, 5);
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
                            td[i].date =DateTime.Parse(lineParts[ind + 1].Trim());
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
                                bool useGly = false;
                                if (i < 2 && (td[x].date) >= glyDate)
                                    useGly = true;

                                box.SampleTubes.Add(BuildTubeFromTubeData(td[x], ind, box.Name, useGly, x, true));
                            }
                        }
                    }
                                           
                    }

                    lineCount++;
                }
                return box;
        }

        private static Tube BuildTubeFromTubeData(TubeData td, int pos, string boxName, bool useGly, int numInSample, bool attachNumInSample)
        {
            Tube t = new Tube();
            t.ParentBox = boxName;
            t.LegacyID = td.id;
            t.SampleDate = td.date;
            t.TubeAccession = t.TubeAccession = CreateAccessionString(boxName, pos);
            string tubeNumForLabel = "";
            if (attachNumInSample)
                tubeNumForLabel = " " + numInSample.ToString();
            t.TubeLabel = td.id + " " + td.date.ToShortDateString() + tubeNumForLabel;
            if (useGly)
                t.Additives = "Glycerol";
            t.BoxPosition = pos;

            return t;
        }

        private static StorageBox LoadLong6BoxFile(StreamReader sr, StorageBox box)
        {
            DateTime glyDate = new DateTime(2017, 6, 28);
            string boxAcc = box.Name.Substring(4, 5);
            char[] separator = new char[1] { ',' };
            
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
                        DateTime dt = DateTime.Parse(parts[2].Trim());
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
                            Tube t = new Tube();
                            t.LegacyID = id;
                            t.SampleDate = dt;
                            t.TubeAccession = CreateAccessionString(box.Name, indices[i]);
                           // t.TubeAccession = "CDIF_" + boxAcc + "_" + indices[i].ToString().PadLeft(2, '0');
                            t.TubeLabel = id + " " + dt.ToShortDateString() + " " + i.ToString();
                            t.ParentBox = box.Name;
                            t.BoxPosition = indices[i];
                            if (i < 2 && (dt >= glyDate))
                                t.Additives = "Glycerol";

                            box.SampleTubes.Add(t);

                        }
                    }
                }
                lineCount++;
            }
                return box;
        }

        private static StorageBox LoadBoxAccessionFormat(StreamReader sr, StorageBox box)
        {
            char[] separator = new char[1] { ',' };

            int lineCount = 1;
            string line = "";


            while ((line = sr.ReadLine()) != null)
            {
                if (lineCount >= 2)
                {
                    string[] parts = line.Split(separator);
                    string accession = parts[1].Trim();
                    string label = parts[2].Trim();
                    string sampleID = parts[3].Trim();
                    string additives = parts[4].Trim();
           

                    if (!sampleID.Equals(""))
                    {
                        Tube temp = new Tube();
                        temp.TubeAccession = accession;
                        temp.TubeLabel = label;
                        temp.SampleID = sampleID;
                        temp.Additives = additives;
                        temp.ParentBox = box.Name;
                        temp.BoxPosition = lineCount - 2;
                        box.SampleTubes.Add(temp);

                    }
                }
                lineCount++;
            }


                return box;
        }

        private static StorageBox LoadSwabBagAccessionFormat(StreamReader sr, StorageBox box)
        {
            
            char[] separator = new char[1] { ',' };

            int lineCount = 1;
            string line = "";
            box.BoxSize = 30;

            while ((line = sr.ReadLine()) != null)
            {
                if (lineCount >= 2)
                {
                    string[] parts = line.Split(separator);
                    string accession = parts[1].Trim();
                    string label = parts[2].Trim();
                    string sampleID = parts[3].Trim();
                    

                    if (!sampleID.Equals(""))
                    {
                        Tube temp = new Tube();
                        temp.TubeAccession = accession;
                        temp.TubeLabel = label;
                        temp.SampleID = sampleID;
                        temp.Additives = "";
                        temp.ParentBox = box.Name;
                        temp.BoxPosition = lineCount - 2;

                        box.SampleTubes.Add(temp);

                    }
                }
                lineCount++;
            }


            return box;
        }

        private static StorageBox LoadSwabBag30(StreamReader sr, StorageBox box)
        {
            char[] separator = new char[1] { ',' };

            int lineCount = 1;
            string line = "";
            box.BoxSize = 30;

            while ((line = sr.ReadLine()) != null)
            {
                if (lineCount >= 2)
                {
                    string[] parts = line.Split(separator);
                    if (parts[1].Trim() != "")
                    {
                        string legacyID = parts[1].Trim();
                        DateTime date = DateTime.Parse(parts[2].Trim());



                        if (!legacyID.Equals(""))
                        {
                            Tube temp = new Tube();
                            temp.TubeAccession = "SWAB_" + box.Name.Substring(4) + "_" + (lineCount - 1).ToString().PadLeft(2, '0');
                            temp.TubeLabel = legacyID + " " + date.ToShortDateString();
                            temp.SampleID = "";
                            temp.Additives = "";
                            temp.ParentBox = box.Name;
                            temp.BoxPosition = 0;
                            box.SampleTubes.Add(temp);

                        }
                    }
                }
                lineCount++;
            }


            return box;
        }

        public static void WriteBoxDataToBoxAccessionFiles(StorageBox[] boxes, string directory, char delim)
        {
            for(int i= 0; i < boxes.Length; i++)
            {
                string filename = directory + boxes[i].Name + ".csv";

                StreamWriter sw = new StreamWriter(filename);
                sw.WriteLine("Box Name:"+ delim +  boxes[i].Name +  delim + "Format" + delim + "Box_Accession");
                List<string> head = CreateHeaderForSampleBox();
               

                StringBuilder sbhead = new StringBuilder();
               for(int k = 1; k < head.Count; k++)
                {
                    sbhead.Append(head[k]);
                    sbhead.Append(delim);
                }
                sw.WriteLine(sbhead.ToString());

                for(int j = 0; j < boxes[i].SampleTubes.Count; j++)
                {
                    StringBuilder sb = new StringBuilder();

                   List<string> line = CreateTubeDataLine(boxes[i].SampleTubes[j]);

                    for(int x = 1; x < line.Count; x++)
                    {
                        sb.Append(line[x]);
                        sb.Append(delim);
                    }
                    sw.WriteLine(sb.ToString());
                }

                sw.Close();
            }

        }

        private static List<string> CreateTubeDataLine(Tube t)
        {
            List<string> line = new List<string>();
            line.Add(t.ParentBox);
            if (t.ParentBox.Contains("Box"))
                line.Add(PositionToRowColFormat(t.BoxPosition));
            else
                line.Add("");
            line.Add(t.TubeAccession);
            line.Add(t.TubeLabel);
            line.Add(t.SampleID);
            line.Add(t.Additives);
            line.Add(t.SampleType);
            line.Add(t.Notes);

            return line;

        }

        private  static string PositionToRowColFormat(int pos)
        {
            int width = 9;
            int x = pos / width;
            int y = pos % width;
            y++;
            char row = (char)(x + 65); //65 is 'A'; 

            return row + y.ToString();
        }

        public static List<string> CreateHeaderForSampleBox()
        {
            List<string> head = new List<string>();
            head.Add("Box Name");   
            head.Add("Position");
            head.Add("Accession");
            head.Add("Label");
            head.Add("Sample ID");
            head.Add("Additives");
            head.Add("Sample Type");
            head.Add("Notes");


            return head;
        }

        public static void WriteStorageData(StorageData sd, string filename, char delim)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine(BuildDelimitedString(CreateHeaderForSampleBox(), delim));

            for(int i = 0; i < sd.Tubes.Count; i++)
            {
                sw.WriteLine(BuildDelimitedString(CreateTubeDataLine(sd.Tubes[i]), delim));
            }


            sw.Close();
        }

        public static void WriteStorageData(StorageData sd, string filename)
        {
            WriteStorageData(sd, filename, ',');
        }

        private static string BuildDelimitedString(List<string> parts, char delim)
        {
            StringBuilder sb = new StringBuilder();
            for(int i =0; i < parts.Count; i++)
            {
                sb.Append(parts[i]);
                sb.Append(delim);
            }
            return sb.ToString();
        }


        ///TODO - merge both box writing functions - they reuse practically all of their code.
        public static void WriteBoxData(StorageBox[] boxes, string directory, char delim, BoxWritingStyle style)
        {

            for (int i = 0; i < boxes.Length; i++)
            {
                string filename = directory + boxes[i].Name + ".csv";

                StreamWriter sw = new StreamWriter(filename);
                sw.WriteLine("Box Name:" + delim + boxes[i].Name + delim + "Format" + delim + "Box_Accession");
                List<string> head = CreateHeaderForSampleBox();


                StringBuilder sbhead = new StringBuilder();
                for (int k = 1; k < head.Count; k++)
                {
                    sbhead.Append(head[i]);
                    sbhead.Append(delim);
                }
                sw.WriteLine(sbhead.ToString());

                for (int j = 0; j < boxes[i].SampleTubes.Count; j++)
                {
                    StringBuilder sb = new StringBuilder();

                    List<string> line = CreateTubeDataLine(boxes[i].SampleTubes[i]);

                    for (int x = 1; x < line.Count; x++)
                    {
                        sb.Append(line[x]);
                        sb.Append(delim);
                    }
                    sw.WriteLine(sb.ToString());
                }

                sw.Close();
            }
        }

        public static void WriteTubeDataToFile(Tube[] tubes, string filename, char delim)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("Box Dataset:" + delim + "ALL" + delim + "Format" + delim + "Box_Database");

            List<string> head = CreateHeaderForSampleBox();
            StringBuilder headSB = new StringBuilder();
            for (int i = 0; i < head.Count; i++)
            {
                headSB.Append(head[i]);
                headSB.Append(delim);
            }

            sw.WriteLine(headSB.ToString());

            for (int j = 0; j < tubes.Length; j++)
            {
                List<string> line = CreateTubeDataLine(tubes[j]);

                StringBuilder lineSB = new StringBuilder();

                for (int k = 0; k < line.Count; k++)
                {
                    lineSB.Append(line[k]);
                    lineSB.Append(delim);
                }
                sw.WriteLine(lineSB.ToString());
            }
            sw.Close();
        }

        public static void WriteBoxesToSingleFile(StorageBox[] boxes, string filename, char delim)
        {
            List<Tube> tubes = new List<Tube>();
            for(int i = 0; i < boxes.Length; i++)
            {
                tubes.AddRange(boxes[i].SampleTubes);
            }
            WriteTubeDataToFile(tubes.ToArray(), filename, delim);
        }


        private static string PadIdentifier(string id)
        {
            if (char.IsLetter(id[0]))
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

        private static string CreateAccessionString(string boxName, int pos)
        {
            pos++;
            return "CDIF_" + boxName.Substring(4) + "_" + pos.ToString().PadLeft(2, '0');
        }






       
    }
}
