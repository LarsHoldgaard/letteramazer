using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.OrderLines;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IOrderLineFactory
    {
        List<OrderLine> Create(List<DbOrderItems> orderItemses);
        OrderLine Create(DbOrderItems dborderlines);
    }
}
