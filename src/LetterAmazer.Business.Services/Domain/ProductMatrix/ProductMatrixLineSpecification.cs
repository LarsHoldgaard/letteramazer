using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.ProductMatrix
{
    public class ProductMatrixLineSpecification:Specifications
    {
        public int OfficeProductId { get; set; }

        public int PageCount { get; set; }

        public ProductMatrixLineSpecification()
        {
            this.PageCount = 1;
        }
    }
}
