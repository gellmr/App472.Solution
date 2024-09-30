using App472.Domain.Abstract;
using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App472.Domain.Concrete
{
    public class EFOrderedProductsRepository : IOrderedProductsRepository
    {
        private EFDBContext context = new EFDBContext();
        public IEnumerable<OrderedProduct> OrderedProducts
        {
            get { return context.OrderedProducts; }
        }
    }
}