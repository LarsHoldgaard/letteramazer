using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Services.PaymentMethods.Implementations;

namespace LetterAmazer.Business.Services.Services
{
    public class PaymentService : IPaymentService
    {
        public void Process(Domain.Payments.PaymentMethods method, Order order)
        {
            IPaymentMethod selectedPaymentMethod = null;
            if (method == Domain.Payments.PaymentMethods.Credits)
            {
                selectedPaymentMethod = new CreditsMethod();
            }
            else if (method == Domain.Payments.PaymentMethods.Bitcoin)
            {
                selectedPaymentMethod = new BitcoinMethod();
            }
            else
            {
                selectedPaymentMethod = new PaypalMethod();
            }

            selectedPaymentMethod.Process(order);

        }

        public void ProcessFunds(Domain.Payments.PaymentMethods method, Customer customer, decimal value)
        {
            IPaymentMethod selectedPaymentMethod = null;
            if (method == Domain.Payments.PaymentMethods.Bitcoin)
            {
                selectedPaymentMethod = new CreditsMethod();
            }
            else if (method == Domain.Payments.PaymentMethods.Invoice)
            {
                selectedPaymentMethod = new BitcoinMethod();
            }
            else
            {
                selectedPaymentMethod = new PaypalMethod();
            }

            selectedPaymentMethod.ProcessFunds(customer,value);
        }

        public List<Domain.Payments.PaymentMethods> GetPaymentMethodsBySpecification(PaymentMethodSpecification specification)
        {
            var methods = new List<Domain.Payments.PaymentMethods>
            {
                Domain.Payments.PaymentMethods.Bitcoin,
                Domain.Payments.PaymentMethods.Invoice,
                Domain.Payments.PaymentMethods.PayPal
            };

            return methods;
        }
    }
}
