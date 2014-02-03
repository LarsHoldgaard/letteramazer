using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services.PaymentMethod
{
    public class CreditsMethod : IPaymentMethod
    {
        public const string NAME = "Credits";
        private LetterAmazerEntities Repository;
        public CreditsMethod(LetterAmazerEntities repository)
        {
            this.Repository = repository;
        }
        
        public string Name
        {
            get { return CreditsMethod.NAME; }
        }

        public string Process(OrderContext orderContext)
        {
            Order order = orderContext.Order;
            if (order == null || order.Letters == null || order.Letters.Count == 0) throw new BusinessException("Order can not be null!");
            if (order.Customer == null || order.Customer.Id == 0) throw new BusinessException("Customer can not be null!");

            order.Customer.Credit -= orderContext.Order.Price;
            order.OrderStatus = OrderStatus.Paid;
            repository.Update(order);
            order.Customer.DateUpdated = DateTime.Now;
            repository.Update(order.Customer);
            unitOfWork.Commit();
            return string.Format("/{0}/singleletter/confirmation", orderContext.CurrentCulture);
        }

        public VerifyPaymentResult Verify(Model.VerifyPaymentContext context)
        {
            return new VerifyPaymentResult();
        }
    }
}
