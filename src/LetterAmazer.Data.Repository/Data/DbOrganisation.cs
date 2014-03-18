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
    
    public partial class DbOrganisation
    {
        public DbOrganisation()
        {
            this.DbOrganisationAddressList = new HashSet<DbOrganisationAddressList>();
            this.DbOrganisationProfileSettings = new HashSet<DbOrganisationProfileSettings>();
            this.DbApiAccess = new HashSet<DbApiAccess>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Address1 { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Address2 { get; set; }
        public string AttPerson { get; set; }
        public string State { get; set; }
        public System.Guid Guid { get; set; }
        public bool IsPrivate { get; set; }
        public Nullable<int> RequiredFulfillmentPartnerId { get; set; }
        public Nullable<int> RequiredOfficeId { get; set; }
    
        public virtual ICollection<DbOrganisationAddressList> DbOrganisationAddressList { get; set; }
        public virtual ICollection<DbOrganisationProfileSettings> DbOrganisationProfileSettings { get; set; }
        public virtual ICollection<DbApiAccess> DbApiAccess { get; set; }
    }
}
