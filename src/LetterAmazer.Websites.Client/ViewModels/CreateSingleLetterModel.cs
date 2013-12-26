using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class CreateSingleLetterModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RecipientName { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationCountryCode { get; set; }
        public string ZipCode { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationAddress { get; set; }

        public string VoucherCode { get; set; }
        public bool SignUpNewsletter { get; set; }

        public bool UseUploadFile { get; set; }
        public string[] UploadFiles { get; set; }
        public string WriteContent { get; set; }
    }
}