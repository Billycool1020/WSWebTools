using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WSWebTool.Data;
using WSWebTool.DTO;

namespace WSWebTool.Controllers
{
    public class UserActivityController : Controller
    {
        // GET: UserActivity
        public ActionResult Index()
        {
            int year = DateTime.Today.Year;
            List<ActivityReport> list = new List<ActivityReport>();
            WSWebContext db = new WSWebContext();
            for (var i = 1; i <= 12; i++)
            {
                DateTime dt = new DateTime(year, i, 1);
                ActivityReport activityReport = new ActivityReport();
                activityReport.Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dt.Month);
                activityReport.MemberCount = db.ForumMembers.Where(x => x.MemberSince.Month ==i && x.MemberSince.Year==year).Count();
                activityReport.MSDN= db.ForumMemberActuvities.Where(x => x.Time.Month ==i && x.Time.Year == year && x.Product=="MSDN").Count();
                activityReport.TN = db.ForumMemberActuvities.Where(x => x.Time.Month == i && x.Time.Year == year && x.Product == "TN").Count();
                list.Add(activityReport);
            }
          

            return View(list);
        }

        public ActionResult DailyReport()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int days = System.DateTime.DaysInMonth(year, month);
            List<DailyReport> list = new List<DailyReport>();
            WSWebContext db = new WSWebContext();
            var activity = db.ForumMemberActuvities.Where(x=>x.Time.Year == year&&x.Time.Month==month).ToList();

            for(var i=1;i<=days;i++)
           {
              
               DateTime dt = new DateTime(year, month, i);
               DailyReport report = new DailyReport();
               report.date = dt;
               var msdn = activity.Where(x => x.Product == "MSDN" && x.Time.Day == i);
               report.MAsk = msdn.Where(x => x.Activity.Contains("Asked")).Count();
               report.MMark = msdn.Where(x => x.Activity.Contains("Marked")).Count();
               report.MVote = msdn.Where(x => x.Activity.Contains("Voted")).Count();
               report.MReply = msdn.Where(x => x.Activity.Contains("Replied") && !x.Activity.Contains("discussion")).Count();
               report.MTotal = report.MAsk + report.MMark + report.MVote + report.MReply;

               var tn = activity.Where(x => x.Product == "TN" && x.Time.Day == i);
               report.TAsk = tn.Where(x => x.Activity.Contains("Asked")).Count();
               report.TMark = tn.Where(x => x.Activity.Contains("Marked")).Count();
               report.TVote = tn.Where(x => x.Activity.Contains("Voted")).Count();
               report.TReply = tn.Where(x => x.Activity.Contains("Replied") && !x.Activity.Contains("discussion")).Count();
               report.TTotal = report.TAsk + report.TMark + report.TVote + report.TReply;
               report.Total = report.MTotal + report.TTotal;
               if (report.Total != 0)
               {
                   list.Add(report);
               }
           }

            list.OrderBy(x => x.date);


            return View(list);
        }
    }
}