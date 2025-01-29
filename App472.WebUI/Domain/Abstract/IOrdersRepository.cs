using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;

namespace App472.WebUI.Domain.Abstract
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> Orders { get; }
        void DeleteOrderedProduct(Int32 ProductID, Int32 OrderID);
        void UpdateOrderedProductLineQuantity(Int32 ProductID, Int32 OrderID, Int32 NewQty);
        void UpdateShippingStatus(Int32 OrderID, Int32 OrderStatus, Nullable<Decimal> PaymentAmount);
        void SaveOrder(Order order);
        bool ProductHasOrders(Int32 ProductID); // return true if this product exists in any orders.
    }
}