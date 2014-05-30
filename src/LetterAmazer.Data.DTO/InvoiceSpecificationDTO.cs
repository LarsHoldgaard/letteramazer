using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Data.DTO
{
    public class InvoiceSpecificationDTO
    {
        public int OrganisationId { get; set; }
        public int OrderId { get; set; }

        public string InvoiceStatus { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
