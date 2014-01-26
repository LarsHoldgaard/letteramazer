using LetterAmazer.Business.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Model
{
    public class OrderContext
    {
        public Customer Customer { get; set; }
        public Order Order { get; set; }
        public bool SignUpNewsletter { get; set; }
        public string CurrentCulture { get; set; }
    }
}
