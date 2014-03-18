using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LetterAmazer.Business.Utils.Helpers
{
    public static class PathHelper
    {
    
        public static string GetAbsoluteFile(string filename)
        {
            var pdfPath = ConfigurationManager.AppSettings["LetterAmazer.Settings.StorePdfPath"];
            string filepath = HttpContext.Current.Server.MapPath(pdfPath + "/" + filename);
            FileInfo file = new FileInfo(filepath);
            if (!Directory.Exists(file.DirectoryName))
            {
                Directory.CreateDirectory(file.DirectoryName);
            }
            return filepath;
        }
    }
}
