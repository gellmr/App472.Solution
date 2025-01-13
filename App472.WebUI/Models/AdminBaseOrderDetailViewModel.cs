using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{

    public class AdminBaseOrderDetailViewModel: AdminViewModel
    {
        public Int32 OrderID { get; set; }
        public IList<OrderedProduct> OrderedProducts { get; set; }

        public Nullable<DateTimeOffset> OrderPlacedDate { get; set; }
        public Nullable<DateTimeOffset> PaymentReceivedDate { get; set; }
        public Nullable<DateTimeOffset> ReadyToShipDate { get; set; }
        public Nullable<DateTimeOffset> ShipDate { get; set; }
        public Nullable<DateTimeOffset> ReceivedDate { get; set; }

        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public ShippingState OrderStatus { get; set; }

        public decimal TotalCost { get {
            decimal sum = 0;
            foreach(var op in OrderedProducts){ sum += (op.Product.Price * op.Quantity); }
            return sum;
        }}
    }
}