using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminUserOrderDetailViewModel: AdminViewModel
    {
        public Int32 UserId { get; set; }
        public Int32 OrderID { get; set; }
        public IList<OrderedProduct> OrderedProducts { get; set; }
        public decimal TotalCost { get {
            decimal sum = 0;
            foreach(var op in OrderedProducts){ sum += op.Product.Price; }
            return sum;
        }}
    }
}