using System;

namespace App472.WebUI.Domain.Entities
{
    public class OrderedProduct
    {
        public Nullable<Int32> Id { get; set; } // set to null, then database will assign a value
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public Int32 Quantity { get; set; }
    }
}