using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Leads;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class LeadService:ILeadService
    {
         
        private LetterAmazerEntities repository;
        private ILeadFactory leadFactory;

        public LeadService(ILeadFactory leadFactory, LetterAmazerEntities repository)
        {
            this.leadFactory = leadFactory;
            this.repository = repository;
        }

        public Lead Create(Lead lead)
        {
            var dbLead = new DbLeads()
            {
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Email = lead.Email ?? string.Empty,
                LeadStatus = (int)LeadStatus.NewLead,
                LettersPrWeek = lead.LettersPrWeek ?? string.Empty,
                Name = lead.Name ?? string.Empty,
                OrganisatioName = lead.OrganisationName ?? string.Empty,
                PhoneNumber = lead.PhoneNumber ?? string.Empty,
                CountryName = lead.CountryName ?? string.Empty
            };

            repository.DbLeads.Add(dbLead);
            repository.SaveChanges();

            return GetLeadById(dbLead.Id);
        }

        public Lead GetLeadById(int id)
        {
            var db_lead = repository.DbLeads.FirstOrDefault(c => c.Id == id);

            if (db_lead == null)
            {
                throw new ArgumentException("No lead by this id");
            }

            return leadFactory.Create(db_lead);
        }
    }
}
