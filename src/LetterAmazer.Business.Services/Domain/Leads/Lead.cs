using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Leads
{
    public class Lead
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public LeadStatus LeadStatus { get; set; }

        public string OrganisationName { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }
        public string LettersPrWeek { get; set; }
        public string PhoneNumber { get; set; }

        public string CountryName { get; set; }
    }
}
