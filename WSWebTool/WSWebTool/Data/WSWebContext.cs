using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WSWebTool.Models;

namespace WSWebTool.Data
{
    public class WSWebContext : DbContext
    {
        public WSWebContext() : base("name=WSWebTools")
        {
            Database.SetInitializer<WSWebContext>(null);
        }
        public virtual DbSet<FollowUpThread> FollowUpThreads { get; set; }
        public virtual DbSet<EscalatedThread> EscalatedThreads { get; set; }
        public virtual DbSet<Engineer> Engineers { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Forum> Forums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                  .HasMany(e => e.Engineers)
                  .WithRequired(e => e.Team)
                  .HasForeignKey(e => e.SubTeamId)
                  .WillCascadeOnDelete(false);


        }

    }
}