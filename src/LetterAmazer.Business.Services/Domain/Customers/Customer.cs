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
        public Organisation.Organisation Organisation { get; set; }

        public OrganisationRole? OrganisationRole { get; set; }

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

        public AddressInfo InvoiceAddress
        {
            get
            {
                if (this.Organisation != null)
                {
                    return this.Organisation.Address;
                }
                return CustomerInfo;
            }  

        }

        /// <summary>
        /// Calculates the vat percentage of the given user
        /// </summary>
        public decimal VatPercentage()
        {
            if (this.Organisation != null)
            {
                if (!this.Organisation.Address.Country.InsideEu)
                {
                    return 0.0m;
                }
            }
            else if (this.CustomerInfo != null && this.CustomerInfo.Country != null)
            {
                if (!this.CustomerInfo.Country.InsideEu)
                {
                    return 0.0m;
                }
            }
            return 0.25m;
        }

        public int PreferedCountryId
        {
            get
            {
                if (Organisation != null && Organisation.OrganisationSettings != null && Organisation.OrganisationSettings.PreferedCountryId.HasValue)
                {
                    return Organisation.OrganisationSettings.PreferedCountryId.Value;
                }

                if (Organisation != null && Organisation.OrganisationSettings != null && Organisation.Address != null)
                {
                    return Organisation.Address.Country.Id;
                }

                return CustomerInfo.Country.Id;
            }
        }

        public Customer()
        {
            this.CustomerInfo = new AddressInfo();
        }
    }
}
