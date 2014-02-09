using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using System;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class ProfileViewModel
    {
        public Customer Customer { get; set; }
        public List<Order> Orders { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}