using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Utils.Enums
{
    public enum OrderStatus
    {
        Created=0,
        Paid=1,
        InProgress=2,
        Done=3,
        Error=4,
        Cancelled=5
    }
}
