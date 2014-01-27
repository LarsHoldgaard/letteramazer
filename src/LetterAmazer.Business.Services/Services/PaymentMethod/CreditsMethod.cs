using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Services.PaymentMethod
{
    public class CreditsMethod : IPaymentMethod
    {
        public const string NAME = "Credits";
        private IRepository repository;
        private IUnitOfWork unitOfWork;
        public CreditsMethod(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        
        public string Name
        {
            get { return CreditsMethod.NAME; }
        }

        public string Process(OrderContext orderContext)
        {
            Order order = orderContext.Order;
            if (order == null || order.OrderItems == null || order.OrderItems.Count == 0) throw new BusinessException("Order can not be null!");
            if (order.Customer == null || order.Customer.Id == 0) throw new BusinessException("Customer can not be null!");

            order.Customer.Credits -= orderContext.Order.Price;
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
