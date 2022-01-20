using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alameen_CustomerAndDonglesMangment.VM
{
    public class UserVm
    {
        public string ID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string? FullName { get; set; }

        [Display(Name = "نوع الحساب")]
        public string? RoleID { get; set; }

        public IEnumerable<string>? roles { get; set; }

    }
}
