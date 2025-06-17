using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App472.WebUI.Domain.Entities
{
    public class OrderedProduct
    {
        [Key]
        public Nullable<Int32> ID { get; set; } // Primary key. Set to null, then database will assign a value


        [ForeignKey("OrderID")] // Use the value of OrderID as foreign key to the Orders table.
        public virtual Order Order { get; set; }  // Navigation property.
        public Nullable<Int32> OrderID { get; set; } // Foreign key value to use, for Orders table.


        [ForeignKey("InStockProductID")] // Use the value of InStockProductID as foreign key to the InStockProducts table.
        public virtual InStockProduct InStockProduct { get; set; } // Navigation property.
        public Int32 InStockProductID { get; set; } // Foreign key value to use, for InStockProducts table.


        public Int32 Quantity { get; set; }
    }
}