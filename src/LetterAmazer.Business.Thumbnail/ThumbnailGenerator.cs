using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using GhostscriptSharp;
using log4net;

namespace LetterAmazer.Business.Thumbnail
{
    public class ThumbnailGenerator
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ThumbnailGenerator));

        private string baseStorePath;

        public ThumbnailGenerator(string basePath)
        {
            //baseStorePath = ConfigurationManager.AppSettings.Get("LetterAmazer.Settings.StoreThumbnail");
            baseStorePath = basePath;
        }

        public byte[] GetThumbnailFromLetter(byte[] inputfile)
        {
            int rectangleX = 120;
            int rectangleY = 50;
            int width = 310;
            int height = 200;

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
                        // DRAWING LETTER DOCUMENT (NOT A4)
                        Brush brush = new SolidBrush(Color.FromArgb(128, 170, 170, 170));
                        Graphics gra = Graphics.FromImage(bmp);
                        
                        gra.FillRectangle(brush, rectangleX, rectangleY, width, height);
                        bmp.Save(ms, ImageFormat.Jpeg);

                        data = ReadFully(ms);
                    }
                }
            }
            return data;
        }

        public byte[] GetThumbnailFromA4(byte[] inputfile)
        {
            int rectangleX = 80;
            int rectangleY = 54;
            int width = 320;
            int height = 192;

            logger.Info("GetThumbnailFromA4");
            string tempInputFilePath = baseStorePath + "\\" +Guid.NewGuid().ToString() + ".pdf";
            string tempOutputFilePatah = baseStorePath + "\\" + Guid.NewGuid().ToString() + ".jpg";

            logger.Info("Temp input path: " + tempInputFilePath);
            logger.Info("Temp output path: " + tempOutputFilePatah);

            byte[] data = null;
            try
            {
                StorePdfFile(inputfile, tempInputFilePath);
                logger.Info("Pdf file stored");

                GhostscriptWrapper.GeneratePageThumb(tempInputFilePath, tempOutputFilePatah, 1, 100, 100);

                logger.Info("Ghostscript run perfectly :)");

                using (FileStream fs = new FileStream(tempOutputFilePatah, FileMode.Open))
                {
                    using (Bitmap bmp = new Bitmap(fs))
                    {
                        using (var ms = new MemoryStream())
                        {
                            // DRAWING A4 DOCUMENT
                            Brush brush = new SolidBrush(Color.FromArgb(128, 230, 230, 230));

                            Graphics gra = Graphics.FromImage(bmp);

                            gra.FillRectangle(brush, rectangleX, rectangleY, width, height);
                            gra.DrawString("123456", new Font("Verdana", 8.0f), new SolidBrush(Color.Black), rectangleX, rectangleY - 10);
                            bmp.Save(ms, ImageFormat.Jpeg);

                            ms.Position = 0;
                            data = ReadFully(ms);
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                logger.Error("Get thumbnail: " + ex + " ||| " + ex.Message);
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
