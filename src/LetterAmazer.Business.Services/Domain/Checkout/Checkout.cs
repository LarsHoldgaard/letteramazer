using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Partners;

namespace LetterAmazer.Business.Services.Domain.Checkout
{
    public class Checkout
    {
        public int UserId { get; set; }

        public List<PartnerTransaction> PartnerTransactions { get; set; }

        public string OrderNumber { get; set; }
        public List<int> PartnerIds { get; set; } 

        public string Email { get; set; }

        /// <summary>
        /// Contains all the letters, where the integer is an officeproductID and the letter is a letter
        /// </summary>
        public List<CheckoutLine> CheckoutLines { get; set; }

        public int RecipientCountry { get; set; }
        public int OriginCountry { get; set; }
        public int PaymentMethodId { get; set; }

        public Checkout()
        {
            this.CheckoutLines = new List<CheckoutLine>();
            this.PartnerIds = new List<int>();
            this.PartnerTransactions = new List<PartnerTransaction>();
        }

    }
}
