using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Utils.Enums;

namespace LetterAmazer.Business.Services.Data
{
    public class Letter
    {
        public int Id { get; set; }
        public AddressInfo FromAddress { get; set; }
        public AddressInfo ToAddress { get; set; }
        public Customer Customer { get; set; }
        public LetterStatus LetterStatus { get; set; }
        public LetterDetails LetterDetails { get; set; }
        public LetterContent LetterContent { get; set; }
    }
}
