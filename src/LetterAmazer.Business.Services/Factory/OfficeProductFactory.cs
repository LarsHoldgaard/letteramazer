using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OfficeProductFactory : IOfficeProductFactory
    {
        public OfficeProductFactory()
        {
            
        }

        public OfficeProduct Create(DbOfficeProducts products)
        {
            return new OfficeProduct();
        }

        public List<OfficeProduct> Create(List<DbOfficeProducts> products)
        {
            return products.Select(Create).ToList();
        }



    }
}
