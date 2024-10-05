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
            Product prod3 = prods[3];
            OrderedProduct op3 = new OrderedProduct();
            op3.Product = prod3; op3.Quantity = 3;

            Order order3 = new Order(orderId++, userId);
            op3.Order = order3; order3.OrderedProducts.Add(op3);
            orders.Add(order3);

            context.Orders.AddRange(orders);
            context.Entry(prod3).State = EntityState.Added;
        }
    }
}