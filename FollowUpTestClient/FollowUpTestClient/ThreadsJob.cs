using FollowUpTestClient.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient
{
    public class ThreadsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            // do main task here
            //TODO:
            Console.WriteLine("do main task at: " + DateTime.Now);
            try
            {
                List<FollowUpThread> list = Filter.FollowThreads(AllThreads.GetList());
                list = Filter.RemoveEscalateThread(list);
                CRUD.ClearFollowData();
                list = list.GroupBy(x => x.ThreadId).Select(y => y.First()).ToList();
                var save = CRUD.SaveFollowData(list);
                var mail = Email.SendMailAsync(list);
                mail.Wait();
                save.Wait();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException+"<br />"+e.Message);
            }
          
        }
    }
}
