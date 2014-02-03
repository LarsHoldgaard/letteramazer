using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;

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
