using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.ProductMatrix;

namespace LetterAmazer.Business.Services.Utils
{
    /// <summary>
    /// This is the start of a huge helper class I will regret some years into the future
    /// Wrote the 15-05-2014
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Used to synchronize the random method, so it gives different numbers when used in a loop
        /// </summary>
        private static readonly object syncLock = new object();

        /// <summary>
        /// Random generator to generate solutions/assignments
        /// </summary>
        private static readonly Random ran = new Random();

        /// <summary>
        /// Will return anything from the min to the max-1
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomInt(int min, int max)
        {
            lock (syncLock)
            {
                return ran.Next(min, max);
            }
        }

        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static Stream StringToStream(string src)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


        public static byte[] GetBytes(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string GetUploadDateString(string path)
        {
            return string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, path);
        }
    }
}
