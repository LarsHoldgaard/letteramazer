using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Leads;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface ILeadFactory
    {
        Lead Create(DbLeads dbLeads);
        List<Lead> Create(List<DbLeads> dbLeads);

    }
}
