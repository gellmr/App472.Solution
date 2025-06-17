using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App472.WebUI.Domain.Entities
{
    public class OrderPayment
    {
        [Key]
        public Nullable<Int32> ID { get; set; } // Primary key



        [ForeignKey("OrderID")] // Use the value of OrderID as foreign key to the Order table.
        public virtual Order Order { get; set; } // Navigation property.
        public Nullable<Int32> OrderID { get; set; } // Foreign key value to use, for Order table.



        public Nullable<Decimal> Amount { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}