using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services.Offices
{
    public class OfficeService:IOfficeService
    {
        private OfficeFactory officeFactory;
        private LetterAmazerEntities repository;
        public OfficeService(LetterAmazerEntities repository,OfficeFactory officeFactory)
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
            //var data = repository.Where<IQueryable<Office>>(c=>c.FirstOrDefault().);
            return new List<Office>();
            //IQueryable<Office> officeList = repository.Offices;

            //if (specification.CountryId > 0)
            //{
            //    officeList = repository.Offices.Where(c => c.CountryId == specification.CountryId);
            //}
            //if (specification.LetterSize != null)
            //{
            //    officeList =
            //        officeList.Where(
            //            c => c.OfficeProducts.Any(d => d.OfficeProductDetail.Size == (int)specification.LetterSize));
            //}

            //return officeList.ToList();
        }
    }
}
