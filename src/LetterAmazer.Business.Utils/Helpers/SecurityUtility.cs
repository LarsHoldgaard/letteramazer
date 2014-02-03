using System;
using System.Text;
using System.Web;

namespace LetterAmazer.Business.Services.Utilities
{
    public static class SecurityUtility
    {
        private static Random random = new Random();
        private static String passwordChars = "0123456789abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVXYZ!@#$*";

        public static String GeneratePassword(int numberOfChars)
        {
            StringBuilder sb = new StringBuilder(numberOfChars);
            for (int i = 0; i < numberOfChars; i++)
            {
                sb.Append(passwordChars[random.Next(passwordChars.Length)]);
            }
            return sb.ToString();
        }

        public static String GetRandomName()
        {
            String retName = "";	// return this string

            // Seed random generator
            Random generator = new Random();

            int length = GetRandomBetween(5, 6);

            // CVCCVC or VCCVCV
            if (GetRandomBetween(1, 2) < 2)
            {
                retName += GetRandomConsonant();
                retName = retName.ToUpper();
                retName += GetRandomVowel();
                retName += GetRandomConsonant();
                retName += GetRandomConsonant();
                if (length >= 5) { retName += GetRandomVowel(); }
                if (length >= 6) { retName += GetRandomConsonant(); }
            }
            else
            {
                retName += GetRandomVowel();
                retName = retName.ToUpper();
                retName += GetRandomConsonant();
                retName += GetRandomConsonant();
                retName += GetRandomVowel();
                if (length >= 5) { retName += GetRandomConsonant(); }
                if (length >= 6) { retName += GetRandomVowel(); }
            }

            return retName;
        }

        // Returns a, e, i, o or u
        public static String GetRandomVowel()
        {
            int randNum = GetRandomBetween(1, 4);

            switch (randNum)
            {
                case 1:
                    return "a";
                case 2:
                    return "e";
                case 3:
                    return "i";
                case 4:
                    return "o";
            }

            return "u";
        }

        public static String GetRandomConsonant()
        {
            // Use the ascii values for a-z and convert to char
            char randLetter = (char)GetRandomBetween(97, 122);
            while (isCharVowel(randLetter))
            {
                randLetter = (char)GetRandomBetween(97, 122);
            }

            return randLetter.ToString();
        }

        public static bool isCharVowel(char letter)
        {
            if (letter == 'a' || letter == 'e' || letter == 'i' || letter == 'o' || letter == 'u')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Returns a random number between lowerbound and upperbound inclusive
        public static int GetRandomBetween(int lb, int ub)
        {
            Random generator = new Random();
            int ret = generator.Next(ub + 1 - lb) + lb;

            return ret;
        }

        public static Customer CurrentUser
        {
            get
            {
                HttpContext currentContext = HttpContext.Current;
                if (currentContext == null) return null;
                if (!currentContext.User.Identity.IsAuthenticated) return null;

                if (currentContext.Items["CurrentUser"] != null)
                {
                    return (Customer)currentContext.Items["CurrentUser"];
                }

                Customer user = ServiceFactory.Get<ICustomerService>().GetCustomerById(System.Convert.ToInt32(currentContext.User.Identity.Name));
                currentContext.Items["CurrentUser"] = user;
                return user;
            }
        }


        public static bool IsAuthenticated
        {
            get { return CurrentUser != null; }
        }
    }
}
