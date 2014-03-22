using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class ApiViewModel
    {
        public string Email { get; set; }
        public string Organistion { get; set; }
        public string Comment { get; set; }

        public string Status { get; set; }
    }
}