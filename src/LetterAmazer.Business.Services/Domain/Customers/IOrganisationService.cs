namespace LetterAmazer.Business.Services.Domain.Customers
{
    public interface IOrganisationService
    {
        Organisation Create(Organisation organisation);
        Organisation Update(Organisation organisation);
        Organisation GetOrganisationById(int id);
    }
}