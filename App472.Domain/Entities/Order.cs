using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App472.Domain.Entities
{
    public class Order
    {
        public Nullable<Int32> UserID { get; set; } // set to null, then database will assign a value
        public Nullable<Int32> OrderID { get; set; } // set to null, then database will assign a value
        public virtual IList<OrderedProduct> OrderedProducts { get; set; }

        public Nullable<DateTimeOffset> OrderPlacedDate { get; set; }
        public Nullable<DateTimeOffset> PaymentReceivedDate { get; set; }
        public Nullable<DateTimeOffset> ReadyToShipDate { get; set; }
        public Nullable<DateTimeOffset> ShipDate { get; set; }
        public Nullable<DateTimeOffset> ReceivedDate { get; set; }

        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string OrderStatus { get; set; }

        public Order()
        {
            UserID = null;
            OrderID = null;
            OrderedProducts = new List<OrderedProduct>();
        }

        public Order(int orderID, int userID, IList<Product> products)
        {
            UserID = userID;
            OrderID = orderID;
            OrderedProducts = new List<OrderedProduct>();
            foreach (Product product in products)
            {
                // here we are relying on mssql to generate the id for the ordered product.
                OrderedProduct op = new OrderedProduct{Order = this, Product = product, Id = null};
                OrderedProducts.Add(op);
            }
        }

        public Decimal PriceTotal{
            get{
                Decimal sum = 0;
                foreach (OrderedProduct op in OrderedProducts){
                    sum += op.Product.Price;
                }
                return sum;
            }
        }
    }
}
