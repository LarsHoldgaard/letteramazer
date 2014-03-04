﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Invoice
{
    public interface IInvoiceService
    {
        Invoice Create(Invoice invoice);

        Invoice Update(Invoice invoice);

        Invoice GetInvoiceById(int id);

        List<Invoice> GetInvoiceBySpecification(InvoiceSpecification specification);
    }
}
