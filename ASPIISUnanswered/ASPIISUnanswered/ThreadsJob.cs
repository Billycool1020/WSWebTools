using ASPIISUnanswered.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPIISUnanswered
{
    class ThreadsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("do main task at: " + DateTime.Now);
            try
            {
                var list = Unanswered.UnansweredThreads();
                var list2 = MSDNUnanswered.MSDNUnansweredlist();
                WSWebToolEntities db = new WSWebToolEntities();
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [MSDNThreads]");
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [ASPIISThreads]");
                db.ASPIISThreads.AddRange(list);
                db.MSDNThreads.AddRange(list2);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException + "<br />" + e.Message);
            }
        }
    }
}
