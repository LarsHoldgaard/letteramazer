using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Utils.Enums
{
    public enum LetterStatus
    {
        Created=0,
        Sending=1,
        Sent=2,
        Returned=3,
        Cancelled=4
    }
}
