using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OrderLines;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OrderLineFactory:IOrderLineFactory
    {
        private ILetterService letterService;

        public OrderLineFactory(ILetterService letterService)
        {
            this.letterService = letterService;
        }

        public List<OrderLine> Create(List<DbOrderItems> orderItemses)
        {
            return orderItemses.Select(Create).ToList();
        }
        public OrderLine Create(DbOrderItems dborderlines)
        {
            var line = new OrderLine()
            {
                Quantity = dborderlines.Quantity,
                ProductType = (ProductType)dborderlines.ItemType
            };

            if (line.ProductType == ProductType.Order && dborderlines.LetterId.HasValue)
            {
                line.BaseProduct = letterService.GetLetterById(dborderlines.LetterId.Value);
            }
            else if (line.ProductType == ProductType.Credits)
            {

            }

            return line;
        }
    }
}
