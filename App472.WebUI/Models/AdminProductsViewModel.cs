using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminProductsViewModel:AdminViewModel
    {
        public IEnumerable<App472.Domain.Entities.Product> Products { get; set; }
    }
}