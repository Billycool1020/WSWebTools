using FollowUpTestClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FollowUpTestClient
{
    class Program
    {
        static void Main(string[] args)
        {


            //FollowContext db = new FollowContext();
            // var list = db.FollowUpThreads.ToList();

            List<FollowUpThread> list = GetDBThreads.GetSHPList();
            list = GetDBThreads.GetOwner(list);

            list = Filter.RemoveEscalateThread(list);
            list = Filter.FliterDeleted(list);
            CRUD.ClearFollowData();
            list = list.GroupBy(x => x.ThreadId).Select(y => y.First()).ToList();
            var save = CRUD.SaveFollowData(list);
            var mail = Email.SendMailAsync(list);
            mail.Wait();
            save.Wait();





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
