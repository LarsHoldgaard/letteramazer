using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class InvoiceService:IInvoiceService
    {

        private LetterAmazerEntities repository;
        private IInvoiceFactory invoiceFactory;

        public InvoiceService(IInvoiceFactory invoiceFactory, LetterAmazerEntities repository)
        {
            this.invoiceFactory = invoiceFactory;
            this.repository = repository;
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

            return GetInvoiceById(dbInvoice.Id);
        }

        public Invoice GetInvoiceById(int id)
        {
            var dbInvoice = repository.DbInvoices.FirstOrDefault(c => c.Id == id);

            if (dbInvoice == null)
            {
                throw new BusinessException("No invoice by this ID");
            }

            return invoiceFactory.Create(dbInvoice);
        }

        public Invoice GetInvoiceById(Guid id)
        {
            var dbInvoice = repository.DbInvoices.FirstOrDefault(c => c.Guid == id);

            if (dbInvoice == null)
            {
                throw new BusinessException("No invoice by this ID");
            }

            return invoiceFactory.Create(dbInvoice);
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
                dbInvoices = dbInvoices.Where(c => c.DateCreated >= specification.DateFrom);
            }
            if (specification.DateTo.HasValue)
            {
                dbInvoices = dbInvoices.Where(c => c.DateCreated <= specification.DateTo);
            }

            return invoiceFactory.Create(dbInvoices.OrderBy(c=>c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }
    }
}
