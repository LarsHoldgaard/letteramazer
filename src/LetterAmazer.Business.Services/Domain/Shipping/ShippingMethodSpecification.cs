﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Shipping
{
    public class ShippingMethodSpecification:Specifications
    {
        public int CountryId { get; set; }
        
    }
}
