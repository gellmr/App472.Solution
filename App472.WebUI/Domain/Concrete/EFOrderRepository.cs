using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App472.WebUI.Domain.Concrete
{
    public class EFOrderRepository : IOrdersRepository
    {
        private IDDBContext context = new IDDBContext();
        public IEnumerable<Order> Orders
        {
            get { return context.Orders; }
        }

        public void DeleteOrderedProduct(Int32 ProductID, Int32 OrderID)
        {
            Order order = context.Orders.FirstOrDefault(o => o.OrderID == OrderID);
            OrderedProduct op = order.OrderedProducts.FirstOrDefault(p => p.Product.ProductID == ProductID);
            context.OrderedProducts.Remove(op);
            order.OrderedProducts.Remove(op);
            context.SaveChanges();
        }

        public void UpdateOrderedProductLineQuantity(Int32 ProductID, Int32 OrderID, Int32 NewQty)
        {
            OrderedProduct op = context.OrderedProducts.FirstOrDefault(p => p.Product.ProductID == ProductID && p.Order.OrderID == OrderID);
            op.Quantity = NewQty;
            context.SaveChanges();
        }

        public void UpdateShippingStatus(Int32 OrderID, Int32 OrderStatus)
        {
            Order order = context.Orders.FirstOrDefault(o => o.OrderID == OrderID);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            Domain.Entities.ShippingState myEnum = (Domain.Entities.ShippingState)OrderStatus;
            order.OrderStatus = Order.ParseShippingState(myEnum);
            context.SaveChanges();
        }

        public void SaveOrder(Order order)
        {
            // Could we possibly reduce this to one database call?
            if (context.Orders.Any(o => o.OrderID == order.OrderID)) // first call
            {
                // record already exists. Update
                Order dbEntry = context.Orders.FirstOrDefault(o => o.OrderID == order.OrderID); // second call

                dbEntry.UserID = order.UserID;
                dbEntry.OrderedProducts = order.OrderedProducts;

                dbEntry.OrderPlacedDate = order.OrderPlacedDate;
                dbEntry.PaymentReceivedDate = order.PaymentReceivedDate;
                dbEntry.ReadyToShipDate = order.ReadyToShipDate;
                dbEntry.ShipDate = order.ShipDate;
                dbEntry.ReceivedDate = order.ReceivedDate;

                dbEntry.BillingAddress = order.BillingAddress;
                dbEntry.ShippingAddress = order.ShippingAddress;
                dbEntry.OrderStatus = order.OrderStatus;

                dbEntry.OrderedProducts = order.OrderedProducts; // update ordered products. This will set FK to null if removed
                context.SaveChanges();
            }
            else
            {
                // create new record
                context.Orders.Add(order);
                foreach (OrderedProduct op in order.OrderedProducts)
                {
                    context.OrderedProducts.Add(op);
                }
                context.SaveChanges();
            }
        }

        // return true if this product exists in any orders.
        public bool ProductHasOrders(Int32 productId)
        {
            if (context.OrderedProducts.Any(op => op.Product.ProductID == productId))
            {
                return true;
            }
            return false;
        }
    }
}
