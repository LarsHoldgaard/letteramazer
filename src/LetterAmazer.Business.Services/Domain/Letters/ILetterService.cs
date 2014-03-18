using System.Collections.Generic;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public interface ILetterService
    {
        Letter GetLetterById(int letterId);
        List<Letter> GetLetterBySpecification(LetterSpecification specification);
        Letter Create(Letter letter);
        Letter Update(Letter letter);
        void Delete(Letter letter);
    }
}
