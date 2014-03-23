using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Content
{
    public class ContentSpecification:Specifications
    {
        public string Alias { get; set; }
        public string Section { get; set; }
    }
}
