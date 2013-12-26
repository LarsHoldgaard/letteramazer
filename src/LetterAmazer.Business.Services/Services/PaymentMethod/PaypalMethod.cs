using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Services.PaymentMethod
{
    public class PaypalMethod : IPaymentMethod
    {
        public string Name
        {
            get { return "Paypal"; }
        }

        public string Process(OrderContext orderContext)
        {
            return "";
        }
    }
}
