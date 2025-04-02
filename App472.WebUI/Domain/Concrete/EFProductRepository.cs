using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System.Collections.Generic;

namespace App472.WebUI.Domain.Concrete
{
    public class EFProductRepository : IProductsRepository
    {
        private IDDBContext context = new IDDBContext();
        public IEnumerable<InStockProduct> InStockProducts
        {
            get { return context.InStockProducts; }
        }

        public void SaveProduct(InStockProduct product)
        {
            if (product.ID == 0)
            {
                context.InStockProducts.Add(product);
            }
            else
            {
                InStockProduct dbEntry = context.InStockProducts.Find(product.ID);
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
            InStockProduct dbEntry = context.InStockProducts.Find(productId);
            if (dbEntry != null)
            {
                context.InStockProducts.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}