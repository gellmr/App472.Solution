using App472.WebUI.Domain.Entities;
using System.Collections.Generic;

namespace App472.WebUI.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<InStockProduct> Products { get; set;}
        public PagingInfo PagingInfo { get; set;}
        public string CurrentCategory{get; set;}
    }
}