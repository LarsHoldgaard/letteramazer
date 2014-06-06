using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Data.DTO
{
    public class AddressInfoDTO
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AttPerson { get; set; }
        public string Organisation { get; set; }
        public string City { get; set; }
        public CountryDTO Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Zipcode { get; set; }
        public string VatNr { get; set; }
        public string Co { get; set; }
        public string State { get; set; }
    }
}
