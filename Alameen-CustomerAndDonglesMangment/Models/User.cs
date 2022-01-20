using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Tasks = new HashSet<Tasks>();
        }

        public string Address { get; set; }
        public int? BranchId { get; set; }
        public string FullName { get; set; }
        public string Jop { get; set; }
        public virtual Branches Branch { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }

    }
}
