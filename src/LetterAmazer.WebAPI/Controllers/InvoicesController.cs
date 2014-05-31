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
    public class InvoicesController : ApiController
    {
        private IInvoiceService invoiceService;
        public InvoicesController(IInvoiceService _invoiceService)
        {
            this.invoiceService = _invoiceService;
        }


        
        /// <summary>
        /// Create invoice
        /// </summary>
        /// <param name="invoiceDTO"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Create")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public InvoiceDTO Create(InvoiceDTO invoiceDTO)
        {
            var newInvoice = this.invoiceService.Create(AutoMapper.Mapper.DynamicMap<InvoiceDTO, Invoice>(invoiceDTO));
            return AutoMapper.Mapper.Map<Invoice, InvoiceDTO>(newInvoice);
        }

        /// <summary>
        /// Update invoice
        /// </summary>
        /// <param name="invoiceDTO"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Update")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public InvoiceDTO Update(InvoiceDTO invoiceDTO)
        {
            var updatedInvoice = this.invoiceService.Update(AutoMapper.Mapper.DynamicMap<InvoiceDTO, Invoice>(invoiceDTO));
            return AutoMapper.Mapper.DynamicMap<Invoice, InvoiceDTO>(updatedInvoice);
        }

        /// <summary>
        /// Delete Invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
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
            var invoiceList = this.invoiceService.GetInvoiceBySpecification(AutoMapper.Mapper.DynamicMap<InvoiceSpecificationDTO, InvoiceSpecification>(invoiceSpecification));
            var invoiceListDto = new List<InvoiceDTO>();
            foreach (var item in invoiceList)
            {
                invoiceListDto.Add(AutoMapper.Mapper.DynamicMap<InvoiceDTO>(item));
            }
            return invoiceListDto;
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <param name="InvoiceSpecificationDTO"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Count")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public int Count(InvoiceSpecificationDTO invoiceSpecification)
        {
            throw new NotImplementedException();
        }
    }
}
