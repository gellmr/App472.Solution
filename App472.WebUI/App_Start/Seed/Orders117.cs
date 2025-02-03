using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace App472.WebUI.App_Start
{
    public static class Orders117
    {
        public static void AddToContext(ref IList<Order> orders, ref IList<InStockProduct> prods, ref IDDBContext context, ref Int32 orderId)
        {
            AppUser user = IDDBExtensions.Users[4]; // User 4 is 117
            string userId = user.Id;

            TimeSpan PerthUtcPlus8 = new TimeSpan(8, 0, 0);
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yest = now.AddDays(-1);
            DateTimeOffset yesterday = new DateTimeOffset(yest.Year, yest.Month, yest.Day, 13, 12, 0, PerthUtcPlus8); // 1.12 pm

            Order ord = new Order(orderId++, userId, null);
            ord.AppUser = user;
            ord.UserID = userId;

            InStockProduct prod = prods[19];
            OrderedProduct op = new OrderedProduct();

            op.Product = prod; op.Quantity = 22;
            op.Order = ord; ord.OrderedProducts.Add(op);
            ord.OrderPlacedDate = yesterday;
            ord.PaymentReceivedDate = yesterday.AddMinutes(10);
            ord.ReadyToShipDate = yesterday.AddMinutes(10);
            ord.ShipDate = null;
            ord.ReceivedDate = null;
            ord.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            ord.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            ord.OrderStatus = "ReadyToShip";
            orders.Add(ord);

            context.Orders.AddRange(orders);
            context.Entry(prod).State = EntityState.Added;
        }
    }
}