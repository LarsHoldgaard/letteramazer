namespace LetterAmazer.Business.Utils.Helpers
{
    public interface IPasswordEncryptor
    {
        string Encrypt(string password);
        bool Equal(string plainPassword, string encryptedPassword);
    }
}
