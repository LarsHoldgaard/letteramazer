using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;

namespace LetterAmazer.Business.Services.Domain.Session
{
    public class Session
    {
        public Guid Id { get; set; }

        public Customer Customer { get; set; }
        public Checkout.Checkout Checkout { get; set; }


    }
}
