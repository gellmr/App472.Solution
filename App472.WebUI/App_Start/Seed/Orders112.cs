using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace App472.WebUI.App_Start
{
    public static class Orders112
    {
        public static void AddToContext(ref IList<Order> orders, ref IList<InStockProduct> prods, ref IDDBContext context, ref Int32 orderId)
        {
            AppUser user = IDDBExtensions.Users[1]; // get second user from our seed
            string userId = user.Id; // 112;

            TimeSpan PerthUtcPlus8 = new TimeSpan(8, 0, 0);
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yest = now.AddDays(-1);
            DateTimeOffset yesterday = new DateTimeOffset(yest.Year, yest.Month, yest.Day, 13,12,0, PerthUtcPlus8); // 1.12 pm

            Order order3 = new Order(orderId++, userId, null);
            order3.AppUser = user;
            order3.UserID = userId;

            InStockProduct prod3 = prods[3];
            OrderedProduct op3 = new OrderedProduct();

            op3.InStockProduct = prod3; op3.Quantity = 1;
            op3.Order = order3; order3.OrderedProducts.Add(op3);
            order3.OrderPlacedDate = yesterday;
            order3.PaymentReceivedDate = yesterday.AddMinutes(10);
            order3.ReadyToShipDate = yesterday.AddMinutes(10);
            order3.ShipDate = null;
            order3.ReceivedDate = null;
            order3.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order3.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order3.OrderStatus = "OrderPlaced";
            orders.Add(order3);

            context.Orders.AddRange(orders);
            context.Entry(prod3).State = EntityState.Added;
        }
    }
}