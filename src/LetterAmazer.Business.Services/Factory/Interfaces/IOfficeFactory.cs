using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IOfficeFactory
    {
        Office Create(DbOffices dbOffices);
        List<Office> Create(List<DbOffices> dbOfficeses);
    }
}