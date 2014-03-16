using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public class Letter:BaseItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public AddressInfo FromAddress { get; set; }
        public AddressInfo ToAddress { get; set; }
        public LetterStatus LetterStatus { get; set; }
        public int OfficeProductId { get; set; }
        public LetterContent LetterContent { get; set; }
        public LetterDetails LetterDetails { get; set; }

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
