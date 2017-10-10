using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WSWebTool.Data;
using WSWebTool.Models;

namespace WSWebTool.Controllers
{
    public class UnansweredController : Controller
    {
        // GET: Unanswered
        public ActionResult Index(string product, int? page)
        {
            
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);
            var seconds = ConvertToUnixTimestamp(month);
            WSWebContext WSdb = new WSWebContext();
            List<string> productlist = WSdb.Products.Select(x => x.ProductName).ToList();
            ViewBag.productlist = productlist;
            ViewBag.product = product;
            List<string> forumid;
            if (!String.IsNullOrEmpty(product))
            {

                forumid = WSdb.Forums.Where(x => x.Product.ProductName == product).Select(x => x.Id).ToList();
            }
            else
            {
                forumid = WSdb.Forums.Select(x => x.Id).ToList();
            }

            var forum = WSdb.Forums.ToList();


            SHPEntities db = new SHPEntities();
            var unanswered = from M in db.ForumMessages
                             join t in db.ForumMessageTags
                             on M.Id equals t.ForumMessageId
                             where M.IsAnswered == false && M.CreatedTimeOfDaily >= seconds && forumid.Contains(t.Tag)
                             orderby M.CreatedTime descending
                             select new UnansweredThreads
                             {
                                 title = M.Title,
                                 url = M.URL,
                                 Temptime = M.CreatedTime,
                                 Forum = t.Tag
                             };
            var list = unanswered.ToList();
            foreach (var u in list)
            {
                u.Product = forum.Where(x => x.Id.ToLower() == u.Forum.ToLower()).FirstOrDefault().Product.ProductName;
                u.CreateTime = ConvertFromUnixTimestamp(u.Temptime);
            }
            list.OrderByDescending(x => x.CreateTime);
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        public static DateTime ConvertFromUnixTimestamp(long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }
    }
}