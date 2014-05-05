using System;
using System.Linq;
using System.Web.Security;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Utils.Helpers;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class CreditsMethod : IPaymentMethod
    {

        private IOrderService orderService;
        private ICustomerService customerService;

        public CreditsMethod(ICustomerService customerService, IOrderService orderService)
        {
            this.customerService = customerService;
            this.orderService = orderService;
        }

        public string Process(Order order)
        {
            var orderLine = order.OrderLines.FirstOrDefault(c => c.ProductType == ProductType.Payment && c.PaymentMethodId == 2);

            if (orderLine == null)
            {
                throw new BusinessException("Credits line wasn't found on order");
            }
            
            var customer = customerService.GetCustomerById(order.Customer.Id);

            if (order.Customer.CreditsLeft < order.Price.Total)
            {
                throw new BusinessException("User does not have enough credits to purchase this order");
            }

            customer.Credit -= orderLine.Price.Total*orderLine.Quantity;

            var updated_customer = customerService.Update(customer);

            order.OrderStatus =OrderStatus.Paid;
            orderService.Update(order);

            SessionHelper.Customer = updated_customer;
            FormsAuthentication.SetAuthCookie(customer.Id.ToString(), true);

            return string.Empty;
        }

        public void VerifyPayment(Order order)
        {
            throw new NotImplementedException();
        }

        public void ChargeBacks(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
