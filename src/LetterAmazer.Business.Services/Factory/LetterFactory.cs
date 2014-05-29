using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class LetterFactory : ILetterFactory
    {
        private ICustomerService _customerService;
        private IAddressFactory AddressFactory;

        public LetterFactory(ICustomerService customerService, IAddressFactory addressFactory)
        {
            this._customerService = customerService;
            this.AddressFactory = addressFactory;
        }

        public Letter Create(DbLetters dbLetter)
        {
            AddressInfo fromAddress = new AddressInfo();

            var letter = new Letter()
            {
                LetterContent = new LetterContent()
                {
                    Path = dbLetter.LetterContent_Path,
                    WrittenContent = dbLetter.LetterContent_WrittenContent,
                },
                LetterStatus = (LetterStatus)(dbLetter.LetterStatus),
                Id = dbLetter.Id,
                ToAddress =  AddressFactory.Create(
                    dbLetter.ToAddress_Address,
                    dbLetter.ToAddress_Address2,
                    dbLetter.ToAddress_Zipcode,
                    dbLetter.ToAddress_City,
                    dbLetter.ToAddress_Country,
                    dbLetter.ToAddress_AttPerson,
                    dbLetter.ToAddress_FirstName,
                    dbLetter.ToAddress_LastName,
                    dbLetter.ToAddress_VatNr,
                    dbLetter.ToAddress_Co,
                    dbLetter.ToAddress_State,
                    dbLetter.ToAddress_CompanyName
                ),
                OfficeId = dbLetter.OfficeId,
                OrderId = dbLetter.OrderId,
                LetterDetails = new LetterDetails()
                {
                    LetterColor = (LetterColor)dbLetter.LetterColor,
                    LetterPaperWeight = (LetterPaperWeight)dbLetter.LetterPaperWeight,
                    LetterSize = (LetterSize)dbLetter.LetterSize,
                    LetterProcessing = (LetterProcessing)dbLetter.LetterProcessing,
                    LetterType = (LetterType)dbLetter.LetterType,
                    DeliveryLabel = (DeliveryLabel)dbLetter.DeliveryLabel
                },
                Guid = dbLetter.Guid,
                DeliveryLabel = (DeliveryLabel)(dbLetter.DeliveryLabel),
                ReturnLabel = dbLetter.ReturnLabel.HasValue ? dbLetter.ReturnLabel.Value : 0
            };

            return letter;
        }

        public List<Letter> Create(List<DbLetters> letterses)
        {
            return letterses.Select(Create).ToList();
        }
    }
}
