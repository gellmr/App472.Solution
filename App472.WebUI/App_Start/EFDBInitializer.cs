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
            IList<Product> products = new List<Product>();

            // populate products

            ProductsWater.Get(ref products);
            ProductsSoccer.Get(ref products);
            ProductsChess.Get(ref products);

            context.Products.AddRange(products);
            base.Seed(context);
        }
    }
}