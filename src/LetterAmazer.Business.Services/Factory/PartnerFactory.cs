using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class PartnerFactory:IPartnerFactory
    {
        public Partner Create(DbPartners partners)
        {
            return new Partner()
            {
                DateCreated = partners.DateCreated,
                Id = partners.Id,
                Guid = partners.Guid,
                PartnerType = (PartnerType)partners.PartnerType,
                Name = partners.Name,
                DateModified = partners.DateUpdated.HasValue ? partners.DateUpdated.Value : (DateTime?)null
            };
        }

        public List<Partner> Create(List<DbPartners> partners)
        {
            return partners.Select(Create).ToList();
        }

        public PartnerTransaction Create(DbPartnerTransactions partners)
        {
            return new PartnerTransaction()
            {
                CustomerId = partners.CustomerId,
                OrderId = partners.OrderId,
                PartnerId = partners.PartnerId, 
                Id = partners.Id,
                ValueId = partners.ValueId
            };
        }

        public List<PartnerTransaction> Create(List<DbPartnerTransactions> partners)
        {
            return partners.Select(Create).ToList();
        }
    }
}
