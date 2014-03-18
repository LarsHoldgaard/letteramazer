using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Api;

namespace LetterAmazer.Business.Services.Domain.Organisation
{
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressInfo Address { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public Guid Guid { get; set; }

        public List<AddressList> AddressList { get; set; }

        public bool IsPrivate { get; set; }

        public OrganisationSettings OrganisationSettings { get; set; }

        public int? RequiredOfficeId { get; set; }
        public int? RequiredFulfillmentPartnerId { get; set; }

        public List<ApiKeys> ApiKeys { get; set; }


        public Organisation()
        {
            this.AddressList = new List<AddressList>();
            this.Address = new AddressInfo();
            this.OrganisationSettings = new OrganisationSettings();
            this.ApiKeys = new List<ApiKeys>();
        }
    }
}
