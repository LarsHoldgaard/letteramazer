﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.ProductMatrix;

namespace LetterAmazer.Business.Services.Utils
{
    public static class Helpers
    {
        public static IEnumerable<ProductMatrix> RemoveDuplicatePriceTypesFromMatrix(IEnumerable<ProductMatrix> matrices)
        {
            List<ProductMatrix> m = new List<ProductMatrix>();
            foreach (var productMatrix in matrices)
            {
                if (!m.Any(c => c.PriceType == productMatrix.PriceType))
                {
                    m.Add(productMatrix);
                }
            }
            return m;
        }
    }
}
