using System;

namespace LetterAmazer.Business.Services.Data
{
    public interface IDBFactory : IDisposable
    {
        LetterAmazerContext Get();
    }
}
