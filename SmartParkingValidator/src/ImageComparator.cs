using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Validator.src
{
    class ImageComparator : IStoppableThread
    {
        BackgroundWorker bg = new BackgroundWorker();
        IMotionDataListener listener;
        Bitmap firstBitmap;
        Bitmap secondBitmap;
        string maskName;
        int interval;
        string eventDate;
        //old points: 0,150;640,150;0,225;640,225
        //new points: 0,100;640,100;0,160;640,160
        //5.1 coordinatas: 0,318 ; 1280,318 ; 0,420 ; 1280,420

        public ImageComparator(IMotionDataListener listener,string eventDate, Bitmap first, Bitmap second, string maskName,int interval)
        {
            this.firstBitmap = first;
            this.secondBitmap = second;
            this.maskName = maskName;
            this.interval = interval;
            this.listener = listener;
            this.eventDate = eventDate;
            bg.DoWork += Bg_DoWork;
            bg.RunWorkerCompleted += Bg_RunWorkerCompleted;
            bg.WorkerSupportsCancellation = true;
            bg.RunWorkerAsync();
            // Old version: new Thread(() => CompareImage(first, second, maskName,interval)).Start();
        }

        private void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            float result = (float)e.Result;
            if ( result < App.motionNodeCorrectPixelPercentage)
            {
                 listener.GetEventData(eventDate,maskName,result);
            }
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = CompareImage();
        }

        private float CompareImage()
        {
            Bitmap first = ConvertDrawingGroupToBitmap(MergeWithTransparentMask(firstBitmap, maskName));
            Bitmap second = ConvertDrawingGroupToBitmap(MergeWithTransparentMask(secondBitmap, maskName));
            float different = CorrectPixels(first, second, interval, "comparator"+maskName);
            return different;
        }

        private DrawingGroup MergeWithTransparentMask(Bitmap transformedImage, string maskNumber)
        {
            var group = new DrawingGroup();
            BitmapImage image = ToBitmapImage(transformedImage);
            string masksource = App.mainPath + App.maskDirName + maskNumber + ".png";
            group.Children.Add(new ImageDrawing(image, new Rect(0, 0, 1280, 102)));
            group.Children.Add(new ImageDrawing(new BitmapImage(new Uri(masksource)), new Rect(0, 0, 1280, 102)));
            return group;
        }

        private Bitmap ConvertDrawingGroupToBitmap(DrawingGroup dg)
        {
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawDrawing(dg);
                dc.Close();
                var bitmap = new RenderTargetBitmap(1280, 102, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(visual);
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                MemoryStream stream = new MemoryStream();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
                return new System.Drawing.Bitmap(stream);
            }
        }

        public BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static float CorrectPixels(Bitmap firstIMG, Bitmap secIMG, int intervalToCorrect, string saveName)
        {
            int correctPixels = 0;

            Bitmap first = (Bitmap)firstIMG.Clone();
            Bitmap second = (Bitmap)secIMG.Clone();
            int TotalPixels = first.Width * first.Height;
            int interval = intervalToCorrect;

            System.Drawing.Bitmap container = new System.Drawing.Bitmap(first.Width, first.Height);
           
                for (int i = 0; i < first.Width; i++)
                {
                    for (int j = 0; j < first.Height; j++)
                    { 

                        bool miss = false;
                        int r = first.GetPixel(i, j).R;
                        int g = first.GetPixel(i, j).G;
                        int b = first.GetPixel(i, j).B;
                        int r2 = second.GetPixel(i, j).R;
                        int g2 = second.GetPixel(i, j).G;
                        int b2 = second.GetPixel(i, j).B;

                        if (r2 == 0 && g2 == 0 && b2 == 0)
                        {
                            TotalPixels--;
                            miss = true;
                        }
                        if (!miss)
                        {
                            int rMin = r - interval;
                            int rMax = r + interval;
                            int gMin = g - interval;
                            int gMax = g + interval;
                            int bMin = b - interval;
                            int bMax = b + interval;

                            if ((rMin < r2 && rMax > r2) && (gMin < g2 && gMax > g2) && (bMin < b2 && bMax > b2))
                            {
                                correctPixels++;
                            }
                            else
                            {
                                second.SetPixel(i, j, System.Drawing.Color.FromArgb(255, 255, 255));
                            }
                        }
                    }
                }

            float dierence = ((float)correctPixels / (float)TotalPixels);
            float percentage = dierence * 100;
            WriteInFile(second, saveName);
            return percentage;
        }


        public static void WriteInFile(System.Drawing.Bitmap bmp, string source)
        {
            string sourceM = App.mainPath + "\\" + source + ".jpg";
            string outputFileName = sourceM;
            using (MemoryStream memory = new MemoryStream())
            {
                try
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        bmp.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                catch (IOException e)
                {
                    string fail = e.StackTrace;
                }

            }
        }

        public void StopBackgroundWorker()
        {
            bg.CancelAsync();
            bg.Dispose();
        }
    }
}
