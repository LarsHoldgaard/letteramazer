using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Utils.Helpers
{
    public static class PasswordGenerator
    {
        private static char[] characterArray = "ABCDEFGHJKMNPQRSTUVWXYZ23456789".ToCharArray();
        private static Random randNum = new Random();

        private static char GetRandomCharacter()
        {
            return characterArray[(int)((characterArray.GetUpperBound(0) + 1) * randNum.NextDouble())];
        }

        public static string Generate(int passwordLength)
        {
            StringBuilder sb = new StringBuilder();
            sb.Capacity = passwordLength;
            for (int count = 0; count <= passwordLength - 1; count++)
            {
                sb.Append(GetRandomCharacter());
            }
            return sb.ToString();
        }
    }
}
