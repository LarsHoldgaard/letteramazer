using LetterAmazer.Business.Services.Domain.AddressInfos;

namespace LetterAmazer.Business.Services.Domain.Customers
{
    public class AddressList
    {
        public int Id { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public int SortIndex { get; set; }

    }
}
