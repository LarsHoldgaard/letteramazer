using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class OfficeService:IOfficeService
    {
        private IOfficeFactory officeFactory;
        private LetterAmazerEntities repository;
        public OfficeService(LetterAmazerEntities repository, IOfficeFactory officeFactory)
        {
            this.repository = repository;
            this.officeFactory = officeFactory;
        }

        public Office GetOfficeById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id has to be above 0");
            }

            var dboffice = repository.DbOffices.FirstOrDefault(c => c.Id == id);

            if (dboffice == null)
            {
                throw new ItemNotFoundException("office");
            }

            var office = officeFactory.Create(dboffice);

            return office;
        }

        public List<Office> GetOfficeBySpecification(OfficeSpecification specification)
        {
            IQueryable<DbOffices> dboffices = repository.DbOffices;

            if (specification.CountryId > 0)
            {
                dboffices = dboffices.Where(c => c.CountryId == specification.CountryId);
            }
            if (specification.FulfilmentPartnerId > 0)
            {
                dboffices = dboffices.Where(c => c.PartnerId == specification.FulfilmentPartnerId);
            }
            return officeFactory.Create(dboffices.ToList());
        }
    }
}
