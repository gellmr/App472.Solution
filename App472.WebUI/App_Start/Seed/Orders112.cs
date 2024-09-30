using App472.Domain.Concrete;
using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace App472.WebUI.App_Start
{
    public static class Orders112
    {
        public static void AddToContext(ref IList<Order> orders, ref IList<Product> prods, ref EFDBContext context, ref Int32 orderId)
        {
            Int32 userId = 112;
            Product prod0 = prods[2];

            orders.Add(new Order(orderId++, userId, new List<Product> { prod0 }));

            context.Orders.AddRange(orders);
            context.Entry(prod0).State = EntityState.Added;
        }
    }
}