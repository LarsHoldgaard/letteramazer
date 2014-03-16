using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class InvoiceFactory : IInvoiceFactory
    {
        private IAddressFactory addressFactory;
        private IOrganisationService organisationService;

        public InvoiceFactory(IAddressFactory addressFactory, IOrganisationService organisationService)
        {
            this.addressFactory = addressFactory;
            this.organisationService = organisationService;
        }

        public Invoice Create(DbInvoices invoices, List<DbInvoiceLines> invoiceLines)
        {
            var organisation = organisationService.GetOrganisationById(invoices.OrganisationId);
                
            return new Invoice()
            {
                Id = invoices.Id,
                Guid = invoices.Guid,
                DateCreated = invoices.DateCreated,
                DateDue = invoices.DateDue,
                InvoiceNumber = invoices.InvoiceNumber,
                InvoiceStatus = (InvoiceStatus) invoices.InvoiceStatus,
                OrderId = invoices.OrderId,
                InvoiceLines = createInvoiceLines(invoiceLines),
                PriceExVat = invoices.PriceExVat,
                PriceTotal = invoices.PriceTotal,
                PriceVatPercentage = invoices.PriceVatPercentage,
                PriceVat = invoices.PriceVat,
                OrganisationId = invoices.OrganisationId,
                InvoiceInfo = addressFactory.Create(invoices.Invoice_Address1, invoices.Invoice_Address2, invoices.Invoice_Zipcode, invoices.Invoice_City, invoices.Invoice_CountryId, invoices.Invoice_AttPerson, string.Empty, string.Empty, invoices.Invoice_Vatnumber, string.Empty, invoices.Invoice_State, invoices.Invoice_Organisation),
                ReceiverInfo = addressFactory.Create(invoices.Receiver_Address1, invoices.Receiver_Address2, invoices.Receiver_Zipcode, invoices.Invoice_City, invoices.Receiver_CountryId, invoices.Receiver_AttPerson, string.Empty, string.Empty, invoices.Receiver_Vatnumber, string.Empty, invoices.Receiver_State, invoices.Receiver_Organisation),
                InvoicePaymentMessage = invoices.InvoicePaymentMessage
            };
        }

        public List<Invoice> Create(List<DbInvoices> invoices, List<List<DbInvoiceLines>> invoiceLines)
        {
            if (invoices.Count != invoiceLines.Count)
            {
                throw new ArgumentException("Every invoice must has a list of invoice lines");
            }

            return invoices.Select((t, i) => Create(t, invoiceLines[i])).ToList();
        }

        private List<InvoiceLine> createInvoiceLines(List<DbInvoiceLines> dbInvoiceLines)
        {
            List<InvoiceLine> invoice = new List<InvoiceLine>();
            foreach (var line in dbInvoiceLines)
            {
                invoice.Add(new InvoiceLine()
                {
                    Description = line.Description,
                    Id = line.Id,
                    InvoiceId = line.InvoiceId,
                    PriceExVat = line.PriceExVat,
                    Quantity = line.Quantity
                });
            }
            return invoice;
        }
    }
}
