using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OrderLines;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class OrderLineService:IOrderLineService
    {
        private LetterAmazerEntities repository;
        private IOrderLineFactory orderLineFactory;

        public OrderLineService(IOrderLineFactory orderLineFactory, LetterAmazerEntities repository)
        {
            this.repository = repository;
            this.orderLineFactory = orderLineFactory;
        }

        public OrderLine Create(OrderLine orderLine)
        {
            var dbOrderLine = new DbOrderItems();
            dbOrderLine.Quantity = orderLine.Quantity;
            dbOrderLine.ItemType = (int)orderLine.ProductType;
            dbOrderLine.OrderId = orderLine.OrderId;

            if (orderLine.ProductType == ProductType.Order)
            {
                var letter = ((Letter)orderLine.BaseProduct);

                DbLetters dbLetter = new DbLetters()
                {
                    FromAddress_Address = letter.FromAddress.Address1,
                    FromAddress_Address2 = letter.FromAddress.Address2,
                    FromAddress_AttPerson = letter.FromAddress.AttPerson,
                    FromAddress_City = letter.FromAddress.City,
                    FromAddress_Co = letter.FromAddress.Co,
                    FromAddress_CompanyName = string.Empty,
                    FromAddress_Country = letter.FromAddress.Country.Id,
                    FromAddress_FirstName = letter.FromAddress.FirstName,
                    FromAddress_LastName = letter.FromAddress.LastName,
                    FromAddress_Postal = letter.FromAddress.PostalCode,
                    FromAddress_State = letter.FromAddress.State,
                    FromAddress_VatNr = letter.FromAddress.VatNr,
                    ToAddress_Address = letter.ToAddress.Address1,
                    ToAddress_Address2 = letter.ToAddress.Address2,
                    ToAddress_AttPerson = letter.ToAddress.AttPerson,
                    ToAddress_City = letter.ToAddress.City,
                    ToAddress_Co = letter.ToAddress.Co,
                    ToAddress_CompanyName = string.Empty,
                    ToAddress_Country = letter.ToAddress.Country.Id,
                    ToAddress_FirstName = letter.ToAddress.FirstName,
                    ToAddress_LastName = letter.ToAddress.LastName,
                    ToAddress_Postal = letter.ToAddress.PostalCode,
                    ToAddress_State = letter.ToAddress.State,
                    ToAddress_VatNr = letter.ToAddress.VatNr,
                    OrderId = letter.OrderId,
                    LetterContent_WrittenContent = letter.LetterContent.WrittenContent,
                    LetterContent_Content = letter.LetterContent.Content,
                    LetterContent_Path = letter.LetterContent.Path,
                    LetterStatus = (int)letter.LetterStatus,
                    OfficeProductId = letter.OfficeProductId,
                };
                dbOrderLine.DbLetters = dbLetter;
            }

            return GetOrderById(dbOrderLine.Id);
        }

        public OrderLine Update(OrderLine order)
        {
            throw new NotImplementedException();
        }

        public List<OrderLine> GetOrderBySpecification(OrderLineSpecification specification)
        {
            IQueryable<DbOrderItems> dbLines = repository.DbOrderItems;

            if (specification.OrderId > 0)
            {
                dbLines = repository.DbOrderItems.Where(c => c.OrderId == specification.OrderId);
            }

            return orderLineFactory.Create(dbLines.Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public OrderLine GetOrderById(int orderLineId)
        {
            var orderLineDb = repository.DbOrderItems.FirstOrDefault(c => c.Id == orderLineId);

            if (orderLineDb == null)
            {
                throw new BusinessException("Orderline doesn't exist");
            }

            return orderLineFactory.Create(orderLineDb);
        }

        public void Delete(OrderLine orderline)
        {
            throw new NotImplementedException();
        }
    }
}
