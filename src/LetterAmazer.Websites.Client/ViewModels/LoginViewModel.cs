using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SharedRes = LetterAmazer.Websites.Client.Resources.Views.Shared;
using HomeRes = LetterAmazer.Websites.Client.Resources.Views.Home;
using DataAnnotationsExtensions;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email", ResourceType = typeof(HomeRes.ViewRes))]
        [Required(ErrorMessageResourceName = "ThisFieldIsRequired", ErrorMessageResourceType = typeof(SharedRes.ViewRes))]
        [Email(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(SharedRes.ViewRes))]
        public string Email { get; set; }
        [Display(Name = "Password", ResourceType = typeof(HomeRes.ViewRes))]
        [Required(ErrorMessageResourceName = "ThisFieldIsRequired", ErrorMessageResourceType = typeof(SharedRes.ViewRes))]
        public string Password { get; set; }

        public bool? Remember { get; set; }
        public string ReturnUrl { get; set; }
    }
}