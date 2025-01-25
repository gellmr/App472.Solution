using App472.WebUI.Models;
using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace App472.WebUI.App_Start
{
    public static class Orders111
    {
        public static void AddToContext(ref IList<Order> orders, ref IList<Product> prods, ref IDDBContext context, ref Int32 orderId)
        {
            AppUser user = IDDBExtensions.Users[0]; // get first user from our seed
            string userId = user.Id; // 111;
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset yesterday = now.AddDays(-1);

            Order order1 = new Order(orderId++, userId, null);
            order1.AppUser = user;
            order1.UserID = userId;

            Product prod1 = prods[0];
            Product prod2 = prods[1];
            Product prod3 = prods[2];
            OrderedProduct op1 = new OrderedProduct();
            OrderedProduct op2 = new OrderedProduct();
            OrderedProduct op3 = new OrderedProduct();
            op1.Product = prod1; op1.Quantity = 1;
            op2.Product = prod2; op2.Quantity = 2;
            op3.Product = prod3; op3.Quantity = 1;
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
            context.Entry(prod1).State = EntityState.Added;
            context.Entry(prod2).State = EntityState.Added;
            context.Entry(prod3).State = EntityState.Added;

            Order order2 = new Order(orderId++, userId, null);
            Product prod4 = prods[3];
            Product prod5 = prods[4];
            OrderedProduct op4 = new OrderedProduct();
            OrderedProduct op5 = new OrderedProduct();
            op4.Product = prod4; op4.Quantity = 2;
            op5.Product = prod5; op5.Quantity = 2;
            op4.Order = order2; order2.OrderedProducts.Add(op4);
            op5.Order = order2; order2.OrderedProducts.Add(op5);
            order2.OrderPlacedDate = yesterday;
            order2.PaymentReceivedDate = yesterday;
            order2.ReadyToShipDate = now;
            order2.ShipDate = null;
            order2.ReceivedDate = null;
            order2.BillingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order2.ShippingAddress = "20 Enterprise Ave, Two Rocks, Perth Western Australia";
            order2.OrderStatus = "ReadyToShip";
            orders.Add(order2);
            context.Entry(prod4).State = EntityState.Added;
            context.Entry(prod5).State = EntityState.Added;

            context.Orders.AddRange(orders);
        }
    }
}