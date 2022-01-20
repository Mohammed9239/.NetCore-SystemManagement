using System;
using System.Collections.Generic;
using System.Text;
using Alameen_CustomerAndDonglesMangment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alameen_CustomerAndDonglesMangment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityTypes> ActivityTypes { get; set; }
        public virtual DbSet<AmeenAddOnTypes> AmeenAddOnTypes { get; set; }
        public virtual DbSet<AmeenVers> AmeenVers { get; set; }
        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<ContractTypes> ContractTypes { get; set; }
        public virtual DbSet<CustBrans> CustBrans { get; set; }
        public virtual DbSet<CustomerStatuses> CustomerStatuses { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<DongleColors> DongleColors { get; set; }
        public virtual DbSet<DongleTypes> DongleTypes { get; set; }
        public virtual DbSet<Dongles> Dongles { get; set; }
        public virtual DbSet<Representatives> Representatives { get; set; }
        public virtual DbSet<Sqlvers> Sqlvers { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<WindowsVers> WindowsVers { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<MaintenanceType> MaintenanceTypes { get; set; }

    }
}
