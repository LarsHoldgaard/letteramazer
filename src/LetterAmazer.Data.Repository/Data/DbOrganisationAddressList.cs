//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LetterAmazer.Data.Repository.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class DbOrganisationAddressList
    {
        public int Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
        public int OrganisationId { get; set; }
        public int OrderIndex { get; set; }
    
        public virtual DbCountries DbCountries { get; set; }
        public virtual DbOrganisation DbOrganisation { get; set; }
    }
}
