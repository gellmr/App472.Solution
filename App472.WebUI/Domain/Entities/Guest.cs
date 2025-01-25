using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace App472.WebUI.Domain.Entities
{
    public class Guest
    {
        public Nullable<Guid> Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual IList<Order> Orders { get; set; }

        [NotMapped]
        public string FullName {
            get{
                return FirstName + " " + LastName;
            }
        }
    }
}
