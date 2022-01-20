using System;
using System.Collections.Generic;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public partial class Representatives
    {
        public Representatives()
        {
            Dongles = new HashSet<Dongles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int? Phone { get; set; }
        public int? CustomersId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }

        public virtual Cities City { get; set; }
        public virtual Customers Customers { get; set; }
        public virtual States State { get; set; }
        public virtual ICollection<Dongles> Dongles { get; set; }
    }
}
