using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Api
{
    public class ApiAccess
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public Role Role { get; set; }
        public int OrganisationId { get; set; }
    }
}
