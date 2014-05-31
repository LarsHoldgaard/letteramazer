using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public class Letter:BaseItem
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OrganisationId { get; set; }
        public AddressInfo FromAddress { get; set; }
        public AddressInfo ToAddress { get; set; }
        public LetterStatus LetterStatus { get; set; }
        
        public LetterContent LetterContent { get; set; }
        public LetterDetails LetterDetails { get; set; }

        public int OfficeId { get; set; }
        public DeliveryLabel DeliveryLabel { get; set; }
        public int ReturnLabel { get; set; }

        public Letter()
        {
            this.LetterStatus =LetterStatus.Created;
            this.LetterDetails = new LetterDetails();
            this.LetterContent = new LetterContent();
            this.FromAddress = new AddressInfo();
            this.ToAddress = new AddressInfo();
        }
    }
}
