using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetterAmazer.Business.Services.Model
{
    public class PaginatedCriteria
    {
        public PaginatedCriteria()
        {
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int Start
        {
            get { return PageIndex * PageSize; }
        }
    }

    public class OrderedPaginatedCriteria : PaginatedCriteria
    {
        public IList<OrderBy> OrderBy { get; set; }

        public OrderedPaginatedCriteria()
        {
            this.OrderBy = new List<OrderBy>();
        }
    }

    public class OrderCriteria : OrderedPaginatedCriteria
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public int CustomerId { get; set; }
    }
}
