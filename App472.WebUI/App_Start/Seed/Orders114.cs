using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.App_Start
{
    public static class Orders114
    {
        public static IList<Order> GetOrders(ref IList<InStockProduct> prods, ref IDDBContext context, ref Int32 orderId, Guid guestID)
        {
            // This user is a Guest, they dont have an AppUser Id
            IList<Order> orders = new List<Order>();

            TimeSpan PerthUtcPlus8 = new TimeSpan(8, 0, 0);
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yest = now.AddDays(-4);
            DateTimeOffset yesterday = new DateTimeOffset(yest.Year, yest.Month, yest.Day, 13, 13, 0, PerthUtcPlus8); // 1.13 pm

            Order order1 = new Order(orderId++, null, guestID);
            Order order2 = new Order(orderId++, null, guestID);

            InStockProduct prod1 = prods[5];
            InStockProduct prod2 = prods[6];

            InStockProduct prod3 = prods[7];
            InStockProduct prod4 = prods[8];

            OrderedProduct op1 = new OrderedProduct();
            OrderedProduct op2 = new OrderedProduct();

            OrderedProduct op3 = new OrderedProduct();
            OrderedProduct op4 = new OrderedProduct();

            op1.InStockProduct = prod1; op1.Quantity = 1;
            op2.InStockProduct = prod2; op2.Quantity = 2;
            op3.InStockProduct = prod3; op3.Quantity = 3;
            op4.InStockProduct = prod4; op4.Quantity = 1;

            op1.Order = order1; order1.OrderedProducts.Add(op1);
            op2.Order = order1; order1.OrderedProducts.Add(op2);

            op3.Order = order2; order2.OrderedProducts.Add(op3);
            op4.Order = order2; order2.OrderedProducts.Add(op4);

            order1.OrderPlacedDate = yesterday;
            order1.PaymentReceivedDate = yesterday.AddMinutes(10);
            order1.ReadyToShipDate = yesterday.AddMinutes(10);
            order1.ShipDate = null;
            order1.ReceivedDate = null;
            order1.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.OrderStatus = "OrderPlaced";
            orders.Add(order1);

            order2.OrderPlacedDate = yesterday;
            order2.PaymentReceivedDate = yesterday.AddMinutes(10);
            order2.ReadyToShipDate = yesterday.AddMinutes(10);
            order2.ShipDate = null;
            order2.ReceivedDate = null;
            order2.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order2.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order2.OrderStatus = "Shipped";
            orders.Add(order2);

            context.Orders.AddRange(orders);
            return orders;
        }
    }
}