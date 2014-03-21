using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using GhostscriptSharp;

namespace LetterAmazer.Business.Thumbnail
{
    public class ThumbnailGenerator
    {
        private string baseStorePath;

        public ThumbnailGenerator(string basePath)
        {
            //baseStorePath = ConfigurationManager.AppSettings.Get("LetterAmazer.Settings.StoreThumbnail");
            baseStorePath = basePath;
        }

        public byte[] GetThumbnailFromLetter(byte[] inputfile)
        {
            string tempInputFilePath = Guid.NewGuid().ToString() + ".pdf";
            string tempOutputFilePatah = Guid.NewGuid().ToString() + ".jpg";

            StorePdfFile(inputfile, tempInputFilePath);

            
            GhostscriptWrapper.GeneratePageThumb(tempInputFilePath, tempOutputFilePatah, 1, 100, 100);

            byte[] data = null;
            using (FileStream fs = new FileStream(tempInputFilePath, FileMode.Open))
            {
                using (Bitmap bmp = new Bitmap(fs))
                {
                    using (var ms = new MemoryStream())
                    {
                        Brush brush = new SolidBrush(Color.FromArgb(128, 230, 230, 230));

                        Graphics gra = Graphics.FromImage(bmp);

                        gra.FillRectangle(brush, 60, 170, 300, 150);
                        bmp.Save(ms, ImageFormat.Jpeg);

                        data = ReadFully(ms);
                    }
                }
            }
            return data;
        }

        public byte[] GetThumbnailFromA4(byte[] inputfile)
        {
            string tempInputFilePath = baseStorePath + "\\" +Guid.NewGuid().ToString() + ".pdf";
            string tempOutputFilePatah = baseStorePath + "\\" + Guid.NewGuid().ToString() + ".jpg";
            
            StorePdfFile(inputfile,tempInputFilePath);

            GhostscriptWrapper.GeneratePageThumb(tempInputFilePath, tempOutputFilePatah, 1, 100, 100);

            byte[] data = null;
            using (FileStream fs = new FileStream(tempInputFilePath, FileMode.Open))
            {
                using (Bitmap bmp = new Bitmap(fs))
                {
                    using (var ms = new MemoryStream())
                    {
                        Brush brush = new SolidBrush(Color.FromArgb(128, 230, 230, 230));

                        Graphics gra = Graphics.FromImage(bmp);

                        gra.FillRectangle(brush, 30, 170, 300, 150);
                        bmp.Save(ms,ImageFormat.Jpeg);

                        data = ReadFully(ms);
                    }
                }
            }
            return data;
        }

        private void StorePdfFile(byte[] inputfile, string storepath)
        {
            File.WriteAllBytes(storepath,inputfile);
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
