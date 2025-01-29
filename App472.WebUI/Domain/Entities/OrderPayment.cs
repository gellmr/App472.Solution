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
        public Nullable<Int32> ID { get; set; } // primary key

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; } // navigation property.
        public Nullable<Int32> OrderID { get; set; } // foreign key

        public Nullable<Decimal> Amount { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}