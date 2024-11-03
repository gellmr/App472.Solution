using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

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

        public Decimal QuantityTotal{
            get{
                Decimal sum = 0;
                foreach (OrderedProduct op in OrderedProducts){
                    sum += op.Quantity;
                }
                return sum;
            }
        }

        public Decimal PriceTotal{
            get{
                Decimal sum = 0;
                foreach (OrderedProduct op in OrderedProducts){
                    sum += (op.Product.Price * op.Quantity);
                }
                return sum;
            }
        }

        public static string ParseAddress(ShippingDetails shippingInfo)
        {
            return  shippingInfo.Line1 + " " +
                    shippingInfo.Line2 + " " +
                    shippingInfo.Line3 + " " +
                    shippingInfo.City + " " +
                    shippingInfo.State + " " +
                    shippingInfo.Country + " " +
                    shippingInfo.Zip;
        }

        public static ShippingState ParseShippingState(string str)
        {
            ShippingState myState;
            ShippingState state = ShippingState.NotYetPlaced;
            try
            {
                if (Enum.TryParse(str, out myState)){
                    switch (myState)
                    {
                        case ShippingState.NotYetPlaced: state = ShippingState.NotYetPlaced; break;
                        case ShippingState.OrderPlaced: state = ShippingState.OrderPlaced; break;
                        case ShippingState.PaymentReceived: state = ShippingState.PaymentReceived; break;
                        case ShippingState.ReadyToShip: state = ShippingState.ReadyToShip; break;
                        case ShippingState.Shipped: state = ShippingState.Shipped; break;
                        case ShippingState.Received: state = ShippingState.Received; break;
                    }
                    return state;
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

        // Get an object to convert to JSON, for responding to ajax requests.
        public OrderDetailUpdateDTO GetUpdateDTO(){
            OrderDetailUpdateDTO response = new OrderDetailUpdateDTO();
            response.QuantityTotal = QuantityTotal;
            response.PriceTotal = PriceTotal;
            response.Rows = new List<OrderDetailUpdateRowDTO>();
            foreach (OrderedProduct p in OrderedProducts){
                OrderDetailUpdateRowDTO row = new OrderDetailUpdateRowDTO();
                row.ProductID = p.Product.ProductID;
                row.ProductName = p.Product.Name;
                row.UnitPrice = p.Product.Price;
                row.Quantity = p.Quantity;
                row.Cost = p.Product.Price * p.Quantity;
                row.Category = p.Product.Category;
                response.Rows.Add(row);
            }
            return response;
        }
    }

    // This class is converted into JSON for responding to the Detail page update requests.
    public class OrderDetailUpdateRowDTO
    {
        public Int32 ProductID{get; set; }
        public string ProductName { get; set; }
        public Decimal UnitPrice { get; set; }
        public Int32 Quantity { get; set; }
        public Decimal Cost { get; set; }
        public string Category { get; set; }
    }

    // This class is converted into JSON for responding to the Detail page update requests.
    public class OrderDetailUpdateDTO
    {
        public List<OrderDetailUpdateRowDTO> Rows{ get; set; }
        public Decimal QuantityTotal { get; set; }
        public Decimal PriceTotal { get; set; }
    }
}
