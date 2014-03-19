using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Currencies;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class CurrencyService:ICurrencyService
    {
        private LetterAmazerEntities repository;

        public CurrencyService(LetterAmazerEntities repository)
        {
            this.repository = repository;
        }

        public decimal Convert(decimal value, CurrencyCode fromCode, CurrencyCode toCode)
        {
            var exchangeRateFrom = repository.DbExchangeRates.Where(c => c.CurrencyId == (int)fromCode).OrderByDescending(c=>c.DateFetched).FirstOrDefault();
            var exchangeRateTo = repository.DbExchangeRates.Where(c => c.CurrencyId == (int)toCode).OrderByDescending(c => c.DateFetched).FirstOrDefault();

            if (exchangeRateFrom == null || exchangeRateTo == null)
            {
                throw new ArgumentException("Selected currency has no exchange rates in table");
            }

            return (value*exchangeRateFrom.ExchangeRate)/exchangeRateTo.ExchangeRate;
        }
    }
}
