using ASPIISUnanswered.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ASPIISUnanswered
{
    class Program
    {
        static void Main(string[] args)
        {

            //var list = Unanswered.UnansweredThreads();
            //WSWebToolEntities db = new WSWebToolEntities();
            //db.Database.ExecuteSqlCommand("TRUNCATE TABLE [ASPIISThreads]");
            //db.ASPIISThreads.AddRange(list);
            //db.SaveChanges();

            HostFactory.Run(x =>
            {
                x.Service<JobScheduler>(s =>
                {
                    s.ConstructUsing(name => new JobScheduler());
                    s.WhenStarted(ms => ms.Start());
                    s.WhenStopped(ms => ms.Stop());
                });

                x.SetServiceName("ASPIISUnanswered");
                x.SetDisplayName("ASPIISUnanswered");
                x.SetDescription("Get ASPIISUnanswered");
                x.RunAsLocalSystem();
                x.StartAutomatically();
            });


        }
    }
}
