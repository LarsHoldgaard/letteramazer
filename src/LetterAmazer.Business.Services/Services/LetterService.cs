using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LetterAmazer.Business.Services.Domain.Caching;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Factory.Interfaces;
using System.Web;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class LetterService : ILetterService
    {
        private LetterAmazerEntities repository;
        private ILetterFactory letterFactory;
        private ICacheService cacheService;

        public LetterService(LetterAmazerEntities repository,ILetterFactory letterFactory,ICacheService cacheService)
        {
            this.letterFactory = letterFactory;
            this.repository = repository;
            this.cacheService = cacheService;
        }

        public Letter GetLetterById(int letterId)
        {
            var cacheKey = cacheService.GetCacheKey(MethodBase.GetCurrentMethod().Name, letterId.ToString());
            if (!cacheService.ContainsKey(cacheKey))
            {
                DbLetters dbletter = repository.DbLetters.FirstOrDefault(c => c.Id == letterId);
                if (dbletter == null)
                {
                    throw new ItemNotFoundException("Letter");
                }

                var letter = letterFactory.Create(dbletter);
                cacheService.Create(cacheKey, letter);
                return letter;
            }
            return (Letter)(cacheService.GetById(cacheKey));
        }

        public List<Letter> GetLetterBySpecification(LetterSpecification specification)
        {
            IQueryable<DbLetters> dbLetters = repository.DbLetters;

            if (specification.Id > 0)
            {
                dbLetters = dbLetters.Where(c => c.Id == specification.Id);
            }
            if (specification.OrderId > 0)
            {
                dbLetters = dbLetters.Where(c => c.OrderId == specification.OrderId);
            }
            if (specification.LetterStatus != null)
            {
                dbLetters = dbLetters.Where(c => specification.LetterStatus.Contains((LetterStatus)c.LetterStatus));
            }

            return letterFactory.Create(dbLetters.OrderBy(c=>c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public Letter Create(Letter letter)
        {
            var dbletter = new DbLetters();

            if (dbletter == null)
            {
                throw new BusinessException();
            }

            dbletter.LetterContent_Path = letter.LetterContent.Path;
            dbletter.LetterContent_WrittenContent = letter.LetterContent.WrittenContent;
            dbletter.LetterStatus = (int)letter.LetterStatus;
            dbletter.OfficeId = letter.OfficeId;

            dbletter.LetterColor = (int)letter.LetterDetails.LetterColor;
            dbletter.LetterPaperWeight = (int)letter.LetterDetails.LetterPaperWeight;
            dbletter.LetterProcessing = (int)letter.LetterDetails.LetterProcessing;
            dbletter.LetterType = (int)letter.LetterDetails.LetterType;
            dbletter.LetterSize = (int)letter.LetterDetails.LetterSize;
            dbletter.DeliveryLabel = (int) letter.LetterDetails.DeliveryLabel;
            dbletter.Guid = Guid.NewGuid();

            dbletter.ToAddress_Address = letter.ToAddress.Address1;
            dbletter.ToAddress_Address2 = letter.ToAddress.Address2;
            dbletter.ToAddress_AttPerson = letter.ToAddress.AttPerson;
            dbletter.ToAddress_City = letter.ToAddress.City;
            dbletter.ToAddress_CompanyName = string.Empty;
            dbletter.ToAddress_Country = letter.ToAddress.Country.Id;
            dbletter.ToAddress_FirstName = letter.ToAddress.FirstName;
            dbletter.ToAddress_LastName = letter.ToAddress.LastName;
            dbletter.ToAddress_Zipcode = letter.ToAddress.Zipcode;
            dbletter.ToAddress_VatNr = letter.ToAddress.VatNr;

            dbletter.FromAddress_Address = letter.FromAddress.Address1;
            dbletter.FromAddress_Address2 = letter.FromAddress.Address2;
            dbletter.FromAddress_AttPerson = letter.FromAddress.AttPerson;
            dbletter.FromAddress_City = letter.FromAddress.City;
            dbletter.FromAddress_CompanyName = string.Empty;
            dbletter.FromAddress_Country = letter.FromAddress.Country.Id;
            dbletter.FromAddress_FirstName = letter.FromAddress.FirstName;
            dbletter.FromAddress_LastName = letter.FromAddress.LastName;
            dbletter.FromAddress_Zipcode = letter.FromAddress.Zipcode;
            dbletter.FromAddress_VatNr = letter.FromAddress.VatNr;

            repository.SaveChanges();


            cacheService.Delete(cacheService.GetCacheKey("GetLetterById",letter.Id.ToString()));
            return GetLetterById(letter.Id);
        }

        public Letter Update(Letter letter)
        {
            var dbletter = repository.DbLetters.FirstOrDefault(c => c.Id == letter.Id);

            if (dbletter == null)
            {
                throw new BusinessException();
            }

            dbletter.LetterContent_Path = letter.LetterContent.Path;
            dbletter.LetterContent_WrittenContent = letter.LetterContent.WrittenContent;
            dbletter.LetterStatus = (int)letter.LetterStatus;
            dbletter.OfficeId = letter.OfficeId;
            dbletter.Guid = letter.Guid;

            dbletter.LetterColor = (int)letter.LetterDetails.LetterColor;
            dbletter.LetterPaperWeight = (int)letter.LetterDetails.LetterPaperWeight;
            dbletter.LetterProcessing = (int)letter.LetterDetails.LetterProcessing;
            dbletter.LetterType = (int)letter.LetterDetails.LetterType;
            dbletter.LetterSize = (int)letter.LetterDetails.LetterSize;
            dbletter.DeliveryLabel = (int)letter.LetterDetails.DeliveryLabel;

            dbletter.ToAddress_Address = letter.ToAddress.Address1;
            dbletter.ToAddress_Address2 = letter.ToAddress.Address2;
            dbletter.ToAddress_AttPerson = letter.ToAddress.AttPerson;
            dbletter.ToAddress_City = letter.ToAddress.City;
            dbletter.ToAddress_CompanyName = string.Empty;
            dbletter.ToAddress_Country = letter.ToAddress.Country.Id;
            dbletter.ToAddress_FirstName = letter.ToAddress.FirstName;
            dbletter.ToAddress_LastName = letter.ToAddress.LastName;
            dbletter.ToAddress_Zipcode = letter.ToAddress.Zipcode;
            dbletter.ToAddress_VatNr = letter.ToAddress.VatNr;

            dbletter.FromAddress_Address = letter.FromAddress.Address1;
            dbletter.FromAddress_Address2 = letter.FromAddress.Address2;
            dbletter.FromAddress_AttPerson = letter.FromAddress.AttPerson;
            dbletter.FromAddress_City = letter.FromAddress.City;
            dbletter.FromAddress_CompanyName = string.Empty;

            if (letter.FromAddress.Country != null)
            {
                dbletter.FromAddress_Country = letter.FromAddress.Country.Id;    
            }
            
            dbletter.FromAddress_FirstName = letter.FromAddress.FirstName;
            dbletter.FromAddress_LastName = letter.FromAddress.LastName;
            dbletter.FromAddress_Zipcode = letter.FromAddress.Zipcode;
            dbletter.FromAddress_VatNr = letter.FromAddress.VatNr;

            repository.SaveChanges();

            cacheService.Delete(cacheService.GetCacheKey("GetLetterById", letter.Id.ToString()));
            return GetLetterById(letter.Id);
        }

        public void Delete(Letter letter)
        {
            var dbletter = repository.DbLetters.FirstOrDefault(c => c.Id == letter.Id);

            if (dbletter == null)
            {
                throw new BusinessException();
            }

            repository.DbLetters.Remove(dbletter);
            repository.SaveChanges();

            cacheService.Delete(cacheService.GetCacheKey("GetLetterById", letter.Id.ToString()));
        }
    }
}
