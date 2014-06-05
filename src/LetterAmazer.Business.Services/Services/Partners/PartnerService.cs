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
            if (specification.ValueId > 0)
            {
                dbPartnerTransactions = dbPartnerTransactions.Where(c => c.ValueId == specification.ValueId);
            }


            return partnerFactory.Create(dbPartnerTransactions.OrderBy(c => c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public PartnerTransaction Create(PartnerTransaction partnerTransaction)
        {
            var partnerT = new DbPartnerTransactions()
            {
                CustomerId = partnerTransaction.CustomerId,
                PartnerId = partnerTransaction.PartnerId,
                ValueId = partnerTransaction.ValueId,
                OrderId = partnerTransaction.OrderId,
                DateUpdated = DateTime.Now,
                DateCreated = DateTime.Now,
            };
            repository.DbPartnerTransactions.Add(partnerT);
            repository.SaveChanges();

            return GetPartnerTransactionById(partnerT.Id);
        }

        public PartnerAccess GetPartnerAccessById(int id)
        {
            var dbPartnerAccess = repository.DbPartnerAccess.FirstOrDefault(c => c.Id == id);

            return partnerFactory.Create(dbPartnerAccess);
        }

        public List<PartnerAccess> GetPartnerAccessBySpecification(PartnerAccessSpecification specification)
        {
            IQueryable<DbPartnerAccess> dbPartnerAccesses = repository.DbPartnerAccess;

            if (specification.UserId > 0)
            {
                dbPartnerAccesses = dbPartnerAccesses.Where(c => c.UserId == specification.UserId);
            }
            if (specification.PartnerId > 0)
            {
                dbPartnerAccesses = dbPartnerAccesses.Where(c => c.PartnerId == specification.PartnerId);
            }
            if (!string.IsNullOrEmpty(specification.Token))
            {
                dbPartnerAccesses = dbPartnerAccesses.Where(c => c.AccessToken == specification.Token);
            }
            return partnerFactory.Create(dbPartnerAccesses.OrderBy(c => c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public PartnerAccess Create(PartnerAccess partnerAccess)
        {
            var dbPartnerAccess = new DbPartnerAccess()
            {
                AccessToken = partnerAccess.AccessId,
                DateCreated = DateTime.Now,
                DateDeleted = null,
                PartnerId = partnerAccess.PartnerId,
                UserId = partnerAccess.UserId
            };
            repository.DbPartnerAccess.Add(dbPartnerAccess);
            repository.SaveChanges();

            return GetPartnerAccessById(dbPartnerAccess.Id);
        }
    }
}
