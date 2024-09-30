using App472.Domain.Concrete;
using App472.Domain.Entities;
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
            Orders111.AddToContext(ref orders, ref products, ref context);

            base.Seed(context);
        }
    }
}