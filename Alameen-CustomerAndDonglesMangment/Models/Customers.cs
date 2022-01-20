using System;
using System.Collections.Generic;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public partial class Customers
    {
        public Customers()
        {
            CustBrans = new HashSet<CustBrans>();
            Dongles = new HashSet<Dongles>();
            Representatives = new HashSet<Representatives>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CommercialName { get; set; }
        public double? ContractPrice { get; set; }
        public DateTime? ContractDate { get; set; }
        public string Logo { get; set; }
        public int? BranchId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? ActivityTypeId { get; set; }
        public int? ContractTypeId { get; set; }
        public int? CustomerStatusId { get; set; }
        public int? WindowsVerId { get; set; }
        public int? SqlverId { get; set; }
        public string Note { get; set; }

        public virtual ActivityTypes ActivityType { get; set; }
        public virtual Branches Branch { get; set; }
        public virtual Cities City { get; set; }
        public virtual ContractTypes ContractType { get; set; }
        public virtual CustomerStatuses CustomerStatus { get; set; }
        public virtual Sqlvers Sqlver { get; set; }
        public virtual States State { get; set; }
        public virtual WindowsVers WindowsVer { get; set; }
        public virtual ICollection<CustBrans> CustBrans { get; set; }
        public virtual ICollection<Dongles> Dongles { get; set; }
        public virtual ICollection<Representatives> Representatives { get; set; }
    }
}
