using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Data.DTO;
using LetterAmazer.WebAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LetterAmazer.WebAPI.Controllers
{
    public class InvoiceController : ApiController
    {
        public InvoiceController()
        {

        }
        private IInvoiceService invoiceService;
        public InvoiceController(IInvoiceService _invoiceService)
        {
            this.invoiceService = _invoiceService;
        }
        
        /// <summary>
        /// Create invoice
        /// </summary>
        /// <param name="invoiceDTO"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Create")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public InvoiceDTO Create(InvoiceDTO invoiceDTO)
        {
            var newInvoice = this.invoiceService.Create(AutoMapper.Mapper.Map<InvoiceDTO, Invoice>(invoiceDTO));
            return AutoMapper.Mapper.Map<Invoice, InvoiceDTO>(newInvoice);
        }

        /// <summary>
        /// Update invoice
        /// </summary>
        /// <param name="invoiceDTO"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Update")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public InvoiceDTO Update(InvoiceDTO invoiceDTO)
        {
            var updatedInvoice = this.invoiceService.Update(AutoMapper.Mapper.Map<InvoiceDTO, Invoice>(invoiceDTO));
            return AutoMapper.Mapper.Map<Invoice, InvoiceDTO>(updatedInvoice);
        }

        /// <summary>
        /// Delete Invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Delete")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public HttpResponseMessage Delete(int invoiceId)
        {
            var invoice = this.invoiceService.GetInvoiceById(invoiceId);
            this.invoiceService.Delete(invoice);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
