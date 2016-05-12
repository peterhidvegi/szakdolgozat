using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Validator.src
{
    public class Converter
    {

        public static System.Drawing.Bitmap ImgToBitmap(System.Windows.Controls.Image imgLife)
        {

            RenderTargetBitmap rtBmp = new RenderTargetBitmap((int)imgLife.ActualWidth, (int)imgLife.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);

            imgLife.Measure(new System.Windows.Size((int)imgLife.ActualWidth, (int)imgLife.ActualHeight));
            imgLife.Arrange(new Rect(new System.Windows.Size((int)imgLife.ActualWidth, (int)imgLife.ActualHeight)));


            rtBmp.Render(imgLife);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            MemoryStream stream = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(rtBmp));

            encoder.Save(stream);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream);
            return bitmap;

        }

        public static string ValueConverter(int conv)
        {
            if (conv < 10)
            {
                return "0" + conv;
            }
            else
            {
                return conv.ToString();
            }

        }

        public static Bitmap bitmapCropper(System.Windows.Controls.Image imageSrc)
        {
            Rectangle cropRect = new Rectangle(new System.Drawing.Point(0, 318), new System.Drawing.Size(1280, 102));
            string address = imageSrc.Source.ToString();
            address = address.Substring(8);
            Bitmap src = new Bitmap(address);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }

            return target;
        }


        public static BitmapImage ToBitmapImage(Bitmap bitmap)
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

    }
}
