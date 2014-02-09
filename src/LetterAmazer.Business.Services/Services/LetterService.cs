﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public LetterService(LetterAmazerEntities repository,ILetterFactory letterFactory)
        {
            this.letterFactory = letterFactory;
            this.repository = repository;
        }

        public Letter GetLetterById(int letterId)
        {
            DbLetters dbletter = repository.DbLetters.FirstOrDefault(c => c.Id == letterId);
            if (dbletter == null)
            {
                throw new ItemNotFoundException("Letter");
            }

            var letter = letterFactory.Create(dbletter);

            return letter;
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

            return letterFactory.Create(dbLetters.Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public Letter Create(Letter letter)
        {
            var dbletter = new DbLetters();

            if (dbletter == null)
            {
                throw new BusinessException();
            }

            dbletter.LetterContent_Content = letter.LetterContent.Content;
            dbletter.LetterContent_Path = letter.LetterContent.Path;
            dbletter.LetterContent_WrittenContent = letter.LetterContent.WrittenContent;
            dbletter.LetterStatus = (int)letter.LetterStatus;
            dbletter.OfficeProductId = letter.OfficeProductId;
            dbletter.ToAddress_Address = letter.ToAddress.Address1;
            dbletter.ToAddress_Address2 = letter.ToAddress.Address2;
            dbletter.ToAddress_AttPerson = letter.ToAddress.AttPerson;
            dbletter.ToAddress_City = letter.ToAddress.City;
            dbletter.ToAddress_CompanyName = string.Empty;
            dbletter.ToAddress_Country = letter.ToAddress.Country.Id;
            dbletter.ToAddress_FirstName = letter.ToAddress.FirstName;
            dbletter.ToAddress_LastName = letter.ToAddress.LastName;
            dbletter.ToAddress_Postal = letter.ToAddress.PostalCode;
            dbletter.ToAddress_VatNr = letter.ToAddress.VatNr;

            dbletter.FromAddress_Address = letter.FromAddress.Address1;
            dbletter.FromAddress_Address2 = letter.FromAddress.Address2;
            dbletter.FromAddress_AttPerson = letter.FromAddress.AttPerson;
            dbletter.FromAddress_City = letter.FromAddress.City;
            dbletter.FromAddress_CompanyName = string.Empty;
            dbletter.FromAddress_Country = letter.FromAddress.Country.Id;
            dbletter.FromAddress_FirstName = letter.FromAddress.FirstName;
            dbletter.FromAddress_LastName = letter.FromAddress.LastName;
            dbletter.FromAddress_Postal = letter.FromAddress.PostalCode;
            dbletter.FromAddress_VatNr = letter.FromAddress.VatNr;

            repository.SaveChanges();

            return GetLetterById(letter.Id);
        }

        public Letter Update(Letter letter)
        {
            var dbletter = repository.DbLetters.FirstOrDefault(c => c.Id == letter.Id);

            if (dbletter == null)
            {
                throw new BusinessException();
            }

            dbletter.LetterContent_Content = letter.LetterContent.Content;
            dbletter.LetterContent_Path = letter.LetterContent.Path;
            dbletter.LetterContent_WrittenContent = letter.LetterContent.WrittenContent;
            dbletter.LetterStatus = (int)letter.LetterStatus;
            dbletter.OfficeProductId = letter.OfficeProductId;
            dbletter.ToAddress_Address = letter.ToAddress.Address1;
            dbletter.ToAddress_Address2 = letter.ToAddress.Address2;
            dbletter.ToAddress_AttPerson = letter.ToAddress.AttPerson;
            dbletter.ToAddress_City = letter.ToAddress.City;
            dbletter.ToAddress_CompanyName = string.Empty;
            dbletter.ToAddress_Country = letter.ToAddress.Country.Id;
            dbletter.ToAddress_FirstName = letter.ToAddress.FirstName;
            dbletter.ToAddress_LastName = letter.ToAddress.LastName;
            dbletter.ToAddress_Postal = letter.ToAddress.PostalCode;
            dbletter.ToAddress_VatNr = letter.ToAddress.VatNr;

            dbletter.FromAddress_Address = letter.FromAddress.Address1;
            dbletter.FromAddress_Address2 = letter.FromAddress.Address2;
            dbletter.FromAddress_AttPerson = letter.FromAddress.AttPerson;
            dbletter.FromAddress_City = letter.FromAddress.City;
            dbletter.FromAddress_CompanyName = string.Empty;
            dbletter.FromAddress_Country = letter.FromAddress.Country.Id;
            dbletter.FromAddress_FirstName = letter.FromAddress.FirstName;
            dbletter.FromAddress_LastName = letter.FromAddress.LastName;
            dbletter.FromAddress_Postal = letter.FromAddress.PostalCode;
            dbletter.FromAddress_VatNr = letter.FromAddress.VatNr;

            repository.SaveChanges();

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
        }
    }
}
