using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using System;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class ProfileViewModel
    {
        public Customer Customer { get; set; }
        public List<OrderViewModel> Orders { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string SuccessMsg { get; set; }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public List<OrderLineViewModel> OrderLines { get; set; }

        public LetterStatus LetterStatus { get; set; }
    }

    public class OrderLineViewModel
    {
        public int Quantity { get; set; }
        public OrderLineProductViewModel OrderLineProductViewModel { get; set; }
    }

    public class OrderLineProductViewModel
    {
        
    }
}