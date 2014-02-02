using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services.FulfillmentPartners
{
    public class FulfillmentPartnerService : IFulfillmentPartnerService
    {
        private IRepository repository;
        private IUnitOfWork unitOfWork;
        public FulfillmentPartnerService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public List<FulfillmentPartner> GetFulfillmentPartnersBySpecifications(FulfillmentPartnerSpecification specification)
        {
            //if (specification.PrintSize != PrintSize.A4)
            //{
            //    return new List<FulfillmentPartner>();
            //}

            var partner = GetFulfillmentPartnerById(1);

            return new List<FulfillmentPartner>() { partner};

        }

        public FulfillmentPartner GetFulfillmentPartnerById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id has to be above 0");
            }

            FulfillmentPartner partner = repository.GetById<FulfillmentPartner>(id);
            if (partner == null)
            {
                throw new ItemNotFoundException("partner");
            }

            return partner;
        }
    }
}
