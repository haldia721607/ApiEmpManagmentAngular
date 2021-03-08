using EmpManagmentWebApiBOL.Tables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmpManagmentWebApiDAL.DbContextClass
{
    public class EmployeeDbContext : IdentityDbContext<ApplicationUser>
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public DbSet<Complaients> Complaients { get; set; }
        public DbSet<ComplaientDetails> ComplaientDetails { get; set; }
        public DbSet<ComplaientCategory> ComplaientCategories { get; set; }
        public DbSet<ComplaientPermamentAddress> ComplaientPermamentAddresses { get; set; }
        public DbSet<ComplaientTempAddress> ComplaientTempAddresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<BikeCollection> BikeCollections { get; set; }
        public DbSet<BikeCategory> BikeCategories { get; set; }
        public DbSet<Bulk> Bulk { get; set; }
        public DbSet<BulkDatas> BulkDatas { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
