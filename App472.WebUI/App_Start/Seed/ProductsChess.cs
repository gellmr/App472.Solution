using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.App_Start
{
    public static class ProductsChess
    {
        public static void Get(ref IList<Product> products)
        {
            Int32 startId = 311;
            string category = "Chess";

            products.Add(new Product()
            {
                ProductID = startId++,
                Name = "Thinking Cap",
                Description = "Improve your concentration by 4% with this stylish sports cap. Comes in deep blue.",
                Category = category,
                Price = 15
            });

            products.Add(new Product()
            {
                ProductID = startId++,
                Name = "Chess Board",
                Description = "Non-reflective and slip resistant.",
                Category = category,
                Price = 25
            });

            products.Add(new Product()
            {
                ProductID = startId++,
                Name = "Speed Chess Timer",
                Description = "Has a digital timer display on both sides, and supercollider toggle button on top. Silent and durable. Batteries not included.",
                Category = category,
                Price = 50
            });

            products.Add(new Product()
            {
                ProductID = startId++,
                Name = "Chess Pieces - Full Set",
                Description = "Full set of chess pieces. Charcoal Black / Frost White.",
                Category = category,
                Price = 90
            });

            products.Add(new Product()
            {
                ProductID = startId++,
                Name = "Individual Chess Piece",
                Description = "Single chess pieces available. Charcoal Black / Frost White.",
                Category = category,
                Price = 10
            });

            products.Add(new Product()
            {
                ProductID = startId++,
                Name = "Unsteady Chair",
                Description = "Secretly give your opponent a disadvantage",
                Category = category,
                Price = 45
            });

            products.Add(new Product()
            {
                ProductID = startId++,
                Name = "Holo Chess",
                Description = "As seen in Star Wars: A New Hope",
                Category = category,
                Price = 22000
            });
        }
    }
}