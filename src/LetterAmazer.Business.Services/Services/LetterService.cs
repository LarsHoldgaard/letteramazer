using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Services.LetterContent;
using System.Web;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class LetterService : ILetterService
    {
        private PdfManager pdfManager;
        private string pdfStoragePath;
        private LetterAmazerEntities Repository;
        private LetterFactory letterFactory;

        public LetterService(LetterAmazerEntities repository,LetterFactory letterFactory,string pdfStoragePath)
        {
            this.letterFactory = letterFactory;
            Repository = repository;
            this.pdfManager = new PdfManager();
            this.pdfStoragePath = pdfStoragePath;
            if (this.pdfStoragePath.StartsWith("~")) // relative path
            {
                this.pdfStoragePath = HttpContext.Current.Server.MapPath(this.pdfStoragePath);
            }
        }

        public string GetRelativeLetterStoragePath()
        {
            return "~/UserData/PdfLetters";
        }

        public Letter GetLetterById(int letterId)
        {
            DbLetters dbletter = Repository.DbLetters.FirstOrDefault(c => c.Id == letterId);
            if (dbletter == null)
            {
                throw new ItemNotFoundException("Letter");
            }

            var letter = letterFactory.CreateLetter(dbletter);

            return letter;
        }

        public List<Letter> GetLetterBySpecification(LetterSpecification specification)
        {
            throw new NotImplementedException();
        }
    }
}
