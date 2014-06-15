using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class AccountViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
        public string ReturnUrl { get; set; }
        public AccountViewModel()
        {
            this.LoginViewModel = new LoginViewModel();
            this.RegisterViewModel = new RegisterViewModel();
        }
    }
}