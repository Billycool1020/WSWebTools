using FollowUp.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUp
{
    class FollowContext :DbContext
    {
        public FollowContext() : base("name=WSWebTools")
        {
            Database.SetInitializer<FollowContext>(null);
        }
        public virtual DbSet<FollowUpThread> FollowUpThreads { get; set; }
        public virtual DbSet<EscalatedThread> EscalatedThreads { get; set; }
    }
}
