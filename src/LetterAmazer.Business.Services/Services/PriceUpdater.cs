using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.PriceUpdater;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Utils;

namespace LetterAmazer.Business.Services.Services
{
    
    public class PriceUpdater : IPriceUpdater
    {
        private IPriceService priceService;
        private IOfficeProductService officeProductService;
        private IProductMatrixService productMatrixService;

        public PriceUpdater(IPriceService priceService, IProductMatrixService productMatrixService,
            IOfficeProductService officeProductService)
        {
            this.priceService = priceService;
            this.productMatrixService = productMatrixService;
            this.officeProductService = officeProductService;
        }

        public void Execute()
        {
            var products = officeProductService.GetOfficeProductBySpecification(new OfficeProductSpecification()
            {
                Take = int.MaxValue
            });

            var matrices = productMatrixService.GetProductMatrixBySpecification(new ProductMatrixSpecification()
            {
                Take = int.MaxValue,
                ProductMatrixReferenceType = ProductMatrixReferenceType.Contractor
            });

            foreach (var officeProduct in products)
            {
                var matrix =
                    matrices.Where(
                        c => c.ValueId == officeProduct.Id && c.ReferenceType == ProductMatrixReferenceType.Contractor);

                
                matrix = Helpers.RemoveDuplicatePriceTypesFromMatrix(matrix);
                UpdateFromProduct(officeProduct, matrix);
            }
        }



        private void UpdateFromProduct(OfficeProduct officeProduct, IEnumerable<ProductMatrix> productMatrix)
        {
            foreach (var productMatrices in productMatrix)
            {
                var matrix = new ProductMatrix()
                {
                    PriceType = productMatrices.PriceType,
                    ReferenceType = ProductMatrixReferenceType.Sales,
                    ValueId = officeProduct.Id,
                    ProductLines = new List<ProductMatrixLine>(),
                    SpanLower = productMatrices.SpanLower,
                    SpanUpper = productMatrices.SpanUpper
                };

                foreach (var orderLine in productMatrices.ProductLines)
                {
                    matrix.ProductLines.Add(new ProductMatrixLine()
                    {
                        BaseCost = CalculateSalesPrice(orderLine.BaseCost, orderLine.LineType),
                        LineType = orderLine.LineType,
                        Title = orderLine.Title
                    });
                }
                productMatrixService.Create(matrix);
                priceService.Create(new Pricing()
                {
                    DateCreated = DateTime.Now,
                    OfficeProductId = officeProduct.Id
                });
            }
        }

        private decimal CalculateSalesPrice(decimal value, ProductMatrixLineType lineType)
        {
            if (lineType == ProductMatrixLineType.Postage)
            {
                return value;
            }
            decimal adderPercentage = 20.0m / 100.0m;
            return value * (1 + adderPercentage);
        }
    }
}
