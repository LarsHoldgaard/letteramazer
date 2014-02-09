﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Countries
{
    public class CountrySpecification:Specifications
    {
        public int Id { get; set; }
        public bool? InsideEu { get; set; }
    }
}
