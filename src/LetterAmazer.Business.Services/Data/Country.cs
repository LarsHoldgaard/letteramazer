//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LetterAmazer.Business.Services.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Country
    {
        public Country()
        {
            this.OfficeProducts = new HashSet<OfficeProduct>();
        }
    
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string FipsCode { get; set; }
        public string CountryCode { get; set; }
        public string IsoNumeric { get; set; }
        public Nullable<double> North { get; set; }
        public string Capital { get; set; }
        public string ContinentName { get; set; }
        public string AreaInSqKm { get; set; }
        public string Languages { get; set; }
        public string IsoAlpha3 { get; set; }
        public string Continent { get; set; }
        public Nullable<double> South { get; set; }
        public Nullable<double> East { get; set; }
        public Nullable<int> GeonameId { get; set; }
        public Nullable<double> West { get; set; }
        public string Population { get; set; }
        public Nullable<bool> InsideEu { get; set; }
    
        public virtual ICollection<OfficeProduct> OfficeProducts { get; set; }
        public virtual Office Office { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
