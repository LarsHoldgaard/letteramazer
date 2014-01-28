using LetterAmazer.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Services
{
    public class SHA1PasswordEncryptor : IPasswordEncryptor
    {
        public string Encrypt(string password)
        {
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in hash)
            {
                stringBuilder.Append(num.ToString("x2"));
            }
            return ((object)stringBuilder).ToString().ToLower();
        }

        public bool Equal(string plainPassword, string encryptedPassword)
        {
            return string.Compare(this.Encrypt(plainPassword), encryptedPassword, true) == 0;
        }
    }
}
