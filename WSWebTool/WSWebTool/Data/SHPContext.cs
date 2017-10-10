using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WSWebTool.Models;

namespace WSWebTool.Data
{
    public class SHPContext : DbContext
    {
        public SHPContext() : base("name=SHP")
        {
            Database.SetInitializer<SHPContext>(null);
        }
        public virtual DbSet<ForumMessage> ForumMessages { get; set; }
        public virtual DbSet<ForumMessageKeyword> ForumMessageKeywords { get; set; }
        public virtual DbSet<ForumMessageLexi> ForumMessageLexis { get; set; }
        public virtual DbSet<ForumMessageTag> ForumMessageTags { get; set; }
        public virtual DbSet<ForumUser> ForumUsers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          


        }
    }
}