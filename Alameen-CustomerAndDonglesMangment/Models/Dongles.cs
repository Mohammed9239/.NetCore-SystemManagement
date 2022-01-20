using System;
using System.Collections.Generic;

namespace Alameen_CustomerAndDonglesMangment.Models
{
    public partial class Dongles
    {
        public int Id { get; set; }
        public string SerialNum { get; set; }
        public int? MainOrSub { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? DongleTypeId { get; set; }
        public int? DongleColorId { get; set; }
        public int? AmeenVerId { get; set; }
        public int? AmeenAddOnTypesId { get; set; }
        public int? CustomerId { get; set; }
        public int? CustBranId { get; set; }
        public int? RepresentativesId { get; set; }
        public int? BranchId { get; set; }
        public string VerNum { get; set; }

        public virtual AmeenAddOnTypes AmeenAddOnTypes { get; set; }
        public virtual AmeenVers AmeenVer { get; set; }
        public virtual Branches Branch { get; set; }
        public virtual CustBrans CustBran { get; set; }
        public virtual Customers Customer { get; set; }
        public virtual DongleColors DongleColor { get; set; }
        public virtual DongleTypes DongleType { get; set; }
        public virtual Representatives Representatives { get; set; }
    }
}
