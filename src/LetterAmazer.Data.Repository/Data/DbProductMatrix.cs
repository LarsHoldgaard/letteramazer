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
    
    public partial class DbProductMatrix
    {
        public DbProductMatrix()
        {
            this.DbProductMatrixLines = new HashSet<DbProductMatrixLines>();
        }
    
        public int Id { get; set; }
        public int PriceType { get; set; }
        public int Span_lower { get; set; }
        public int Span_upper { get; set; }
        public int ValueId { get; set; }
        public int ReferenceType { get; set; }
    
        public virtual ICollection<DbProductMatrixLines> DbProductMatrixLines { get; set; }
    }
}
