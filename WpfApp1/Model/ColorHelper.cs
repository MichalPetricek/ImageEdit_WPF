using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1.Model
{
    internal class ColorHelper
    {
        public int[,] Pixels { get; set; }

        public void FilterColor()
        {
            /*int a = solidColorBrush.Color.A;
            int r = solidColorBrush.Color.R;
            int g = solidColorBrush.Color.G;
            int b = solidColorBrush.Color.B;*/


            //int color = (a << 24) + (r << 16) + (g << 8) + b;
            for (int x = 0; x < Pixels.GetLength(0); ++x)
            {
                for (int y = 0; y < Pixels.GetLength(1); ++y)
                {

                    int a = (int)(Pixels[x, y] & 0xFF000000);
                    int r = (Pixels[x, y] >> 16) & 0xFF;
                    int g = (Pixels[x, y] >> 8) & 0xFF;
                    int b = (Pixels[x, y]) & 0xFF;
                    int prumer = (r + g + b) / 3;
                    Pixels[x, y] = a + (prumer << 16) + (prumer << 8) + prumer;
                }
            }
        }

        public void BlueFilter()
        {

            for (int x = 0; x < Pixels.GetLength(0); ++x)
            {
                for (int y = 0; y < Pixels.GetLength(1); ++y)
                {

                }
            }
        }

        public void Flip()
        {
            /*int[,] testArray = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            int[,] testArray2 = new int[testArray.GetLength(0), testArray.GetLength(1)];
            for (int x = 0; x < testArray.GetLength(0); ++x)
            {
                for (int y = 0; y < testArray.GetLength(1); ++y)
                {
                    testArray2[x, y] = testArray[x, testArray.GetLength(1) - (y + 1)];
                }
            }*/



            int[,] refArray = new int[Pixels.GetLength(0), Pixels.GetLength(1)];
            for (int x = 0; x < Pixels.GetLength(0); ++x)
            {
                for (int y = 0; y < Pixels.GetLength(1); ++y)
                {
                    refArray[x, y] = Pixels[x, Pixels.GetLength(1) - (y + 1)];
                }
            }
            Pixels = refArray;
        }
        public void Reduction()
        {
            for (int x = 0; x < Pixels.GetLength(0); ++x)
            {
                for (int y = 0; y < Pixels.GetLength(1); ++y)
                {
                    int color = Pixels[x, y];
                    Pixels[x, y] = (int)((int)color & 0xFFF0F0C0);
                }
            }
        }
    }
}
