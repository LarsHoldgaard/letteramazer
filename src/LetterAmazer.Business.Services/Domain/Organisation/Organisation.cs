using System;
using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.AddressInfos;

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

        public Organisation()
        {
            this.AddressList = new List<AddressList>();
            this.Address = new AddressInfo();
        }
    }
}
