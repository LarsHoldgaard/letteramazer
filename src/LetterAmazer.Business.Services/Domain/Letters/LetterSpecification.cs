using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public class LetterSpecification:Specifications
    {
        public LetterSpecification()
        {
            this.LetterStatus = new List<LetterStatus>();
        }


        public int Id { get; set; }
        public int OrderId { get; set; }
        public List<LetterStatus> LetterStatus { get; set; }
    }
}
