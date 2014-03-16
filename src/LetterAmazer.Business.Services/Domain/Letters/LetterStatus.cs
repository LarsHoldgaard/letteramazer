using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public enum LetterStatus
    {
        Created = 0,
        Sent = 1,
        Returned = 2,
        Cancelled = 3,
        InProgress=4
    }
}
