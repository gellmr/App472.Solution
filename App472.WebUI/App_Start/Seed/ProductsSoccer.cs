﻿using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.App_Start
{
    public static class ProductsSoccer
    {
        public static void Get(ref IList<InStockProduct> products)
        {
            Int32 startId = 211;
            string category = "Soccer";

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Soccer Ball",
                Description = "FIFA approved size and weight",
                Category = category,
                Price = 35
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Corner Flags",
                Description = "Give some flourish to your playing field with these coloured corner flags",
                Category = category,
                Price = 25
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Referee Whisle",
                Description = "For serious games, call it with this chrome Referee Whistle.",
                Category = category,
                Price = 12
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Red and Yellow Cards",
                Description = "Official size and colour, waterproof high visibility retroflective coating.",
                Category = category,
                Price = 10
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Soccer Stadium",
                Description = "Flat packed 30,000 seat stadium.",
                Category = category,
                Price = 80000
            });

            products.Add(new InStockProduct()
            {
                ID = startId++,
                Name = "Soccer Goals",
                Description = "One lightweight aluminium standard size impact foam coated soccer goal with netting.",
                Category = category,
                Price = 1000
            });
        }
    }
}