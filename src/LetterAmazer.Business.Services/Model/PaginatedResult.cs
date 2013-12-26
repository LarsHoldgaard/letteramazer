using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetterAmazer.Business.Services.Model
{
    public class PaginatedResult<T>
    {
        public IList<T> Results { get; set; }
        public long TotalItems { get; set; }

        public PaginatedResult()
        {
            this.Results = new List<T>();
        }
    }

    public class PaginatedInfo<T> : PaginatedResult<T>
    {
        public PaginatedInfo()
        {
        }

        public PaginatedInfo(int pageIndex, int pageSize, PaginatedResult<T> result)
            : this(result)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public PaginatedInfo(PaginatedResult<T> result)
        {
            this.Results = result.Results;
            this.TotalItems = result.TotalItems;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
