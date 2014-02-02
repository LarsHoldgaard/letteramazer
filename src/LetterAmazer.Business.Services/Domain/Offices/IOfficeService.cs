using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Data;

namespace LetterAmazer.Business.Services.Domain.Offices
{
    public interface IOfficeService
    {
        Office GetOfficeById(int id);
        List<Office> GetOfficeBySpecification(OfficeSpecification specification);

    }
}
