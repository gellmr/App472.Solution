using App472.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App472.Domain.Entities;

namespace App472.Domain.Concrete
{
    public class EFOrderRepository : IOrdersRepository
    {
        private EFDBContext context = new EFDBContext();
        public IEnumerable<Order> Orders {
            get { return context.Orders; }
        }
    }
}
