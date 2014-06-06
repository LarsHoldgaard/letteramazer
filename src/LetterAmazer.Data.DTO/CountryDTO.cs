using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Data.DTO
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public string CountryCode { get; set; }
        public string Capital { get; set; }
        public bool InsideEu { get; set; }
        public int Population { get; set; }
        public string ArealInSqKm { get; set; }
        public string Fipscode { get; set; }
        public int ContinentId { get; set; }

        public bool Enabled { get; set; }

        public decimal? VatPercentage { get; set; }
    }
}
