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
    
    public partial class DbCmsContent
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string SeoTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Section { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public bool Enabled { get; set; }
        public string CmsKey { get; set; }
        public string Alias { get; set; }
    }
}