﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Countries
{
    public class Country
    {
        public string Name { get; set; }
        public string Continent { get; set; }
        public string CurrencyCode { get; set; }
        public string CountryCode { get; set; }
        public string Capital { get; set; }
        public bool InsideEu { get; set; }
        public string Population { get; set; }
        public string ArealInSqKm { get; set; }
        public string Fipscode { get; set; }
    }
}
