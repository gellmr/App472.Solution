using App472.Domain.Concrete;
using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace App472.WebUI.App_Start
{
    public static class Orders111
    {
        public static void AddToContext(ref IList<Order> orders, ref IList<Product> prods, ref EFDBContext context, ref Int32 orderId)
        {
            Int32 userId = 111;
            Product prod0 = prods[0];
            Product prod1 = prods[1];
            Product prod2 = prod1;
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yesterday = now.AddDays(-1);

            Order order1 = new Order(orderId++, userId, new List<Product> { prod0, prod1, prod2 });
            order1.OrderPlacedDate = yesterday;
            order1.PaymentReceivedDate = yesterday;
            order1.ReadyToShipDate = now;
            order1.ShipDate = null;
            order1.ReceivedDate = null;
            order1.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.OrderStatus = "Ready to Ship";
            orders.Add(order1);

            Order order2 = new Order(orderId++, userId, new List<Product> { prod0 });
            orders.Add(order2);

            context.Orders.AddRange(orders);
            context.Entry(prod0).State = EntityState.Added;
            context.Entry(prod1).State = EntityState.Added;
            context.Entry(prod2).State = EntityState.Added;
        }
    }
}