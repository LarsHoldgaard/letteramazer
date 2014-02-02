using LetterAmazer.Business.Services.Data;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public interface ILetterService
    {
        decimal GetCost(Letter letter);
        string GetRelativeLetterStoragePath();
        Letter GetLetterById(int letterId);
    }
}
