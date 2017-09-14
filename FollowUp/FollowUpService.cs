using FollowUp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FollowUp
{
    public partial class FollowUpService : ServiceBase
    {
        public FollowUpService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }


        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {

            List<FollowUpThread> list = Filter.FollowThreads(AllThreads.GetList());
            list = Filter.RemoveEscalateThread(list);
            CRUD.ClearFollowData();
            var save = CRUD.SaveFollowData(list);
            var mail = Email.SendMailAsync(list);
            mail.Wait();
            save.Wait();


        }
        protected override void OnStop()
        {
        }
    }
}
