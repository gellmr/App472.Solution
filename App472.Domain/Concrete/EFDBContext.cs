using App472.Domain.Entities;
using System.Data.Entity;

namespace App472.Domain.Concrete
{
    public class EFDBContext : DbContext
    {
        public EFDBContext():base("EFConnection") {}

        public DbSet<Product> Products { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
