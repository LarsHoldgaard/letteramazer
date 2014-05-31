using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels.User
{
    public class OrderOverviewViewModel
    {
        public List<OrderViewModel> Orders { get; set; }

        public OrderOverviewViewModel()
        {
            this.Orders = new List<OrderViewModel>();
        }
    }
}