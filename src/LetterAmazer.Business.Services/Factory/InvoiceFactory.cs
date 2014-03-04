using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class InvoiceFactory:IInvoiceFactory
    {
        public Invoice Create(DbInvoices invoices)
        {
            return new Invoice()
            {
                Id = invoices.Id,
                Guid = invoices.Guid,
                DateCreated = invoices.DateCreated,
                DateDue = invoices.DateDue,
                InvoiceNumber = invoices.InvoiceNumber,
                InvoiceStatus =(InvoiceStatus) invoices.InvoiceStatus,
                OrderId = invoices.OrderId
            };
        }

        public List<Invoice> Create(List<DbInvoices> invoices)
        {
            return invoices.Select(Create).ToList();
        }
    }
}
