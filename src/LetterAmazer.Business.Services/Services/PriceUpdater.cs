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
            var total_products = officeProductService.GetOfficeProductBySpecification(new OfficeProductSpecification()
            {
                Take = int.MaxValue,
                ProductMatrixReferenceType = ProductMatrixReferenceType.Contractor
            });
            var groupedProducts = officeProductService.GroupByUnique(total_products);

            var total_matrices = productMatrixService.GetProductMatrixBySpecification(new ProductMatrixLineSpecification()
            {
                Take = int.MaxValue,
            });

            foreach (var groupedProduct in groupedProducts)
            {
                StoreCheapestOfficeProductInGroup(groupedProduct, total_matrices);
            }
        }

        #region Private helpers

        private void StoreCheapestOfficeProductInGroup(KeyValuePair<int, List<OfficeProduct>> groupedProduct, IEnumerable<ProductMatrixLine> total_matrices)
        {
            decimal cheapest = Decimal.MaxValue;
            int lowestOfficeProductId = 0;

            foreach (var officeProduct in groupedProduct.Value)
            {
                var matrices = total_matrices.Where(c => c.OfficeProductId == officeProduct.Id);

                // TODO: Must fix more than 1
                var price = priceService.GetPriceByMatrixLines(matrices, 1);

                if (cheapest > price.PriceExVat)
                {
                    lowestOfficeProductId = officeProduct.Id;
                    cheapest = price.PriceExVat;
                }

            }

            SaveOfficeProduct(lowestOfficeProductId);
        }

        private void SaveOfficeProduct(int lowestOfficeProductId)
        {
            // We can assume this is the cheapest officeproduct of any with exact same product details
            var cheapest_officeProduct = officeProductService.GetOfficeProductById(lowestOfficeProductId);
            cheapest_officeProduct.ReferenceType = ProductMatrixReferenceType.Sales;
           
            // Update price of the office product matrix lines
            foreach (var productMatrixLine in cheapest_officeProduct.ProductMatrixLines)
            {
                productMatrixLine.BaseCost = CalculateSalesPrice(productMatrixLine.BaseCost,
                    productMatrixLine.LineType);
            }

            // Add cheapest office product to database by either creating or updating price
            var existing_officeProduct =
                officeProductService.GetOfficeProductBySpecification(new OfficeProductSpecification()
                {
                    ContinentId = cheapest_officeProduct.ContinentId,
                    CountryId = cheapest_officeProduct.CountryId,
                    LetterColor = cheapest_officeProduct.LetterDetails.LetterColor,
                    LetterPaperWeight = cheapest_officeProduct.LetterDetails.LetterPaperWeight,
                    LetterProcessing = cheapest_officeProduct.LetterDetails.LetterProcessing,
                    LetterSize = cheapest_officeProduct.LetterDetails.LetterSize,
                    LetterType = cheapest_officeProduct.LetterDetails.LetterType,
                    ProductScope = cheapest_officeProduct.ProductScope,
                    ZipId = cheapest_officeProduct.ContinentId,
                    ProductMatrixReferenceType = ProductMatrixReferenceType.Sales
                }).FirstOrDefault();

            // If product doesn't exist, we will just create it
            if (existing_officeProduct == null)
            {
                existing_officeProduct = officeProductService.Create(cheapest_officeProduct);

                foreach (var productMatrixLine in cheapest_officeProduct.ProductMatrixLines)
                {
                    productMatrixLine.OfficeProductId = existing_officeProduct.Id;
                    productMatrixService.Create(productMatrixLine);
                }
            }
            else
            {
                // update lines
                var lines = productMatrixService.GetProductMatrixBySpecification(new ProductMatrixLineSpecification()
                {
                    OfficeProductId = existing_officeProduct.Id
                });

                foreach (var productMatrixLine in lines)
                {
                    productMatrixService.Delete(productMatrixLine);
                }

                foreach (var productMatrixLine in cheapest_officeProduct.ProductMatrixLines)
                {
                    productMatrixService.Create(productMatrixLine);
                }
            }

        }

        private decimal CalculateSalesPrice(decimal value, ProductMatrixLineType lineType)
        {
            if (lineType == ProductMatrixLineType.Postage)
            {
                return value;
            }
            decimal adderPercentage = 15.0m / 100.0m;
            return value * (1 + adderPercentage);
        }

        #endregion
    }
}
