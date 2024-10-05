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
        void SaveOrder(Order order);
        void DeleteOrderedProduct(Int32 ProductID, Int32 OrderID);
    }
}
