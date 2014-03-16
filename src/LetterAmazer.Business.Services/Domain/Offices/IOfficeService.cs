using System.Collections.Generic;

namespace LetterAmazer.Business.Services.Domain.Offices
{
    public interface IOfficeService
    {
        Office GetOfficeById(int id);
        List<Office> GetOfficeBySpecification(OfficeSpecification specification);

    }
}
