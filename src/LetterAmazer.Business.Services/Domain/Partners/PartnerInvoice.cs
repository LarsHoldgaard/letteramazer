using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Partners
{

    public class PartnerInvoice
    {
        public string Id { get; set; }
        public string PdfUrl { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
