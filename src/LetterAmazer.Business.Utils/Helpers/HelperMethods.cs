using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace LetterAmazer.Business.Utils.Helpers
{
    public static class HelperMethods
    {


        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string Utf8FixString(string fuckedString)
        {
            fuckedString = fuckedString.Replace("&amp;nbsp;", "\n\n");
            byte[] bytes = Encoding.Default.GetBytes(fuckedString);
            var utf8fixed = Encoding.UTF8.GetString(bytes);
            var decoded = HttpUtility.HtmlDecode(utf8fixed);
            return decoded;
        }

        public static void CreateFileFromMemoryStream(string path, MemoryStream ms)
        {
            using (FileStream file = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
            {
                

                byte[] bytes = new byte[ms.Length];
                ms.Read(bytes, 0, (int)ms.Length);
                file.Write(bytes, 0, bytes.Length);
                ms.Close();
            }
        }

        public static string GetMD5HashFromStream(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string HashFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return HashFile(fs);
            }
        }

        public static string HashFile(Stream stream)
        {
            StringBuilder sb = new StringBuilder();

            if (stream != null)
            {
                stream.Seek(0, SeekOrigin.Begin);

                MD5 md5 = MD5CryptoServiceProvider.Create();
                byte[] hash = md5.ComputeHash(stream);
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));

                stream.Seek(0, SeekOrigin.Begin);
            }

            return sb.ToString();
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static List<int> ControlListSelections(ListControl list)
        {
            var values = new List<int>();
            for (var counter = 0; counter < list.Items.Count; counter++)
            {
                if (list.Items[counter].Selected)
                {
                    int cValue = 0;
                    Int32.TryParse(list.Items[counter].Value, out cValue);
                    if (cValue > 0)
                    {
                        values.Add(cValue);
                    }
                    else
                    {
                        throw new ArgumentException("The list selected values is not ints");
                    }
                }
            }
            return values;
        }

        /// <summary>
        /// Get the last char in a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string LastChar(string s)
        {
            return s.Substring(s.Length - 1);
        }

        public static int ConvertToInt(string d)
        {
            try
            {
                return int.Parse(d);
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public static string ConvertToFriendlyUrl(string s)
        {
            var illegalChars = ("+^:;,?½<>´'!$€\"*~%&/\\().µ").ToCharArray();
            var input = s.Replace(" ", "-").Replace("ø","o").Replace("å","a").Replace("æ","ae");
            return CleanString(input, illegalChars).Replace("--", "-").Replace("--", "-").Replace("--", "-").Replace("--", "-").ToLower().Trim();
        }

        public static string CleanString(string str, char[] badChars)
        {
            var result = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                if (!badChars.Contains(str[i]))
                    result.Append(str[i]);
            }
            return result.ToString();
        }

        public static string[] RemoveJsonFromEntries(string entries)
        {
            return JsonConvert.DeserializeObject<string[]>(entries);
        }

        public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var cnt = new Dictionary<T, int>();

            if(list1 != null) {
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            }

            if (list2 != null)
            {
                foreach (T s in list2)
                {
                    if (cnt.ContainsKey(s))
                    {
                        cnt[s]--;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        public static string AnalyticsScript(string id)
        {
            StringBuilder bld = new StringBuilder();
            bld.AppendLine("<script type=\"text/javascript\">");
            bld.AppendLine("var _gaq = _gaq || [];");
            bld.AppendLine(string.Format("_gaq.push(['_setAccount', '{0}']);", id));
            bld.AppendLine("_gaq.push(['_trackPageview']);");
            bld.AppendLine("(function () {");
            bld.AppendLine("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
            bld.AppendLine("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
            bld.AppendLine("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
            bld.AppendLine("})();");
            bld.AppendLine("setTimeout(\"_gaq.push(['_trackEvent','15_seconds','read'])\", 15000);")
            ;
            bld.AppendLine("</script>");
            return bld.ToString();
        }



        public static string RemoveMultipleWhitespaces(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
        }

        public static bool IsMobileSession()
        {
            return HttpContext.Current.Request.Browser["IsMobileDevice"] == "true";
            
        }
        public static string WriteOutList(List<int> objects)
        {
            return string.Join(",", objects.ToArray());
        }

        public static decimal AddVat(decimal number, decimal percentage)
        {
            return number*(1+(percentage/100));

        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Zæøå0-9_.]+", "", RegexOptions.Compiled);
        }

        public static string ConvertStringToAlias(string s)
        {
            var illegalChars = ("+^:;,?½<>´'!$€\"*~%&/\\().µ").ToCharArray();
            var input = s.Replace(" ", "-").Replace("ø", "o").Replace("å", "a").Replace("æ", "ae");
            return CleanString(input, illegalChars).Replace("--", "-").Replace("--", "-").Replace("--", "-").Replace("--", "-").ToLower().Trim();
        }
    }
}
