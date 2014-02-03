namespace LetterAmazer.Business.Services.Domain.Countries
{
    public interface ICountryService
    {

        void AddCountry(Country country);
        Country GetCountryById(int id);
    }
}
