using System.Security.Cryptography;
using System.Text;

namespace LetterAmazer.Business.Utils.Helpers
{
    public static class SHA1PasswordEncryptor
    {
        public static string Encrypt(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in hash)
            {
                stringBuilder.Append(num.ToString("x2"));
            }
            return ((object)stringBuilder).ToString().ToLower();
        }

        public static bool Equal(string plainPassword, string encryptedPassword)
        {
            return string.Compare(Encrypt(plainPassword), encryptedPassword, true) == 0;
        }
    }
}
