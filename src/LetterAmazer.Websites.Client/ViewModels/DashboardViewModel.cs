using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using System;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Websites.Client.ViewModels.User;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class DashboardViewModel
    {
        public Customer Customer { get; set; }
        public OrderOverviewViewModel OrderOverviewViewModel { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string SuccessMsg { get; set; }

        public LetterType LetterType { get; set; }

        public InvoiceOverviewViewModel UnpaidInvoices { get; set; }

        public decimal Credits { get; set; }
        public int LettersLastMonth { get; set; }
        public decimal MoneyLastMoney { get; set; }

        public DashboardStatus? DashboardStatus { get; set; }

        public DashboardViewModel()
        {
            this.UnpaidInvoices = new InvoiceOverviewViewModel();
            this.OrderOverviewViewModel = new OrderOverviewViewModel();
        }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string CreatedByEmail { get; set; }
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

    public enum DashboardStatus
    {
        Normal=0,
        SendLetter=1
    }
}