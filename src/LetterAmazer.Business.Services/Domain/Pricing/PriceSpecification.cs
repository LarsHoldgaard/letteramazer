﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Pricing
{
    public class PriceSpecification:Specifications
    {
        public int CountryId { get; set; }
        
    }
}
