using System;
using System.Collections.Generic;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public partial class Branches
    {
        public Branches()
        {
            AspNetUsers = new HashSet<User>();
            Customers = new HashSet<Customers>();
            Dongles = new HashSet<Dongles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> AspNetUsers { get; set; }
        public virtual ICollection<Customers> Customers { get; set; }
        public virtual ICollection<Dongles> Dongles { get; set; }
    }
}
