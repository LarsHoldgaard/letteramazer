using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class OfficeProductService : IOfferProductService
    {
        private LetterAmazerEntities repository;
        
        private IOfficeProductFactory officeProductFactory;

        public OfficeProductService(LetterAmazerEntities repository,IOfficeProductFactory officeProductFactory)
        {
            this.repository = repository;
            this.officeProductFactory = officeProductFactory;
        }

        public OfficeProduct GetOfficeProductById(int letterId)
        {
            throw new NotImplementedException();
        }

        public List<OfficeProduct> GetOfficeProductBySpecification(OfficeProductSpecification specification)
        {
            throw new NotImplementedException();
        }

        public OfficeProduct Create(OfficeProduct letter)
        {
            throw new NotImplementedException();
        }

        public OfficeProduct Update(OfficeProduct letter)
        {
            throw new NotImplementedException();
        }

        public void Delete(OfficeProduct letter)
        {
            throw new NotImplementedException();
        }
    }
}
