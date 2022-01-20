using System;
using System.Collections.Generic;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public partial class Cities
    {
        public Cities()
        {
            CustBrans = new HashSet<CustBrans>();
            Customers = new HashSet<Customers>();
            Representatives = new HashSet<Representatives>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }

        public virtual States State { get; set; }
        public virtual ICollection<CustBrans> CustBrans { get; set; }
        public virtual ICollection<Customers> Customers { get; set; }
        public virtual ICollection<Representatives> Representatives { get; set; }
    }
}
