using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public class PaymentMethods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDeleted { get; set; }

        public int MinimumAmount { get; set; }
        public int MaximumAmount { get; set; }
        public decimal Price { get; set; }

        public int SortOrder { get; set; }
        public bool IsVisible { get; set; }

        public bool RequiresLogin { get; set; }
    }
}
