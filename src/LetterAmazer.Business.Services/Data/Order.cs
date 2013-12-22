using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Utils.Enums;

namespace LetterAmazer.Business.Services.Data
{
    public class Order
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public List<Letter> Letters { get; set; }
        public Customer Customer { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }


    }
}
