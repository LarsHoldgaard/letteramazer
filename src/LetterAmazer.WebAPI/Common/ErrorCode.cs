using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.WebAPI.Common
{
    public enum ErrorCode
    {
        //Common
        Success = 0,
        InvalidData = 1,
        GeneralError = 2,
        DatabaseError = 3,
        Timeout = 4,
        ConnectionError = 5,
        ValidationError = 6
    }
}