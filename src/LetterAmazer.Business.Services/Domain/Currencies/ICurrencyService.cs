using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Currencies
{
    public interface ICurrencyService
    {
        decimal Convert(decimal value, CurrencyCode fromCode, CurrencyCode toCode);
    }
}
