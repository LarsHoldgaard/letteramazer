using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "This is not a valid e-mail address")]
        [Required(ErrorMessage = "You need to enter a valid e-mail address")]
        public string Email { get; set; }
        public string Password { get; set; }

        public bool? Remember { get; set; }
        public string ReturnUrl { get; set; }
    }
}