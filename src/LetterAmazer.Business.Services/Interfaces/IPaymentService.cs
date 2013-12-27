using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Interfaces
{
    public interface IPaymentService
    {
        string Process(OrderContext orderContext);
        IPaymentMethod Get(string methodName);
    }
}
