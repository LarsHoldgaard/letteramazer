using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Leads;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class LeadFactory:ILeadFactory
    {
        public Lead Create(DbLeads dbLeads)
        {
            return new Lead()
            {
                DateCreated = dbLeads.DateCreated,
                DateUpdated = dbLeads.DateUpdated,
                Email =dbLeads.Email,
                Id = dbLeads.Id,
                LeadStatus = (LeadStatus)dbLeads.LeadStatus,
                LettersPrWeek = dbLeads.LettersPrWeek,
                Name = dbLeads.Name,
                OrganisationName = dbLeads.OrganisatioName,
                PhoneNumber = dbLeads.PhoneNumber,
                CountryName =dbLeads.CountryName
            };
        }

        public List<Lead> Create(List<DbLeads> dbLeads)
        {
            return dbLeads.Select(Create).ToList();
        }
    }
}
