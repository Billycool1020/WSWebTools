using ForumUserUpdateService.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ForumUserUpdateService
{
    class Program
    {
        static void Main(string[] args)
        {
           

            HostFactory.Run(x =>
            {
                x.Service<JobScheduler>(s =>
                {
                    s.ConstructUsing(name => new JobScheduler());
                    s.WhenStarted(ms => ms.Start());
                    s.WhenStopped(ms => ms.Stop());
                });

                x.SetServiceName("ForumUserUpdateService");
                x.SetDisplayName("ForumUserUpdateService");
                x.SetDescription("Update User and Avtivity");
                x.RunAsLocalSystem();
                x.StartAutomatically();
            });


        }
    }
}
