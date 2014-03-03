using System;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.AddressInfos;

namespace LetterAmazer.Business.Services.Domain.Customers
{
    public class Customer
    {
        public int Id { get; set; }
        public AddressInfo CustomerInfo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public DateTime? DateActivated { get; set; }
        public decimal Credit { get; set; }
        public decimal CreditLimit { get; set; }
        public string ResetPasswordKey { get; set; }
        public string RegisterKey { get; set; }
        public int OrganisationId { get; set; }

        public bool IsActivated {
            get
            {
                if ((DateActivated.HasValue && DateActivated.Value <= DateTime.Now)&& (string.IsNullOrEmpty(RegisterKey)))
                {
                    return true;
                }
                return false;
            }
        }

        public decimal CreditsLeft
        {
            get { return Credit - CreditLimit; }
        }

        public Customer()
        {
            this.CustomerInfo = new AddressInfo();
        }
    }
}
