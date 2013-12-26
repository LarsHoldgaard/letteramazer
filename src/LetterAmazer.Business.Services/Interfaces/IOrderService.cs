﻿using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Interfaces
{
    public interface IOrderService
    {
        string CreateOrder(OrderContext orderContext);
    }
}