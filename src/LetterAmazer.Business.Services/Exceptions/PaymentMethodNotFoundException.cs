using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Exceptions
{
    [global::System.Serializable]
    public class PaymentMethodNotFoundException : Exception
    {
        public PaymentMethodNotFoundException() { }
        public PaymentMethodNotFoundException(string message) : base(message) { }
        public PaymentMethodNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected PaymentMethodNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
