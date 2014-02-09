using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class LetterFactory : ILetterFactory
    {
        private ICustomerService CustomerService;
        private IAddressFactory AddressFactory;

        public LetterFactory(ICustomerService customerService, IAddressFactory addressFactory)
        {
            this.CustomerService = customerService;
            this.AddressFactory = addressFactory;
        }

        public Letter Create(DbLetters dbLetter)
        {
            var letter = new Letter()
            {
                Customer = CustomerService.GetCustomerById(dbLetter.Id),
                LetterContent = new LetterContent()
                {
                    Content = dbLetter.LetterContent_Content,
                    Path = dbLetter.LetterContent_Path,
                    WrittenContent = dbLetter.LetterContent_WrittenContent
                },
                LetterStatus = (LetterStatus)(dbLetter.LetterStatus),
                Id = dbLetter.Id,
                FromAddress = AddressFactory.Create(
                    dbLetter.FromAddress_Address,
                    dbLetter.FromAddress_Address2,
                    dbLetter.FromAddress_Postal,
                    dbLetter.FromAddress_City,
                    dbLetter.FromAddress_Country.Value,
                    dbLetter.FromAddress_AttPerson,
                    dbLetter.FromAddress_FirstName,
                    dbLetter.FromAddress_LastName,
                    dbLetter.FromAddress_VatNr,
                    dbLetter.FromAddress_Co,
                    dbLetter.FromAddress_State
                ),
                ToAddress =  AddressFactory.Create(
                    dbLetter.ToAddress_Address,
                    dbLetter.ToAddress_Address2,
                    dbLetter.ToAddress_Postal,
                    dbLetter.ToAddress_City,
                    dbLetter.ToAddress_Country.Value,
                    dbLetter.ToAddress_AttPerson,
                    dbLetter.ToAddress_FirstName,
                    dbLetter.ToAddress_LastName,
                    dbLetter.ToAddress_VatNr,
                    dbLetter.ToAddress_Co,
                    dbLetter.ToAddress_State
                ),
                OfficeProductId = dbLetter.OfficeProductId,
                OrderId = dbLetter.OrderId
            };

            return letter;
        }

        public List<Letter> Create(List<DbLetters> letterses)
        {
            return letterses.Select(Create).ToList();
        }
    }
}
