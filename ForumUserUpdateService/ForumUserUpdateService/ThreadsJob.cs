using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumUserUpdateService
{
    class ThreadsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NewUser.AddNewUser();
            }
            catch (Exception e)
            {
                Email.SendErrorMail(e, "New User");
            }
            try
            {
                NewActivity.AddNewActivity();
            }
            catch (Exception e)
            {
                Email.SendErrorMail(e, "New Activity");
            }
        }
    }
}
