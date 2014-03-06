namespace LetterAmazer.Business.Services.Domain.Organisation
{
    public interface IOrganisationService
    {
        Organisation Create(Organisation organisation);
        Organisation Update(Organisation organisation);
        Organisation GetOrganisationById(int id);

        AddressList GetAddressListById(int id);
    }
}