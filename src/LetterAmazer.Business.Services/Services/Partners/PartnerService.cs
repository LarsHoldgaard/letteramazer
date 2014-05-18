using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services.Partners
{
    public class PartnerService:IPartnerService
    {

        private IPartnerFactory partnerFactory;
        

        private LetterAmazerEntities repository;
        public PartnerService(LetterAmazerEntities repository,IPartnerFactory partnerFactory)
        {
            this.partnerFactory = partnerFactory;
            this.repository = repository;
        }


        public Partner GetPartnerById(int id)
        {
            var dbPartner = repository.DbPartners.FirstOrDefault(c => c.Id == id);

            if (dbPartner == null)
            {
                throw new ArgumentException("No partner by this ID");
            }

            var partner = partnerFactory.Create(dbPartner);
            return partner;
        }

        public PartnerTransaction GetPartnerTransactionById(int id)
        {
            var dbPartnerTransaction = repository.DbPartnerTransactions.FirstOrDefault(c => c.Id == id);

            if (dbPartnerTransaction == null)
            {
                throw new ArgumentException("No partner by this ID");
            }

            var partner = partnerFactory.Create(dbPartnerTransaction);
            return partner;
        }

        public List<PartnerTransaction> GetPartnerTransactionBySpecification(PartnerTransactionSpecification specification)
        {
            IQueryable<DbPartnerTransactions> dbPartnerTransactions = repository.DbPartnerTransactions;

            if (specification.CustomerId > 0)
            {
                dbPartnerTransactions = dbPartnerTransactions.Where(c => c.CustomerId == specification.CustomerId);
            }
            if (specification.OrderId > 0)
            {
                dbPartnerTransactions = dbPartnerTransactions.Where(c => c.OrderId == specification.OrderId);
            }
            if (specification.PartnerId > 0)
            {
                dbPartnerTransactions = dbPartnerTransactions.Where(c => c.PartnerId == specification.PartnerId);
            }

            return partnerFactory.Create(dbPartnerTransactions.OrderBy(c => c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }
    }
}
