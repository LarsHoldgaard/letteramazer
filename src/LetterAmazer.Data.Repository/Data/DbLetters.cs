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
    
    public partial class DbLetters
    {
        public DbLetters()
        {
            this.DbOrderlines = new HashSet<DbOrderlines>();
        }
    
        public int Id { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string FromAddress_Address { get; set; }
        public string FromAddress_Address2 { get; set; }
        public string FromAddress_AttPerson { get; set; }
        public string FromAddress_City { get; set; }
        public string FromAddress_CompanyName { get; set; }
        public Nullable<int> FromAddress_Country { get; set; }
        public string FromAddress_FirstName { get; set; }
        public string FromAddress_LastName { get; set; }
        public string FromAddress_Zipcode { get; set; }
        public string FromAddress_VatNr { get; set; }
        public string ToAddress_Address { get; set; }
        public string ToAddress_Address2 { get; set; }
        public string ToAddress_AttPerson { get; set; }
        public string ToAddress_City { get; set; }
        public string ToAddress_CompanyName { get; set; }
        public int ToAddress_Country { get; set; }
        public string ToAddress_FirstName { get; set; }
        public string ToAddress_LastName { get; set; }
        public string ToAddress_Zipcode { get; set; }
        public string ToAddress_VatNr { get; set; }
        public string LetterContent_Path { get; set; }
        public string LetterContent_WrittenContent { get; set; }
        public byte[] LetterContent_Content { get; set; }
        public int LetterStatus { get; set; }
        public string ToAddress_State { get; set; }
        public string ToAddress_Co { get; set; }
        public string FromAddress_State { get; set; }
        public string FromAddress_Co { get; set; }
        public int OrderId { get; set; }
        public int LetterColor { get; set; }
        public int LetterPaperWeight { get; set; }
        public int LetterProcessing { get; set; }
        public int LetterSize { get; set; }
        public int LetterType { get; set; }
        public int OfficeId { get; set; }
    
        public virtual DbCountries DbCountries { get; set; }
        public virtual DbCountries DbCountries1 { get; set; }
        public virtual DbCustomers DbCustomers { get; set; }
        public virtual ICollection<DbOrderlines> DbOrderlines { get; set; }
    }
}
