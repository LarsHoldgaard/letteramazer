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
            Skip = 0;
            Take = 100;
        }

        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
