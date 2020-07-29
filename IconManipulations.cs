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
                    case 5: icon = Properties.Resources.miner; break;
                    case 6: icon = Properties.Resources.beacon; break;
                    default: icon = Properties.Resources.blank; break;
                }
            }
            return icon;
        }
    }
}
