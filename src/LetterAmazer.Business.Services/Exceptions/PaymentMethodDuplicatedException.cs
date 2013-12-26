using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Exceptions
{
    [global::System.Serializable]
    public class PaymentMethodDuplicatedException : Exception
    {
        public PaymentMethodDuplicatedException() { }
        public PaymentMethodDuplicatedException(string message) : base(message) { }
        public PaymentMethodDuplicatedException(string message, Exception inner) : base(message, inner) { }
        protected PaymentMethodDuplicatedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
