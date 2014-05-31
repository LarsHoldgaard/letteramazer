using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Domain.Checkout
{
    public interface ICheckoutService
    {
        Order ConvertCheckout(Checkout checkout);
    }
}
