using AutoMapper;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.WebAPI.Common
{
    public static class AutomapperMaps
    {
        public static void CreateMaps()
        {
            Mapper.AllowNullDestinationValues = true;
            Mapper.Configuration.AllowNullCollections = true;

            #region Order

            Mapper.CreateMap<Order, OrderDTO>();
            Mapper.CreateMap<OrderDTO, Order>();

            Mapper.CreateMap<Customer, CustomerDTO>();
            Mapper.CreateMap<CustomerDTO, Customer>();

            Mapper.CreateMap<PartnerTransaction, PartnerTransactionDTO>();
            Mapper.CreateMap<PartnerTransactionDTO, PartnerTransaction>();

            Mapper.CreateMap<OrderLine, OrderLineDTO>();
            Mapper.CreateMap<OrderLineDTO, OrderLine>();
            #endregion
        }

    }
}