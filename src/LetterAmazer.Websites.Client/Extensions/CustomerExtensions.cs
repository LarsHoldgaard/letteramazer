using LetterAmazer.Business.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.Extensions
{
    public static class CustomerExtensions
    {
        public static decimal GetAvailableCredits(this Customer customer)
        {
            return customer.Credits.Value + Math.Abs(customer.CreditLimit);
        }
    }
}