using System.Collections.Generic;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public interface ILetterService
    {
        string GetRelativeLetterStoragePath();
        Letter GetLetterById(int letterId);
        List<Letter> GetLetterBySpecification(LetterSpecification specification);

    }
}
