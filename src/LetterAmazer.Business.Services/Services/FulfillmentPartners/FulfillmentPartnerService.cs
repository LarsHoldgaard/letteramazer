using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services.FulfillmentPartners
{
    public class FulfillmentPartnerService : IFulfillmentPartnerService
    {
        private FulfilmentPartnerFactory fulfilmentPartnerFactory;
        private LetterAmazerEntities repository;
        public FulfillmentPartnerService(LetterAmazerEntities repoEntities, FulfilmentPartnerFactory factory)
        {
            this.repository = repoEntities;
            this.fulfilmentPartnerFactory = factory;
        }

        public List<FulfilmentPartner> GetFulfillmentPartnersBySpecifications(FulfillmentPartnerSpecification specification)
        {
            //if (specification.PrintSize != PrintSize.A4)
            //{
            //    return new List<FulfillmentPartner>();
            //}

            var partner = GetFulfillmentPartnerById(1);

            return new List<FulfilmentPartner>() { partner };

        }

        public FulfilmentPartner GetFulfillmentPartnerById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id has to be above 0");
            }

            var dbPartner = repository.DbFulfillmentPartners.FirstOrDefault(c=>c.Id ==id);
            if (dbPartner == null)
            {
                throw new ItemNotFoundException("partner");
            }

            var partner = this.fulfilmentPartnerFactory.Create(dbPartner);

            return partner;
        }
    }
}
