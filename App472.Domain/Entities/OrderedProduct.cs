using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App472.Domain.Entities
{
    public class OrderedProduct
    {
        public Nullable<Int32> Id { get; set; } // set to null, then database will assign a value
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}