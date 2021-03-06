﻿using System;
using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Letters;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public interface IOrderService
    {
        Order Create(Order order);

        Order Update(Order order);
        void UpdateByLetters(IEnumerable<Letter> letters);

        List<Order> GetOrderBySpecification(OrderSpecification specification);
        Order GetOrderById(int orderId);
        Order GetOrderById(Guid orderId);

        void Delete(Order order);

        List<OrderLine> GetOrderLinesBySpecification(OrderLineSpecification specification);

        void ReplenishOrderLines(Order order);
    }
}
