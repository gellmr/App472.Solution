using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace App472.WebUI.App_Start
{
    public static class Orders118
    {
        public static void AddToContext(ref IList<Order> orders, ref IList<InStockProduct> prods, ref IDDBContext context, ref Int32 orderId)
        {
            AppUser user = IDDBExtensions.Users[5]; // User 5 is 118
            string userId = user.Id;

            TimeSpan PerthUtcPlus8 = new TimeSpan(8, 0, 0);
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yest = now.AddDays(-1);
            DateTimeOffset yesterday = new DateTimeOffset(yest.Year, yest.Month, yest.Day, 13, 12, 0, PerthUtcPlus8); // 1.12 pm

            Order orderA = new Order(orderId++, userId, null);
            orderA.AppUser = user;
            orderA.UserID = userId;

            Order orderB = new Order(orderId++, userId, null);
            orderB.AppUser = user;
            orderB.UserID = userId;

            InStockProduct prodA = prods[15];
            InStockProduct prodB = prods[16];
            OrderedProduct opA = new OrderedProduct();
            OrderedProduct opB = new OrderedProduct();

            opA.Product = prodA; opA.Quantity = 2;
            opA.Order = orderA; orderA.OrderedProducts.Add(opA);

            opB.Product = prodB; opB.Quantity = 3;
            opB.Order = orderB; orderB.OrderedProducts.Add(opB);

            orderA.OrderPlacedDate = yesterday;
            orderA.PaymentReceivedDate = yesterday.AddMinutes(10);
            orderA.ReadyToShipDate = yesterday.AddMinutes(10);
            orderA.ShipDate = null;
            orderA.ReceivedDate = null;
            orderA.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            orderA.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            orderA.OrderStatus = "Received";
            orders.Add(orderA);

            orderB.OrderPlacedDate = yesterday;
            orderB.PaymentReceivedDate = yesterday.AddMinutes(10);
            orderB.ReadyToShipDate = yesterday.AddMinutes(10);
            orderB.ShipDate = null;
            orderB.ReceivedDate = null;
            orderB.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            orderB.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            orderB.OrderStatus = "Shipped";
            orders.Add(orderB);

            context.Orders.AddRange(orders);
            context.Entry(prodA).State = EntityState.Added;
        }
    }
}