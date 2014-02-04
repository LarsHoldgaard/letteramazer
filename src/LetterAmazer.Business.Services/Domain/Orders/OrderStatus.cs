using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public enum OrderStatus
    {
        Created=0,
        Paid=1,
        InProgress=2,
        Done=3,
        Cancelled=4
    }
}
