﻿using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Domain.Shipping;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services.Shipping
{
    public class ShippingService:IShippingMethodService
    {

        private LetterAmazerEntities repository;
        private IFulfillmentPartnerService fulfillmentPartnerService;

        public ShippingService(LetterAmazerEntities repository, IFulfillmentPartnerService fulfillmentService)
        {
            this.repository = repository;
            this.fulfillmentPartnerService = fulfillmentService;
        }


        public List<ShippingMethod> GetShippingMethodsBySpecification(ShippingMethodSpecification shippingMethodSpecification)
        {
            var fulfillmentPartners = fulfillmentPartnerService.GetFulfillmentPartnersBySpecifications(new FulfillmentPartnerSpecification()
            {
               // CountryId = shippingMethodSpecification.CountryId
            });

            if (fulfillmentPartners == null || !fulfillmentPartners.Any())
            {
                return new List<ShippingMethod>();
            }

            var partner = fulfillmentPartners.FirstOrDefault();

            //var shippingOption = new ShippingMethod()
            //{
            //    FulfillmentPartner = partner,
            //    DeliveryOption = new DeliveryOption()
            //    {
            //        LetterQuatity = LetterQuatity.Normal,
            //        PrintQuality = PrintQuality.Normal,
            //        PrintSize = LetterSize.A4
            //    },
            //    ShippingPrice = new Price()
            //};

            return new List<ShippingMethod>() { };
        }
    }
}
