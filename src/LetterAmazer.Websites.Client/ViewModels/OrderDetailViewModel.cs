using System;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Pricing;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class OrderDetailViewModel
    {
        public AddressInfo AddressInfo { get; set; }
        public LetterDetails LetterDetails { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public DateTime? DatePaid { get; set; }

        public DateTime? DateSent { get; set; }

        public Price Price { get; set; }

        public int Id { get; set; }
    }

}