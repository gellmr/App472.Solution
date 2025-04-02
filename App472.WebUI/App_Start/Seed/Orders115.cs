using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace App472.WebUI.App_Start
{
    public static class Orders115
    {
        public static void AddToContext(ref IList<Order> orders, ref IList<InStockProduct> prods, ref IDDBContext context, ref Int32 orderId)
        {
            AppUser user = IDDBExtensions.Users[2]; // 3rd user is 115
            string userId = user.Id;

            TimeSpan PerthUtcPlus8 = new TimeSpan(8, 0, 0);
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yest = now.AddDays(-1);
            DateTimeOffset yesterday = new DateTimeOffset(yest.Year, yest.Month, yest.Day, 13, 12, 0, PerthUtcPlus8); // 1.12 pm

            Order order1 = new Order(orderId++, userId, null);
            order1.AppUser = user;
            order1.UserID = userId;

            InStockProduct prod10 = prods[10];
            OrderedProduct op10 = new OrderedProduct();

            op10.InStockProduct = prod10; op10.Quantity = 4;
            op10.Order = order1; order1.OrderedProducts.Add(op10);
            order1.OrderPlacedDate = yesterday;
            order1.PaymentReceivedDate = yesterday.AddMinutes(10);
            order1.ReadyToShipDate = yesterday.AddMinutes(10);
            order1.ShipDate = null;
            order1.ReceivedDate = null;
            order1.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.OrderStatus = "ReadyToShip";
            orders.Add(order1);

            context.Orders.AddRange(orders);
            context.Entry(prod10).State = EntityState.Added;
        }
    }
}