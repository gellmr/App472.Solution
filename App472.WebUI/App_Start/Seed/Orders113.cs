using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;

namespace App472.WebUI.App_Start
{
    public static class Orders113
    {
        public static IList<Order> GetOrders(ref IList<Product> prods, ref IDDBContext context, ref Int32 orderId, Guid guestID)
        {
            // This user is a Guest, they dont have an AppUser Id
            IList<Order> orders = new List<Order>();

            TimeSpan PerthUtcPlus8 = new TimeSpan(8, 0, 0);
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yest = now.AddDays(-1);
            DateTimeOffset yesterday = new DateTimeOffset(yest.Year, yest.Month, yest.Day, 13, 13, 0, PerthUtcPlus8); // 1.13 pm

            Order order1 = new Order(orderId++, null, guestID);

            Product prod1 = prods[0];
            Product prod2 = prods[1];
            Product prod3 = prods[2];
            OrderedProduct op1 = new OrderedProduct();
            OrderedProduct op2 = new OrderedProduct();
            OrderedProduct op3 = new OrderedProduct();

            op1.Product = prod1; op1.Quantity = 1;
            op2.Product = prod2; op2.Quantity = 2;
            op3.Product = prod3; op3.Quantity = 3;
            op1.Order = order1; order1.OrderedProducts.Add(op1);
            op2.Order = order1; order1.OrderedProducts.Add(op2);
            op3.Order = order1; order1.OrderedProducts.Add(op3);
            order1.OrderPlacedDate = yesterday;
            order1.PaymentReceivedDate = yesterday.AddMinutes(10);
            order1.ReadyToShipDate = yesterday.AddMinutes(10);
            order1.ShipDate = null;
            order1.ReceivedDate = null;
            order1.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.OrderStatus = "OrderPlaced";
            orders.Add(order1);

            context.Orders.AddRange(orders);
            return orders;
        }
    }
}