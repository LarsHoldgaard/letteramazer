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

}
