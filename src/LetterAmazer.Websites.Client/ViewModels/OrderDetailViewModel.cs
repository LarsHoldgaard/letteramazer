using System;
using System.Collections.Generic;
using iTextSharp.text;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Pricing;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class OrderDetailViewModel
    {
        public List<LetterDetailViewModel> Letters { get; set; }
        
        public OrderStatus OrderStatus { get; set; }
        public int OrderId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DatePaid { get; set; }

        public DateTime? DateSent { get; set; }
        public Price Price { get; set; }

        public int Step { get; set; }
        

        public OrderDetailViewModel()
        {
            this.Letters = new List<LetterDetailViewModel>();
        }
    }


    public class LetterDetailViewModel
    {
        public int Id { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public LetterDetails LetterDetails { get; set; }
        public Price Price { get; set; }


    }

}