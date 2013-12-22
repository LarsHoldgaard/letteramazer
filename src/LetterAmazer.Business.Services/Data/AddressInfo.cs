namespace LetterAmazer.Business.Services.Data
{
    public class AddressInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AttPerson { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Postal { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string VatNr { get; set; }
        public override string ToString()
        {
            return string.Format("{0}<br/>{1}<br/>{2}<br/>{3}",
                FirstName + LastName,
                Address + " " + Address2,
                Postal + City,
                Country);
        }
    }
}
