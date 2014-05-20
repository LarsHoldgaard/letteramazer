using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Data.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        public AddressInfoDTO CustomerInfo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public DateTime? DateActivated { get; set; }
        public decimal Credit { get; set; }
        public decimal CreditLimit { get; set; }
        public string ResetPasswordKey { get; set; }
        public string RegisterKey { get; set; }
        public string AccountStatus { get; set; }
    }
}
