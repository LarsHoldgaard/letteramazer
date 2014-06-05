using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Partners
{
    public class PartnerAccessSpecification:Specifications
    {
        public int UserId { get; set; }
        public int PartnerId { get; set; }
        public string Token { get; set; }
    }
}
