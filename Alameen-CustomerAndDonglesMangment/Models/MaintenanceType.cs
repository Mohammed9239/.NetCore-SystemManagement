using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public class MaintenanceType
    {
        public MaintenanceType()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
