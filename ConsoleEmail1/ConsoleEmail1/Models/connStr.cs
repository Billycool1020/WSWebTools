using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace ConsoleEmail1.Models
{
    class connStr:DbContext
    {
        public connStr() : base()
        {

        }
        public DbSet<MyThread> MyThreads { get; set; }
    }
}
