using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public enum PaymentMethods
    {
        PayPal=0,
        Invoice=1,
        Bitcoin=2,
        Credits=3
    }
}
