using System;
using System.Collections.Generic;

namespace App472.Domain.Entities
{
    public enum ShippingState
    {
        NotYetPlaced,
        OrderPlaced,
        PaymentReceived,
        ReadyToShip,
        Shipped,
        Received
    }

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

        public Order(int orderID, int userID)
        {
            UserID = userID;
            OrderID = orderID;
            OrderedProducts = new List<OrderedProduct>();
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

        public static ShippingState ParseShippingState(string str)
        {
            ShippingState myState;
            try{
                if (Enum.TryParse(str, out myState)){
                    switch (myState)
                    {
                        case ShippingState.NotYetPlaced: return ShippingState.NotYetPlaced; break;
                        case ShippingState.OrderPlaced: return ShippingState.OrderPlaced; break;
                        case ShippingState.PaymentReceived: return ShippingState.PaymentReceived; break;
                        case ShippingState.ReadyToShip: return ShippingState.ReadyToShip; break;
                        case ShippingState.Shipped: return ShippingState.Shipped; break;
                        case ShippingState.Received: return ShippingState.Received; break;
                    }
                }
                throw new Exception("could not parse string as ShippingState enum");
            }
            catch (Exception e){
                throw e;
            }
        }

        public static string ParseShippingState(ShippingState myState)
        {
            string myString = "";
            switch (myState)
            {
                case ShippingState.NotYetPlaced: myString = "NotYetPlaced"; break;
                case ShippingState.OrderPlaced: myString = "OrderPlaced"; break;
                case ShippingState.PaymentReceived: myString = "PaymentReceived"; break;
                case ShippingState.ReadyToShip: myString = "ReadyToShip"; break;
                case ShippingState.Shipped: myString = "Shipped"; break;
                case ShippingState.Received: myString = "Received"; break;
            }
            return myString;
        }
    }
}
