using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Model.External_data
{
    public class CountryJson
    {
        public string countryName { get; set; }
        public string currencyCode { get; set; }
        public string fipsCode { get; set; }
        public string countryCode { get; set; }
        public string isoNumeric { get; set; }
        public float north { get; set; }
        public string capital { get; set; }
        public string continentName { get; set; }
        public string areaInSqKm { get; set; }
        public string languages { get; set; }
        public string isoAlpha3 { get; set; }
        public string continent { get; set; }
        public float south { get; set; }
        public float east { get; set; }
        public int geonameId { get; set; }
        public float west { get; set; }
        public string population { get; set; }
    }
}
