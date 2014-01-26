using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class ProfileViewModel
    {
        public Customer Customer { get; set; }
        public PaginatedInfo<Order> Orders { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}