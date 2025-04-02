using App472.WebUI.Domain.Entities;
using System.Collections.Generic;

namespace App472.WebUI.Domain.Abstract
{
    public interface IProductsRepository
    {
        IEnumerable<InStockProduct> InStockProducts { get; }
        void SaveProduct(InStockProduct product);
        InStockProduct DeleteProduct(int productId);
    }
}