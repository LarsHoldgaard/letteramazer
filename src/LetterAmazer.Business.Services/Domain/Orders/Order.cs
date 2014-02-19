﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.util;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OrderLines;
using LetterAmazer.Business.Services.Domain.Payments;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string OrderCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Customer Customer { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DatePaid { get; set; }

        public string TransactionCode { get; set; }
        public decimal Cost { get; set; }

        public decimal VatPercentage { get; set; }

        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        
        public List<PaymentMethods> PaymentMethods { get; set; }


        public Order()
        {
            this.PaymentMethods = new List<PaymentMethods>();
        }

        public override string ToString()
        {
            
            

            return base.ToString();
        }
    }
}
