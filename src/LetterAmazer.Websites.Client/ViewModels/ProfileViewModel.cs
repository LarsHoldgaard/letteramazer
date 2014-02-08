using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Model;
using System;

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