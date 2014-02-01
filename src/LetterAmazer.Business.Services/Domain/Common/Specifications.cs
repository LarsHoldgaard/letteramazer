using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Common
{
    public abstract class Specifications
    {
        protected Specifications()
        {
            Page = 1;
            Limit = 10;
        }

        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
