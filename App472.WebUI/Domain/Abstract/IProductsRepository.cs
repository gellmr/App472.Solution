using App472.WebUI.Domain.Entities;
using System.Collections.Generic;

namespace App472.WebUI.Domain.Abstract
{
    public interface IProductsRepository
    {
        IEnumerable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productId);
    }
}