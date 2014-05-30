using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Data.DTO
{
    public class CustomerSpecificationDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ResetPasswordKey { get; set; }
        public string RegistrationKey { get; set; }
        public int OrganisationId { get; set; }
    }
}
