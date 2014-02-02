using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Services.LetterContent;
using System.IO;
using System.Web;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services
{
    public class LetterService : ILetterService
    {
        private PdfManager pdfManager;
        private string pdfStoragePath;
        private IRepository repository;
        public LetterService(string pdfStoragePath, IRepository repository)
        {
            this.repository = repository;
            this.pdfManager = new PdfManager();
            this.pdfStoragePath = pdfStoragePath;
            if (this.pdfStoragePath.StartsWith("~")) // relative path
            {
                this.pdfStoragePath = HttpContext.Current.Server.MapPath(this.pdfStoragePath);
            }
        }

        public decimal GetCost(Letter letter)
        {
            int pagesCount = this.pdfManager.GetPagesCount(Path.Combine(this.pdfStoragePath, letter.LetterContent.Path));
            return GetCost(pagesCount, letter.ToAddress);
        }

        private decimal GetCost(int numerOfPages, AddressInfo address)
        {
            return CalculateLetterCost(numerOfPages, address.Address, address.Postal, address.City, "0");// TODO: Fix country
        }

        private decimal CalculateLetterCost()
        {
            return CalculateLetterCost(1);
        }

        private decimal CalculateLetterCost(int pages)
        {
            return CalculateLetterCost(pages, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private decimal CalculateLetterCost(int pages, string address, string postal, string city, string country)
        {
            return CalculateLetterBaseCost(address, postal, city, country) + CalculateAdditionalPagesCount(pages);
        }

        private decimal CalculateLetterCost(string address, string postal, string city, string country)
        {
            return CalculateLetterCost(1, address, postal, city, country);
        }

        private decimal CalculateAdditionalPagesCount(int pages)
        {
            return pages * 0.30m;
        }

        private decimal CalculateLetterBaseCost(string address, string postal, string city, string country)
        {
            return 1.8m;
        }

        public string GetRelativeLetterStoragePath()
        {
            return "~/UserData/PdfLetters";
        }

        public Letter GetLetterById(int letterId)
        {
            Letter letter = repository.GetById<Letter>(letterId);
            if (letter == null)
            {
                throw new ItemNotFoundException("Letter");
            }
            return letter;
        }
    }
}
