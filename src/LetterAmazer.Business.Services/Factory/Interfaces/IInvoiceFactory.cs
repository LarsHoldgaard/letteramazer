using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IInvoiceFactory
    {
        Invoice Create(DbInvoices invoices);
        List<Invoice> Create(List<DbInvoices> invoices);
    }
}
