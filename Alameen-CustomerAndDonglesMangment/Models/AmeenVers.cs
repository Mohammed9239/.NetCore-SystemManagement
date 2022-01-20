using System;
using System.Collections.Generic;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public partial class AmeenVers
    {
        public AmeenVers()
        {
            Dongles = new HashSet<Dongles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Dongles> Dongles { get; set; }
    }
}
