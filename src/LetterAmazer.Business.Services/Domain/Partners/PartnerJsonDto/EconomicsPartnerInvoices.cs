using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Partners.PartnerJsonDto
{

    public class EconomicsPartnerInvoices
    {
        public EconomicInvoice[] collection { get; set; }
        public Metadata metaData { get; set; }
        public Pagination pagination { get; set; }
        public string self { get; set; }
        
    }

    public class Metadata
    {
    }

    public class Pagination
    {
        public int maxPageSizeAllowed { get; set; }
        public int skipPages { get; set; }
        public int pageSize { get; set; }
        public int results { get; set; }
        public int resultsWithoutFilter { get; set; }
        public string firstPage { get; set; }
        public string nextPage { get; set; }
        public string lastPage { get; set; }
    }

    public class EconomicInvoice
    {
        public string orderId { get; set; }
        public string id { get; set; }
        public DateTime date { get; set; }
        public DateTime dueDate { get; set; }
       
        public string pdf { get; set; }
       
        public int attention { get; set; }
       
        public DateTime deliveryDate { get; set; }
        public string customerName { get; set; }
        public string customerCountry { get; set; }
        public string customerCity { get; set; }

        public string customerAddress { get; set; }
        public string customerPostalCode { get; set; }
        public string customerCounty { get; set; }

        public decimal netAmount { get; set; }
        public decimal vatAmount { get; set; }
        public string self { get; set; }
    }

    public class EconomicCustomer
    {
        public int customerNumber { get; set; }
    }

    public class EconomicOurprimaryreference
    {
        public string self { get; set; }
    }

    public class EconomicLayout
    {
        public string name { get; set; }
        public string self { get; set; }
    }


}
