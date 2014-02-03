﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Customers;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public class Letter
    {
        public AddressInfo FromAddress { get; set; }
        public AddressInfo ToAddress { get; set; }
        public Customer Customer { get; set; }
        public LetterStatus LetterStatus { get; set; }
        public int OfficeProductId { get; set; }
        public LetterContent LetterContent { get; set; } 
        
    }
}
