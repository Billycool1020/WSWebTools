using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumUserUpdateService
{
    class JobScheduler
    {
        private IScheduler _scheduler;
        //private string StrCron = "0 0 8 ? * MON-FRI"; //每周一到周五 8:00AM
        private string StrCron = "0 0 6 * * ? ";
        public JobScheduler()
        {
            IJobDetail job = JobBuilder.Create<ThreadsJob>().Build();

            #region 测试

            // create trigger (fire every 5 seconds)
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("MainTrigger", "MainGroup")
            //    .WithCronSchedule("0/5 * * * * ? *")
            //    .StartAt(DateTime.UtcNow)
            //    .WithPriority(1)
            //    .Build();
            #endregion

            ITrigger trigger = TriggerBuilder.Create().StartNow().WithCronSchedule(StrCron).Build();

            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
            _scheduler.ScheduleJob(job, trigger);
        }

        public void Start()
        {
            // start scheduler
            _scheduler.Start();
        }

        public void Stop()
        {
            // shutdown scheduler
            _scheduler.Shutdown();
        }
    }
}
