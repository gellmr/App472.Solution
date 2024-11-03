using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App472.Domain.Abstract
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> Orders { get; }
        void DeleteOrderedProduct(Int32 ProductID, Int32 OrderID);
        void UpdateOrderedProductLineQuantity(Int32 ProductID, Int32 OrderID, Int32 NewQty);
        void UpdateShippingStatus(Int32 OrderID, Int32 OrderStatus);
        void SaveOrder(Order order);
    }
}
