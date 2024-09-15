using System.Collections.Generic;
using App472.Domain.Entities;

namespace App472.WebUI.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set;}
        public PagingInfo PagingInfo { get; set;}
        public string CurrentCategory{get; set;}
    }
}