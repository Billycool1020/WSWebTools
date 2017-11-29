using EPS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EPS.DAL
{
    public class EPSContext : DbContext
    {
        public EPSContext() : base("name=EPS")
        {
            Database.SetInitializer<EPSContext>(null);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lob> Lobs { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Product> Products { get; set; }
       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(c => c.Skills).WithMany(i => i.Employees)
                .Map(t => t.MapLeftKey("Employee")
                    .MapRightKey("Skill")
                    .ToTable("EmployeeSkill"));
        }
    }
}