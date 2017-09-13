using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcdiffrecords.DataReconciliation
{
    public enum Box
    {
        Donskey,
        CDiff,
    };

    public struct BoxLocation
    {
        int x;
        int y;
        Box box;
        int boxNum;


        public BoxLocation(int row, int col, Box bx, int bxnum)
        {
            
            x = row;
            y = col;
            box = bx;
            boxNum = bxnum;
        }



        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public Box BoxIdentifier
        {
            get { return box; }
        }
        
        public int BoxNumber
        {
            get { return boxNum; }
        }

        public override bool Equals(object obj)
        {
            if (obj is BoxLocation)
            {
                return ((box == ((BoxLocation)obj).BoxIdentifier)&&(boxNum == ((BoxLocation)obj).BoxNumber)&&(x == ((BoxLocation)obj).X) && (y == ((BoxLocation)obj).Y));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (x.GetHashCode() * 5) + y.GetHashCode() + (box.GetHashCode()*3) + (boxNum.GetHashCode()*7);
        }

        public override string ToString()
        {
            return GetBox() + " " + boxNum.ToString() + " [" + GetRow() + "," + x.ToString() + "]";
        }
        private string GetBox()
        {
            if (box == Box.CDiff)
                return "CDiff";
            return "Donskey";
        }
        private string GetRow()
        {
            switch (y)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";
                case 6:
                    return "F";
                case 7:
                    return "G";
                case 8:
                    return "H";
                case 9:
                    return "I";
                default:
                    return "Z";
            }
        }
    }
}
