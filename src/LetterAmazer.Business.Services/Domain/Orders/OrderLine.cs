using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public class OrderLine
    {
        public ProductType ProductType { get; set; }
        public BaseProduct BaseProduct { get; set; }
        public int Quantity { get; set; }
    }
}
