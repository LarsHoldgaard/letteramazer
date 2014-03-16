using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface ILetterFactory
    {
        Letter Create(DbLetters dbLetter);
        List<Letter> Create(List<DbLetters> letterses);
    }
}