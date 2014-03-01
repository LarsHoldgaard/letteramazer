using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Customers
{
    public class CustomerSpecification:Specifications
    {
        public int  Id { get; set; }
        public string Email { get; set; }
        public string ResetPasswordKey { get; set; }
        public string RegistrationKey { get; set; }
        public int OrganisationId { get; set; }
        public CustomerRole? CustomerRole { get; set; }
    }
}
