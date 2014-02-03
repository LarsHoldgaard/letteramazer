using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class LetterFactory
    {
        public CustomerService CustomerService { get; set; }


        public Letter CreateLetter(DbLetters dbLetter)
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
                LetterStatus = (LetterStatus)(dbLetter.LetterStatus)
            };

            return letter;
        }
    }
}
