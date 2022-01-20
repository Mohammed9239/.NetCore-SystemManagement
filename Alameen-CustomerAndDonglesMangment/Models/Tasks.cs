using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public enum State
    {
        [Display(Name ="مكتمل")] Complete = 1,
        [Display(Name = "مكتمل جزئي")] PartiallyComplete = 2,
        [Display(Name = "غير مكتمل")] UnComplete = 3,
        [Display(Name = "محظور")] Block = 4,
    }

    public class Tasks
    {
        public int Id { get; set; }
        public string NameOfCustomer { get; set; }
        public int? CustomerId { get; set; }
        public string Problem { get; set; }
        public string UserId { get; set; }
        public State State { get; set; }
        public string Note { get; set; }
        public double Cost { get; set; }
        public bool IsPaid { get; set; }
        public int? MaintenanceTypeId { get; set; }



        public virtual Customers Customer { get; set; }
        public virtual User User { get; set; }
        public virtual MaintenanceType MaintenanceType { get; set; }

    }
}
