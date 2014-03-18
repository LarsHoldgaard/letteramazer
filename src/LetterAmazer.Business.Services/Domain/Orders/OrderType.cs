using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public enum OrderType
    {
        SingleLetter=0,
        LoggedInLetter=1,
        ApiLetter=2,
        Credits=3
    }
}
