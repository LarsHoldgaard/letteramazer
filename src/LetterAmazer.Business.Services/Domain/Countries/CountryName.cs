using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Countries
{
    public class CountryName
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CountryId { get; set; }
        public string Language { get; set; }
    }
}
