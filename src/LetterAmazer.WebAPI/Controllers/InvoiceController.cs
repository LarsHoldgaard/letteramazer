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
        [HttpGet, ActionName("Get")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        [Route("api/Invoice/{invoiceId}", Name = "Invoice")]
        public InvoiceDTO  Get(int invoiceId)
        {
            var getdata = this.invoiceService.GetInvoiceById(invoiceId);
            var finaldt= AutoMapper.Mapper.DynamicMap<Invoice, InvoiceDTO>(getdata);
            return finaldt;
        }

        /// <summary>
        /// Find Order
        /// </summary>
        /// <param name="InvoiceSpecificationDTO"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public IList<InvoiceDTO> Find(InvoiceSpecificationDTO invoiceSpecification)
        {
            var invoiceList = this.invoiceService.GetInvoiceBySpecification(AutoMapper.Mapper.Map<InvoiceSpecificationDTO, InvoiceSpecification>(invoiceSpecification));
            return AutoMapper.Mapper.DynamicMap<IList<Invoice>, IList<InvoiceDTO>>(invoiceList);
        }
    }
}
