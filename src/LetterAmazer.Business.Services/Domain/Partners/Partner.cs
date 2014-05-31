using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Partners
{
    public class Partner
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public PartnerType PartnerType { get; set; }
    }
}
