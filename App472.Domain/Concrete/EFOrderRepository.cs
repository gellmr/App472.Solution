using App472.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using App472.Domain.Entities;

namespace App472.Domain.Concrete
{
    public class EFOrderRepository : IOrdersRepository
    {
        private EFDBContext context = new EFDBContext();
        public IEnumerable<Order> Orders {
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
    }
}
