using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.Data
{
   public class BoxLocation
    {
        public string boxID = "";
        const int boxWidth = 9;
        public int row =0;
        public int col =0;
        int accession = 0;
        string accessionPrefix = "CDIF";

        public string Box { get => boxID; set => boxID = value; }
        public String LocationInBox { get => GetRowLetter() + (col+1).ToString(); }
        public int NumericLocationInBox { get => (row * boxWidth + col); }
        public int Row { get => row; set => row = value; }
        public int Column { get => col; set => col = value; }
        public BoxLocation(string id, int samRow, int samCol)
        {
            boxID = id;
            row = samRow;
            col = samCol;
        }

       private char GetRowLetter()
        {
            switch(row)
            {
                case 0:
                    return 'A';
                case 1:
                    return 'B';
                case 2:
                    return 'C';
                case 3:
                    return 'D';
                case 4:
                    return 'E';
                case 5:
                    return 'F';
                case 6:
                    return 'G';
                case 7:
                    return 'H';
                case 8:
                    return 'I';
            }
            return 'Z';
        }

        public string AccessionIdentifier
        {
            get { return accessionPrefix + accession.ToString().PadLeft(8, '0'); }
        }
    }
}
