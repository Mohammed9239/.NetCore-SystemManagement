using System;
using System.Collections.Generic;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public partial class States
    {
        public States()
        {
            Cities = new HashSet<Cities>();
            CustBrans = new HashSet<CustBrans>();
            Customers = new HashSet<Customers>();
            Representatives = new HashSet<Representatives>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<CustBrans> CustBrans { get; set; }
        public virtual ICollection<Customers> Customers { get; set; }
        public virtual ICollection<Representatives> Representatives { get; set; }
    }
}
