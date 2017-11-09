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
        public virtual DbSet<MSDNForum> MSDNForums { get; set; }
        public virtual DbSet<ThreadNote> ThreadNotes { get; set; }
        public virtual DbSet<ASPIISForum> ASPIISForums { get; set; }
        public virtual DbSet<ASPIISThread> ASPIISThreads { get; set; }
        public virtual DbSet<MSDNThread> MSDNThreads { get; set; }
        public virtual DbSet<ThreadStatus> ThreadStatus { get; set; }
        public virtual DbSet<ForumMember> ForumMembers { get; set; }
        public virtual DbSet<ForumMemberActivity> ForumMemberActuvities { get; set; }
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