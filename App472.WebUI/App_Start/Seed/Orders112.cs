using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace App472.WebUI.App_Start
{
    public static class Orders112
    {
        public static void AddToContext(ref IList<Order> orders, ref IList<Product> prods, ref IDDBContext context, ref Int32 orderId)
        {
            AppUser user = IDDBExtensions.Users[1]; // get second user from our seed
            string userId = user.Id; // 112;
            Product prod3 = prods[3];
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yesterday = now.AddDays(-1);
            OrderedProduct op3 = new OrderedProduct();
            op3.Product = prod3; op3.Quantity = 3;

            Order order3 = new Order(orderId++, userId, null);
            order3.AppUser = user;
            order3.UserID = userId;

            op3.Order = order3; order3.OrderedProducts.Add(op3);
            order3.OrderPlacedDate = yesterday;
            order3.PaymentReceivedDate = yesterday;
            order3.ReadyToShipDate = now;
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