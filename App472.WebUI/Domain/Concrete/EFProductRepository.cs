using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System.Collections.Generic;

namespace App472.WebUI.Domain.Concrete
{
    public class EFProductRepository : IProductsRepository
    {
        private IDDBContext context = new IDDBContext();
        public IEnumerable<InStockProduct> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(InStockProduct product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                InStockProduct dbEntry = context.Products.Find(product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }
            context.SaveChanges();
        }

        public InStockProduct DeleteProduct(int productId)
        {
            InStockProduct dbEntry = context.Products.Find(productId);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}