using System;
using System.Linq;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class CreditsMethod : IPaymentMethod
    {
        //public const string NAME = "Credits";
        //private LetterAmazerEntities Repository;
        //public CreditsMethod(LetterAmazerEntities repository)
        //{
        //    this.Repository = repository;
        //}
        
        //public string Name
        //{
        //    get { return CreditsMethod.NAME; }
        //}

        //public string Process(OrderContext orderContext)
        //{
        //    Order order = orderContext.Order;
        //    if (order == null || order.Letters == null || order.Letters.Count == 0) throw new BusinessException("Order can not be null!");
        //    if (order.Customer == null || order.Customer.Id == 0) throw new BusinessException("Customer can not be null!");

        //    var dbOrder = Repository.DbOrders.FirstOrDefault(c => c.Id == order.Id);
        //    if (dbOrder == null)
        //    {
        //        throw new BusinessException("Order cannot be null");
        //    }

        //    dbOrder.OrderStatus = (int)OrderStatus.Paid;

        //    var dbCustomer = Repository.DbCustomers.FirstOrDefault(c => c.Id == order.Customer.Id);

        //    if (dbCustomer == null)
        //    {
        //        throw new Exception();
        //    }
        //    dbCustomer.Credits -= orderContext.Order.Price;

        //    Repository.SaveChanges();
        //    return string.Format("/{0}/singleletter/confirmation", orderContext.CurrentCulture);
        //}

        //public VerifyPaymentResult Verify(Model.VerifyPaymentContext context)
        //{
        //    return new VerifyPaymentResult();
        //}

        private ICustomerService customerService;

        public CreditsMethod(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public string Process(Order order)
        {
            var orderLine = order.OrderLines.FirstOrDefault(c => c.ProductType == ProductType.Payment && c.PaymentMethodId == 2);

            if (orderLine == null)
            {
                throw new BusinessException("Credits line wasn't found on order");
            }

            var customer = customerService.GetCustomerById(order.Customer.Id);

            customer.Credit -= orderLine.Price.PriceExVat*orderLine.Quantity;

            customerService.Update(customer);

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
