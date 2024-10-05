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
            Product prod1 = prods[0];
            Product prod2 = prods[1];
            Product prod3 = prods[2];
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yesterday = now.AddDays(-1);
            OrderedProduct op1 = new OrderedProduct();
            OrderedProduct op2 = new OrderedProduct();
            OrderedProduct op3 = new OrderedProduct();
            op1.Product = prod1; op1.Quantity = 1;
            op2.Product = prod2; op2.Quantity = 2;
            op3.Product = prod3; op3.Quantity = 1;

            Order order1 = new Order(orderId++, userId);
            op1.Order = order1; order1.OrderedProducts.Add(op1);
            op2.Order = order1; order1.OrderedProducts.Add(op2);
            op3.Order = order1; order1.OrderedProducts.Add(op3);

            order1.OrderPlacedDate = yesterday;
            order1.PaymentReceivedDate = yesterday;
            order1.ReadyToShipDate = now;
            order1.ShipDate = null;
            order1.ReceivedDate = null;
            order1.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order1.OrderStatus = "ReadyToShip";
            orders.Add(order1);

            context.Orders.AddRange(orders);
            context.Entry(prod1).State = EntityState.Added;
            context.Entry(prod2).State = EntityState.Added;
        }
    }
}