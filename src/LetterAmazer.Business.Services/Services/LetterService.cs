﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Interfaces;

namespace LetterAmazer.Business.Services.Services
{
    public class LetterService : ILetterService
    {
        public decimal GetCost(int numerOfPages, AddressInfo address)
        {
            return CalculateLetterCost(numerOfPages, address.Address, address.Postal, address.City, address.Country);
        }

        private decimal CalculateLetterCost()
        {
            return CalculateLetterCost(1);
        }

        private decimal CalculateLetterCost(int pages)
        {
            return CalculateLetterCost(pages, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private decimal CalculateLetterCost(int pages, string address, string postal, string city, string country)
        {
            return CalculateLetterBaseCost(address, postal, city, country) + CalculateAdditionalPagesCount(pages);
        }

        private decimal CalculateLetterCost(string address, string postal, string city, string country)
        {
            return CalculateLetterCost(1, address, postal, city, country);
        }

        private decimal CalculateAdditionalPagesCount(int pages)
        {
            return pages * 0.30m;
        }

        private decimal CalculateLetterBaseCost(string address, string postal, string city, string country)
        {
            return 1.8m;
        }
    }
}