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
    
    public partial class DbPartners
    {
        public DbPartners()
        {
            this.DbPartnerTransactions = new HashSet<DbPartnerTransactions>();
        }
    
        public int Id { get; set; }
        public System.Guid Guid { get; set; }
        public string Name { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public int PartnerType { get; set; }
    
        public virtual ICollection<DbPartnerTransactions> DbPartnerTransactions { get; set; }
    }
}