using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Model
{
    public class VerifyPaymentContext
    {
        public string Parameters { get; set; }
    }

    public class VerifyPaymentResult
    {
        public NameValueCollection Results { get; set; }
        public int OrderId { get; set; }
    }
}
