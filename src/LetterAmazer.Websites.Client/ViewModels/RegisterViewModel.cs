using System.Web.Mvc;
using DataAnnotationsExtensions;
using SharedRes = LetterAmazer.Websites.Client.Resources.Views.Shared;
using HomeRes = LetterAmazer.Websites.Client.Resources.Views.Home;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email", ResourceType = typeof(HomeRes.ViewRes))]
        [Required(ErrorMessageResourceName = "ThisFieldIsRequired", ErrorMessageResourceType = typeof(SharedRes.ViewRes))]
        [Email(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(SharedRes.ViewRes))]
        public string Email { get; set; }
        [Display(Name = "Password", ResourceType = typeof(HomeRes.ViewRes))]
        [Required(ErrorMessageResourceName = "ThisFieldIsRequired", ErrorMessageResourceType = typeof(SharedRes.ViewRes))]
        public string Password { get; set; }


        public string SelectedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }


        [Display(Name = "ConfirmPassword", ResourceType = typeof(HomeRes.ViewRes))]
        [EqualTo("Password", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(SharedRes.ViewRes))]
        public string ConfirmPassword { get; set; }

        public string ResetPasswordKey { get; set; }

        public RegisterViewModel()
        {
            this.Countries = new List<SelectListItem>();
        }
    }
}