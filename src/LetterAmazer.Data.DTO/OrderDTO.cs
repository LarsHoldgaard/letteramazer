using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Data.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string OrderCode { get; set; }
        public string OrderStatus { get; set; }
        public CustomerDTO Customer { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DatePaid { get; set; }

        public DateTime? DateSent { get; set; }

        public string TransactionCode { get; set; }


        public List<PartnerTransactionDTO> PartnerTransactions { get; set; }

        public List<OrderLineDTO> OrderLines { get; set; }
    }
}
