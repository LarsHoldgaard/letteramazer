using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class BitcoinMethod : IPaymentMethod
    {
        public void Process(Order order)
        {
            throw new NotImplementedException();
        }

        public void ProcessFunds(Customer customer, decimal value)
        {
            throw new NotImplementedException();
        }
    }
}
