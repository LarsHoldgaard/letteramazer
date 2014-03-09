using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.AddressInfos;

namespace LetterAmazer.Business.Services.Domain.Organisation
{
    public class AddressList
    {
        public int Id { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public int SortIndex { get; set; }

        public int OrganisationId { get; set; }


        public AddressList()
        {
            this.AddressInfo = new AddressInfo();
        }
    }
}
