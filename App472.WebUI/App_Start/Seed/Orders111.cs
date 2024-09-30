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
        public static void AddToContext(ref IList<Order> orders, ref IList<Product> prods, ref EFDBContext context)
        {
            Int32 userId = 111;
            Int32 orderId = 1; // MSSQL auto increment starts at 1
            Product prod0 = prods[0];
            Product prod1 = prods[1];

            orders.Add( new Order ( orderId++, userId, new List<Product> { prod0, prod1 } ));
            orders.Add( new Order ( orderId++, userId, new List<Product> { prod0        } ));

            context.Orders.AddRange(orders);
            context.Entry(prod0).State = EntityState.Added;
            context.Entry(prod1).State = EntityState.Added;
        }
    }
}