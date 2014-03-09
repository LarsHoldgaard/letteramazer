using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class InvoiceService:IInvoiceService
    {
        private IMailService mailService;
        private LetterAmazerEntities repository;
        private IInvoiceFactory invoiceFactory;

        public InvoiceService(IInvoiceFactory invoiceFactory, LetterAmazerEntities repository, IMailService mailService)
        {
            this.invoiceFactory = invoiceFactory;
            this.repository = repository;
            this.mailService = mailService;
        }


        public Invoice Create(Invoice invoice)
        {
            DbInvoices dbInvoice = new DbInvoices();
            dbInvoice.Guid = invoice.Guid;
            dbInvoice.DateDue = invoice.DateDue;
            dbInvoice.DateCreated = DateTime.Now;
            dbInvoice.OrganisationId = invoice.OrganisationId;
            dbInvoice.OrderId = invoice.OrderId;
            dbInvoice.InvoiceStatus = (int) invoice.InvoiceStatus;
            dbInvoice.InvoiceNumber = invoice.InvoiceNumber;
            dbInvoice.PriceExVat = invoice.PriceExVat;
            dbInvoice.PriceTotal = invoice.PriceTotal;
            dbInvoice.PriceVatPercentage = invoice.PriceVatPercentage;
            dbInvoice.PriceVat = invoice.PriceVat;

            dbInvoice.Receiver_Zipcode = invoice.ReceiverInfo.Zipcode;
            dbInvoice.Receiver_Vatnumber = invoice.ReceiverInfo.VatNr;
            dbInvoice.Receiver_State = invoice.ReceiverInfo.State;
            dbInvoice.Receiver_Organisation = invoice.ReceiverInfo.Organisation;
            dbInvoice.Receiver_CountryId = invoice.ReceiverInfo.Country.Id;
            dbInvoice.Receiver_City = invoice.ReceiverInfo.City;
            dbInvoice.Receiver_AttPerson = invoice.ReceiverInfo.AttPerson;
            dbInvoice.Receiver_Address2 = invoice.ReceiverInfo.Address2;
            dbInvoice.Receiver_Address1 = invoice.ReceiverInfo.Address1;

            dbInvoice.Invoice_Zipcode = invoice.InvoiceInfo.Zipcode;
            dbInvoice.Invoice_Vatnumber = invoice.InvoiceInfo.VatNr;
            dbInvoice.Invoice_State = invoice.InvoiceInfo.State;
            dbInvoice.Invoice_Organisation = invoice.InvoiceInfo.Organisation;
            dbInvoice.Invoice_CountryId = invoice.InvoiceInfo.Country.Id;
            dbInvoice.Invoice_City = invoice.InvoiceInfo.City;
            dbInvoice.Invoice_AttPerson = invoice.InvoiceInfo.AttPerson;
            dbInvoice.Invoice_Address2 = invoice.InvoiceInfo.Address2;
            dbInvoice.Invoice_Address1 = invoice.InvoiceInfo.Address1;


            repository.DbInvoices.Add(dbInvoice);
            repository.SaveChanges();

            foreach (var invoiceLine in invoice.InvoiceLines)
            {
                var dbLine = new DbInvoiceLines()
                {
                    InvoiceId = dbInvoice.Id,
                    Quantity = invoiceLine.Quantity,
                    PriceExVat = invoiceLine.PriceExVat,
                    Description = invoiceLine.Description
                };
                repository.DbInvoiceLines.Add(dbLine);
            }
            repository.SaveChanges();

            mailService.NotificationInvoiceCreated();

            return GetInvoiceById(dbInvoice.Id);
        }

        public Invoice Update(Invoice invoice)
        {
            var dbInvoice = repository.DbInvoices.FirstOrDefault(c => c.Id == invoice.Id);

            if (dbInvoice == null)
            {
                throw new BusinessException("No invoice by this ID");
            }

            dbInvoice.Guid = invoice.Guid;
            dbInvoice.DateDue = invoice.DateDue;
            dbInvoice.DateCreated = DateTime.Now;
            dbInvoice.OrganisationId = invoice.OrganisationId;
            dbInvoice.OrderId = invoice.OrderId;
            dbInvoice.InvoiceStatus = (int)invoice.InvoiceStatus;
            dbInvoice.InvoiceNumber = invoice.InvoiceNumber;

            dbInvoice.PriceExVat = invoice.PriceExVat;
            dbInvoice.PriceTotal = invoice.PriceTotal;
            dbInvoice.PriceVatPercentage = invoice.PriceVatPercentage;
            dbInvoice.PriceVat = invoice.PriceVat;

            dbInvoice.Receiver_Zipcode = invoice.ReceiverInfo.Zipcode;
            dbInvoice.Receiver_Vatnumber = invoice.ReceiverInfo.VatNr;
            dbInvoice.Receiver_State = invoice.ReceiverInfo.State;
            dbInvoice.Receiver_Organisation = invoice.ReceiverInfo.Organisation;
            dbInvoice.Receiver_CountryId = invoice.ReceiverInfo.Country.Id;
            dbInvoice.Receiver_City = invoice.ReceiverInfo.City;
            dbInvoice.Receiver_AttPerson = invoice.ReceiverInfo.AttPerson;
            dbInvoice.Receiver_Address2 = invoice.ReceiverInfo.Address2;
            dbInvoice.Receiver_Address1 = invoice.ReceiverInfo.Address1;

            dbInvoice.Invoice_Zipcode = invoice.InvoiceInfo.Zipcode;
            dbInvoice.Invoice_Vatnumber = invoice.InvoiceInfo.VatNr;
            dbInvoice.Invoice_State = invoice.InvoiceInfo.State;
            dbInvoice.Invoice_Organisation = invoice.InvoiceInfo.Organisation;
            dbInvoice.Invoice_CountryId = invoice.InvoiceInfo.Country.Id;
            dbInvoice.Invoice_City = invoice.InvoiceInfo.City;
            dbInvoice.Invoice_AttPerson = invoice.InvoiceInfo.AttPerson;
            dbInvoice.Invoice_Address2 = invoice.InvoiceInfo.Address2;
            dbInvoice.Invoice_Address1 = invoice.InvoiceInfo.Address1;

            return GetInvoiceById(dbInvoice.Id);
        }

        public Invoice GetInvoiceById(int id)
        {
            var dbInvoice = repository.DbInvoices.FirstOrDefault(c => c.Id == id);

            if (dbInvoice == null)
            {
                throw new BusinessException("No invoice by this ID");
            }

            var dbInvoiceLines = repository.DbInvoiceLines.Where(c => c.InvoiceId == dbInvoice.Id);

            return invoiceFactory.Create(dbInvoice,dbInvoiceLines.ToList());
        }

        public Invoice GetInvoiceById(Guid id)
        {
            var dbInvoice = repository.DbInvoices.FirstOrDefault(c => c.Guid == id);

            if (dbInvoice == null)
            {
                throw new BusinessException("No invoice by this ID");
            }

            var dbInvoiceLines = repository.DbInvoiceLines.Where(c => c.InvoiceId == dbInvoice.Id);

            

            return invoiceFactory.Create(dbInvoice,dbInvoiceLines.ToList());
        }

        public List<Invoice> GetInvoiceBySpecification(InvoiceSpecification specification)
        {
            IQueryable<DbInvoices> dbInvoices = repository.DbInvoices;

            if (specification.OrderId > 0)
            {
                dbInvoices = dbInvoices.Where(c => c.OrderId == specification.OrderId);
            }
            if (specification.OrganisationId > 0)
            {
                dbInvoices = dbInvoices.Where(c => c.OrganisationId == specification.OrganisationId);
            }
            if (specification.DateFrom.HasValue)
            {
                dbInvoices = dbInvoices.Where(c => EntityFunctions.TruncateTime(c.DateCreated) >= EntityFunctions.TruncateTime(specification.DateFrom.Value));
            }
            if (specification.DateTo.HasValue)
            {
                dbInvoices = dbInvoices.Where(c => EntityFunctions.TruncateTime(c.DateCreated) <= EntityFunctions.TruncateTime(specification.DateTo.Value));
            }

            var dbInvoiceFound =
                dbInvoices.OrderByDescending(c => c.DateCreated).Skip(specification.Skip).Take(specification.Take).ToList();
            var dbInvoiceLinesFound = new List<List<DbInvoiceLines>>();

            foreach (var dbInvoicesInFound in dbInvoiceFound)
            {
                var dbInvoiceLines = repository.DbInvoiceLines.Where(c => c.InvoiceId == dbInvoicesInFound.Id).ToList();
                dbInvoiceLinesFound.Add(dbInvoiceLines);
            }

            return invoiceFactory.Create(dbInvoiceFound,dbInvoiceLinesFound);
        }
    }
}
