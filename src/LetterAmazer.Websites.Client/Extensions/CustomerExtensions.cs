using System;
using LetterAmazer.Business.Services.Domain.Customers;

namespace LetterAmazer.Websites.Client.Extensions
{
    public static class CustomerExtensions
    {
        public static decimal GetAvailableCredits(this Customer customer)
        {
            return customer.Credit + Math.Abs(customer.CreditLimit);
        }
    }
}