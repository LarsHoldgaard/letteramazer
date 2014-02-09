using LetterAmazer.Business.Services.Domain.Countries;

namespace LetterAmazer.Business.Services.Domain.AddressInfos
{
    public class AddressInfo
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AttPerson { get; set; }
        public string Organisation { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostalCode { get; set; }
        public string VatNr { get; set; }
        public string Co { get; set; }
        public string State { get; set; }

    }
}
