using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Interfaces
{
    public interface IPaymentMethod
    {
        string Name { get; }
        string Process(OrderContext orderContext);
    }
}
