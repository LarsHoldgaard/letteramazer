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
    
    public partial class DbCustomers
    {
        public DbCustomers()
        {
            this.DbLetters = new HashSet<DbLetters>();
            this.DbOrders = new HashSet<DbOrders>();
        }
    
        public int Id { get; set; }
        public string CustomerInfo_Address { get; set; }
        public string CustomerInfo_Address2 { get; set; }
        public string CustomerInfo_AttPerson { get; set; }
        public string CustomerInfo_City { get; set; }
        public string CustomerInfo_CompanyName { get; set; }
        public Nullable<int> CustomerInfo_Country { get; set; }
        public string CustomerInfo_FirstName { get; set; }
        public string CustomerInfo_LastName { get; set; }
        public string CustomerInfo_Postal { get; set; }
        public string CustomerInfo_VatNr { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public Nullable<decimal> Credits { get; set; }
        public decimal CreditLimit { get; set; }
        public string ResetPasswordKey { get; set; }
        public string Phone { get; set; }
        public string CustomerInfo_State { get; set; }
        public string CustomerInfo_Co { get; set; }
        public Nullable<System.DateTime> DateActivated { get; set; }
    
        public virtual DbCountries DbCountries { get; set; }
        public virtual ICollection<DbLetters> DbLetters { get; set; }
        public virtual ICollection<DbOrders> DbOrders { get; set; }
    }
}
