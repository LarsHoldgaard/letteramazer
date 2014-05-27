using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Data.DTO
{
    public class OrderLineDTO
    {
        public string ProductType { get; set; }
       // public BaseItem BaseProduct { get; set; }


        public int PaymentMethodId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
