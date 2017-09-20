using FollowUpTestClient.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient
{
    class FollowContext : DbContext
    {
        public FollowContext() : base("name=WSWebTools")
        {
            Database.SetInitializer<FollowContext>(null);
        }
        public virtual DbSet<FollowUpThread> FollowUpThreads { get; set; }
        public virtual DbSet<EscalatedThread> EscalatedThreads { get; set; }
        public virtual DbSet<Engineer> Engineers { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Product> Products { get; set; }

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
