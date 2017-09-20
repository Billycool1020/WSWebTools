using FollowUpTestClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FollowUpTestClient
{
    class Program
    {
        static void Main(string[] args)
        {

            //HostFactory.Run(x =>
            //{
            //    x.Service<JobScheduler>(s =>
            //    {
            //        s.ConstructUsing(name => new JobScheduler());
            //        s.WhenStarted(ms => ms.Start());
            //        s.WhenStopped(ms => ms.Stop());
            //    });

            //    x.SetServiceName("FollowUpService");
            //    x.SetDisplayName("FollowUpService");
            //    x.SetDescription("Update No Follow Up threads");
            //    x.RunAsLocalSystem();
            //    x.StartAutomatically();
            //});
        }
    }
}
