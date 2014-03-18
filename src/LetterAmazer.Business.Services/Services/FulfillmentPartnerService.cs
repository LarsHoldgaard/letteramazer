using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class FulfillmentPartnerService : IFulfillmentPartnerService
    {
        private IFulfilmentPartnerFactory fulfilmentPartnerFactory;
        private LetterAmazerEntities repository;
        public FulfillmentPartnerService(LetterAmazerEntities repoEntities, IFulfilmentPartnerFactory factory)
        {
            this.repository = repoEntities;
            this.fulfilmentPartnerFactory = factory;
        }

        public List<FulfilmentPartner> GetFulfillmentPartnersBySpecifications(FulfillmentPartnerSpecification specification)
        {
            IQueryable<DbFulfillmentPartners> dbPartners = repository.DbFulfillmentPartners;

            if (specification.ShopId > 0)
            {
                dbPartners = dbPartners.Where(c => c.ShopId == specification.ShopId);
            }

            var partners =
                fulfilmentPartnerFactory.Create(
                    dbPartners.OrderBy(c => c.Id).Skip(specification.Skip).Take(specification.Take).ToList());

            return partners;
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

        public FulfilmentPartner Create(FulfilmentPartner partner)
        {
            var dbpartner = new DbFulfillmentPartners();
            dbpartner.Name = partner.Name;
            dbpartner.ShopId = partner.ShopId;

            repository.DbFulfillmentPartners.Add(dbpartner);
            repository.SaveChanges();

            return GetFulfillmentPartnerById(dbpartner.Id);
        }
    }
}
