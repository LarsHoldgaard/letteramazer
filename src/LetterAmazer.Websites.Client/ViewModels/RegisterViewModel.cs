﻿using System.Web.Mvc;
using DataAnnotationsExtensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class RegisterViewModel
    {
        [EmailAddress(ErrorMessage = "This is not a valid e-mail address")]
        [Required(ErrorMessage = "You need to enter a valid e-mail address")]
        public string Email { get; set; }
     
        public string Password { get; set; }


        public string SelectedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }


        public string ConfirmPassword { get; set; }

        public string ResetPasswordKey { get; set; }

        public RegisterViewModel()
        {
            this.Countries = new List<SelectListItem>();
        }
    }
}