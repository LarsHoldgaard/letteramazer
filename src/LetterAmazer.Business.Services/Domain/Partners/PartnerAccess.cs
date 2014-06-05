using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Partners
{
    public class PartnerAccess
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PartnerId { get; set; }
        public string AccessId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
