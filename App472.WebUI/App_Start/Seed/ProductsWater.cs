using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.App_Start
{
    public static class ProductsWater
    {
        public static void Get(ref IList<InStockProduct> products)
        {
            Int32 startId = 111;
            string category = "Water Sports";

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Polycarbon Injection Molded River Kayak",
                Description = "Ready to tame the wilderness? Travel by boat with this one person kayak.",
                Category = category,
                Price = 350
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Life Jacket",
                Description = "Coastmaster TM Duratex All Weather Sea Life Jacket. For Tactical sea advantage.",
                Category = category,
                Price = 100
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "White Water Rafting Helmet",
                Description = "Waterproof and Durable, this helmet comes in 12 colors.",
                Category = category,
                Price = 90
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Single Paddle",
                Description = "Right or Left handed paddle, for kayaking or canoeing.",
                Category = category,
                Price = 40
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Double Paddle",
                Description = "Double-ended paddle for kayaking or canoeing.",
                Category = category,
                Price = 50
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Camping Towel",
                Description = "Deluxe El Capitan All-Weather Towel... For drying off after water activities",
                Category = category,
                Price = 15
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Sunscreen SPF 50+",
                Description = "Extreme Sports Edition fast drying activewear sunblock",
                Category = category,
                Price = 25
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Waterproof Equipment Bag",
                Description = "Carry your gear in this tough and compact waterproof bag",
                Category = category,
                Price = 80
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Drink Bottle",
                Description = "Dont forget to drink water, while your out doing water sports.",
                Category = category,
                Price = 20
            });
        }
    }
}