using App472.WebUI.Domain.Entities;
using System.Collections.Generic;

namespace App472.WebUI.Domain.Abstract
{
    public interface IProductsRepository
    {
        IEnumerable<InStockProduct> Products { get; }
        void SaveProduct(InStockProduct product);
        InStockProduct DeleteProduct(int productId);
    }
}