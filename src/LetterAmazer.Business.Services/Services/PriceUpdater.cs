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
            var cheapest_officeProduct = officeProductService.GetOfficeProductById(lowestOfficeProductId);
            cheapest_officeProduct.ReferenceType = ProductMatrixReferenceType.Sales;

            foreach (var productMatrixLine in cheapest_officeProduct.ProductMatrixLines)
            {
                productMatrixLine.BaseCost = CalculateSalesPrice(productMatrixLine.BaseCost,
                    productMatrixLine.LineType);
            }
            officeProductService.Create(cheapest_officeProduct);
            foreach (var productMatrixLine in cheapest_officeProduct.ProductMatrixLines)
            {
                productMatrixLine.OfficeProductId = cheapest_officeProduct.Id;
                productMatrixService.Create(productMatrixLine);
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

        #endregion
    }
}
