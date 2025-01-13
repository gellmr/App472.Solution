using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminProductsViewModel:AdminViewModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}