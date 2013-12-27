using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private IDictionary<string, IPaymentMethod> methods;

        public PaymentService(IPaymentMethod[] methods)
        {
            this.methods = new Dictionary<string, IPaymentMethod>();
            foreach (var method in methods)
            {
                Register(method);
            }
        }

        private void Register(IPaymentMethod method)
        {
            if (this.methods.ContainsKey(method.Name)) throw new PaymentMethodDuplicatedException();
            this.methods[method.Name] = method;
        }

        public string Process(Model.OrderContext orderContext)
        {
            return this.methods[orderContext.Order.PaymentMethod].Process(orderContext);
        }

        public IPaymentMethod Get(string methodName)
        {
            if (!this.methods.ContainsKey(methodName)) throw new PaymentMethodNotFoundException();
            return this.methods[methodName];
        }
    }
}
