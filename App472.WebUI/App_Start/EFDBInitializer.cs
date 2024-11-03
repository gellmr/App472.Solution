﻿using App472.Domain.Concrete;
using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace App472.WebUI.App_Start
{
    public class EFDBInitializer : DropCreateDatabaseAlways<EFDBContext>
    {
        protected override void Seed(EFDBContext context)
        {
            // Populate products
            IList<Product> products = new List<Product>();
            ProductsWater.Get(ref products);
            ProductsSoccer.Get(ref products);
            ProductsChess.Get(ref products);
            context.Products.AddRange(products);

            // Populate orders
            IList<Order> orders = new List<Order>();
            Int32 orderIdStart = 1;  // MSSQL auto increment starts at 1 for orderId
            Orders111.AddToContext(ref orders, ref products, ref context, ref orderIdStart);
            Orders112.AddToContext(ref orders, ref products, ref context, ref orderIdStart);

            IList<Guest> guests = new List<Guest>();
            Guid guestID = Guid.NewGuid();
            guests.Add(new Guest{
                Id = guestID,
                FirstName = "Dye",
                LastName = "McDonald",
                Email = "guest-113@gmail.com",
                Orders = Orders113.GetOrders(ref products, ref context, ref orderIdStart, guestID)
            });
            context.Guests.AddRange(guests);

            base.Seed(context);
        }
    }
}