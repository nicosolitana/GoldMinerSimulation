using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace GoldMinerSimulation
{
    public static class IconManipulations
    {
        public static BitmapSource ConvertIcon(Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static Bitmap SelectIcon(int type)
        {
            Bitmap icon;
            if (type < 3)
            {
                switch (type)
                {
                    case 0: icon = Properties.Resources.rockOne; break;
                    case 1: icon = Properties.Resources.rockTwo; break;
                    default: icon = Properties.Resources.rockThree; break;
                }
            }
            else
            {
                switch (type)
                {
                    case 3: icon = Properties.Resources.potOfGold; break;
                    case 4: icon = Properties.Resources.pitBomb; break;
                    case 6: icon = Properties.Resources.beacon; break;
                    case 7: icon = Properties.Resources.minerNorth; break;
                    case 8: icon = Properties.Resources.minerEast; break;
                    case 9: icon = Properties.Resources.minerWest; break;
                    case 10: icon = Properties.Resources.minerSouth; break;
                    case 11: icon = Properties.Resources.winner; break;
                    default: icon = Properties.Resources.blank; break;
                }
            }
            return icon;
        }

        public static int GetMinerIcon(Direction dir)
        {
            switch (dir)
            {
                case Direction.North: return 7;
                case Direction.East: return 8;
                case Direction.West: return 9;
                default: return 10;
            }
        }
    }
}
