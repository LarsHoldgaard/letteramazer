using LetterAmazer.Business.Services.Data;
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
            return "";
        }

        public VerifyPaymentResult Verify(Model.VerifyPaymentContext context)
        {
            return new VerifyPaymentResult();
        }
    }
}
