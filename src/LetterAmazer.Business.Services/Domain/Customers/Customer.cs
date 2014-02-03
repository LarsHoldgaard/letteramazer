using System;
using LetterAmazer.Business.Services.Domain.AddressInfos;

namespace LetterAmazer.Business.Services.Domain.Customers
{
    public class Customer
    {
        public int Id { get; set; }
        public AddressInfo CustomerInfo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public decimal Credit { get; set; }
        public decimal CreditLimit { get; set; }
        public string ResetPasswordKey { get; set; }

    }
}
