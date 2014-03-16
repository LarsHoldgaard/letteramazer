using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Countries;

namespace LetterAmazer.Business.Services.Domain.Offices
{
    public class Office
    {
        public int Id { get; set; }

        public int FulfillmentPartnerId { get; set; }
        public string Title { get; set; }
        public Country Country { get; set; }
    }
}
