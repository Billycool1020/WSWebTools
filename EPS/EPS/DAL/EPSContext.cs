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

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Lob> Lobs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(c => c.Skills).WithMany(i => i.Employees)
                .Map(t => t.MapLeftKey("MSAlias")
                .MapRightKey("SkillId")
                .ToTable("EmployeeSkill"));


        }


    }
}