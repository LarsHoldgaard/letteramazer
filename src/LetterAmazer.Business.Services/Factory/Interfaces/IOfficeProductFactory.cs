using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IOfficeProductFactory
    {
        OfficeProduct Create(DbOfficeProducts products);
        List<OfficeProduct> Create(List<DbOfficeProducts> products);
    }
}