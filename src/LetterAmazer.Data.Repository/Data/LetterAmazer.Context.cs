﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LetterAmazerEntities : DbContext
    {
        public LetterAmazerEntities()
            : base("name=LetterAmazerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<DbContinents> DbContinents { get; set; }
        public DbSet<DbCountries> DbCountries { get; set; }
        public DbSet<DbCountryNames> DbCountryNames { get; set; }
        public DbSet<DbCoupons> DbCoupons { get; set; }
        public DbSet<DbCustomers> DbCustomers { get; set; }
        public DbSet<DbFulfillmentPartners> DbFulfillmentPartners { get; set; }
        public DbSet<DbInvoiceLines> DbInvoiceLines { get; set; }
        public DbSet<DbInvoices> DbInvoices { get; set; }
        public DbSet<DbLetters> DbLetters { get; set; }
        public DbSet<DbOfficeProducts> DbOfficeProducts { get; set; }
        public DbSet<DbOffices> DbOffices { get; set; }
        public DbSet<DbOrderlines> DbOrderlines { get; set; }
        public DbSet<DbOrders> DbOrders { get; set; }
        public DbSet<DbOrganisation> DbOrganisation { get; set; }
        public DbSet<DbOrganisationAddressList> DbOrganisationAddressList { get; set; }
        public DbSet<DbOrganisationProfileSettings> DbOrganisationProfileSettings { get; set; }
        public DbSet<DbPaymentMethods> DbPaymentMethods { get; set; }
        public DbSet<DbProductMatrixLines> DbProductMatrixLines { get; set; }
        public DbSet<DbShops> DbShops { get; set; }
        public DbSet<DbLog> DbLog { get; set; }
        public DbSet<DbApiAccess> DbApiAccess { get; set; }
    }
}
