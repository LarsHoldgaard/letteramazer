using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public bool? Remember { get; set; }
        public string ReturnUrl { get; set; }
    }
}