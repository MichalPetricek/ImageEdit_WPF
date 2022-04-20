using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WpfApp1.Model
{
    internal class Filters
    {
        public int[,] Pixels { get; set; }
        public bool Parallel { get; set; }
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


        public void Colors(int channel)
        {
            if (Parallel)
            {
                if (Pixels != null)
                {
                    uint mask = 0;

                    switch (channel)
                    {
                        case 0: mask = 0xFFFF0000; break;
                        case 1: mask = 0xFF00FF00; break;
                        case 2: mask = 0xFF0000FF; break;
                    }
                    int controlHeight = (int)Math.Ceiling((decimal)Pixels.GetLength(0));
                    int controlWidth = (int)Math.Ceiling((decimal)Pixels.GetLength(1));
                    Task[] tasks = new Task[Environment.ProcessorCount];
                    int constant = (int)(Pixels.GetLength(0) / tasks.Length);

                    for (int i = 0; i < tasks.Length; i++)
                    {
                        tasks[i] = Task.Factory.StartNew(() =>
                        {
                            for (int x = constant * i; x < constant * i + 1; x++)
                            {
                                for (int y = 0; y < controlWidth; y++)
                                {
                                    if (!(x >= controlHeight || y >= controlWidth))
                                        Pixels[x, y] &= (int)mask;
                                }
                            }
                        });
                    }
                    Task.WaitAll();
                }
            }
            else
            {
                if (Pixels != null)
                {
                    uint mask = 0;

                    switch (channel)
                    {
                        case 0: mask = 0xFFFF0000; break;
                        case 1: mask = 0xFF00FF00; break;
                        case 2: mask = 0xFF0000FF; break;
                    }
                    for (int x = 0; x < Pixels.GetLength(0); x++)
                    {
                        for (int y = 0; y < Pixels.GetLength(1); y++)
                        {
                            Pixels[x, y] &= (int)mask;
                        }
                    }

                }
            }
        }
        public void Blur(Bitmap image)
        {
            Bitmap blurred = image;
            Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            for (int xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (int yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    int avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    for (int x = xx; (x < xx + 3 && x < image.Width); x++)
                    {
                        for (int y = yy; (y < yy + 3 && y < image.Height); y++)
                        {
                            Color pixel = blurred.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    
                    for (int x = xx; x < xx + 3 && x < image.Width && x < rectangle.Width; x++)
                        for (int y = yy; y < yy + 3 && y < image.Height && y < rectangle.Height; y++)
                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }
            using (var memory = new MemoryStream())
            {
                blurred.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                Pixels = Array2DBMIConverter.BitmapImageToArray2D(bitmapImage);

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
