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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {



        }


    }
}