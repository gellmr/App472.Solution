using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            Order order = context.Orders.FirstOrDefault(o => o.ID == OrderID);
            OrderedProduct op = order.OrderedProducts.FirstOrDefault(p => p.InStockProduct.ID == ProductID);
            context.OrderedProducts.Remove(op);
            order.OrderedProducts.Remove(op);
            context.SaveChanges();
        }

        public void UpdateOrderedProductLineQuantity(Int32 ProductID, Int32 OrderID, Int32 NewQty)
        {
            OrderedProduct op = context.OrderedProducts.FirstOrDefault(p => p.InStockProduct.ID == ProductID && p.Order.ID == OrderID);
            op.Quantity = NewQty;
            context.SaveChanges();
        }

        public void UpdateShippingStatus(Int32 OrderID, Int32 OrderStatus, Nullable<Decimal> PaymentAmount)
        {
            Order order = context.Orders.FirstOrDefault(o => o.ID == OrderID);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            Domain.Entities.ShippingState myEnum = (Domain.Entities.ShippingState)OrderStatus;
            order.OrderStatus = Order.ParseShippingState(myEnum);
            if (myEnum == ShippingState.PaymentReceived && PaymentAmount.HasValue)
            {
                OrderPayment payment = new OrderPayment{ Amount=PaymentAmount.Value, Date=DateTimeOffset.Now, Order=order };
                context.OrderPayments.Add(payment);
            }
            context.SaveChanges();
        }

        public void SaveOrder(Order order)
        {
            bool exists = context.Orders.Any(o => o.ID == order.ID);

            if (exists)
            {
                // Update
                Order dbEntry = context.Orders.First(o => o.ID == order.ID);

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

                dbEntry.OrderedProducts = order.OrderedProducts;

                context.SaveChanges();
            }
            else
            {
                // create new record
                context.Orders.Add(order);
                foreach (OrderedProduct op in order.OrderedProducts)
                {
                    // We need to create new OrderedProduct entities in the database.

                    // But the InStockProduct objects already exist in the database, we dont want to create these.
                    // Mark them as Unchanged, to prevent EF6 creating duplicate objects.

                    // Tell EF6 to track the InStockProduct entity, since it already exists in database. Dont create it.
                    InStockProduct ip = op.InStockProduct;
                    context.Entry(ip).State = EntityState.Unchanged;

                    // Create the ordered product record
                    context.OrderedProducts.Add(op);
                }
                context.SaveChanges();
            }
        }

        // return true if this product exists in any orders.
        public bool ProductHasOrders(Int32 productId)
        {
            if (context.OrderedProducts.Any(op => op.InStockProduct.ID == productId))
            {
                return true;
            }
            return false;
        }
    }
}
