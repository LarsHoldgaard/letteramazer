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
    
    public partial class DbProductMatrixLines
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int LineType { get; set; }
        public decimal BaseCost { get; set; }
        public int PriceType { get; set; }
        public int Span_lower { get; set; }
        public int Span_upper { get; set; }
        public int OfficeProductId { get; set; }
        public int CurrencyId { get; set; }
    
        public virtual DbOfficeProducts DbOfficeProducts { get; set; }
    }
}
