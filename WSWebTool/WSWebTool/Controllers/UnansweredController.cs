using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WSWebTool.Data;
using WSWebTool.Models;
using System.Threading.Tasks;

namespace WSWebTool.Controllers
{
    public class UnansweredController : Controller
    {
        public ActionResult Detail(UnansweredThreads thread)
        {

            return View();
        }

        public ActionResult IIS(string product, int? page)
        {
            //UnansweredThreads
            WSWebContext WSdb = new WSWebContext();


            List<string> productlist = WSdb.ASPIISForums.Where(x => x.Product.ProductName == "IIS").OrderBy(x=>x.ForumName).Select(x => x.ForumName).ToList();
            ViewBag.productlist = productlist;
            ViewBag.product = product;



            var threads = from t in WSdb.ASPIISThreads
                          where t.ASPIISForum.Product.ProductName == "IIS"
                          orderby t.PostDate descending
                          select new UnansweredThreads
                          {
                              ThreadId = t.ThreadId,
                              title = t.Title,
                              url = t.Link,
                              CreateTime = t.PostDate,
                              Product = t.ASPIISForum.ForumName,
                              IsLastOp = t.IsLastOp
                          };

            if (!String.IsNullOrEmpty(product))
            {
                threads = threads.Where(x => x.Product == product);

            }

            var list = threads.ToList();



            foreach (var u in list)
            //Parallel.ForEach(list, (u) =>
            {
                var Note = WSdb.ThreadNotes.Find(u.ThreadId);
                if (Note != null)
                {
                    u.Note = Note.Note;
                }
                else
                {
                    u.Note = "";
                }
            };

            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            list = list.Where(x => x.IsLastOp &&x.CreateTime>= month).ToList();

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Asp(string product, int? page)
        {
            //UnansweredThreads
            WSWebContext WSdb = new WSWebContext();


            List<string> productlist = WSdb.ASPIISForums.Where(x => x.Product.ProductName == "ASP.NET").Select(x => x.ForumName).OrderBy(x => x).ToList();
            ViewBag.productlist = productlist;
            ViewBag.product = product;





            var threads = from t in WSdb.ASPIISThreads
                          where t.ASPIISForum.Product.ProductName == "ASP.NET"
                          orderby t.PostDate descending
                          where t.IsLastOp==true
                          select new UnansweredThreads
                          {
                              ThreadId = t.ThreadId,
                              title = t.Title,
                              url = t.Link,
                              CreateTime = t.PostDate,
                              Product = t.ASPIISForum.ForumName,
                              IsLastOp = t.IsLastOp
                          };

            if (!String.IsNullOrEmpty(product))
            {
                threads = threads.Where(x => x.Product == product);

            }

            var list = threads.ToList();



            //foreach (var u in list)
            Parallel.ForEach(list, (u) =>
            {
                var date = (DateTime.Now - u.CreateTime);

                u.Idle = (date.Days==0?"": date.Days + "d ") + date.Hours + "H";
                
                using (WSWebContext WSdb2 = new WSWebContext())
                {
                    var Note = WSdb2.ThreadNotes.Find(u.ThreadId);
                    if (Note != null)
                    {
                        u.Note = Note.Note;
                    }
                    else
                    {
                        u.Note = "";
                    }
                }
               
            });
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            list = list.Where(x => x.IsLastOp&&x.CreateTime>= month).OrderByDescending(l => l.CreateTime).ToList();
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        // GET: Unanswered
        public ActionResult Index(string product, int? page)
        {

            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var seconds = ConvertToUnixTimestamp(month);
            WSWebContext WSdb = new WSWebContext();
            List<string> productlist = WSdb.Products.Select(x => x.ProductName).OrderBy(x=>x).ToList();
            ViewBag.productlist = productlist;
            ViewBag.product = product;

            List<string> forumid;
            if (!String.IsNullOrEmpty(product))
            {

                forumid = WSdb.Forums.Where(x => x.Product.ProductName == product).Select(x => x.Id).ToList();
            }
            else
            {
                ViewBag.Target = "N/A";
                forumid = WSdb.Forums.Select(x => x.Id).ToList();
            }
            var forum = WSdb.Forums.Include("Product").ToList();

            SHPEntities db = new SHPEntities();


            var Threads = (from M in db.ForumMessages
                           join t in db.ForumMessageTags
                           on M.Id equals t.ForumMessageId
                           where M.CreatedTimeOfDaily >= seconds && forumid.Contains(t.Tag)
                           select M).Count();

            ViewBag.Threads = Threads;

           
            var Marked = (from M in db.ForumMessages
                          join t in db.ForumMessageTags
                          on M.Id equals t.ForumMessageId
                          where M.IsAnswered == true && M.CreatedTimeOfDaily >= seconds && forumid.Contains(t.Tag)
                          select M.PostMessageId).ToList().Select(x => x.First()).Count();
            ViewBag.Marked = Marked;


            if (!String.IsNullOrEmpty(product))
            {
                var OPVAR = WSdb.Products.Where(x => x.ProductName == product).FirstOrDefault().OPVAR;
                ViewBag.CProduct = product + "  OPVAR Target: " + OPVAR * 100 + "%";
                ViewBag.Target = OPVAR;
                ViewBag.Need = "Need:" + (int)(Threads * OPVAR - Marked + 1);
            }



            var unanswered = from M in db.ForumMessages
                             join t in db.ForumMessageTags
                             on M.Id equals t.ForumMessageId
                             where M.IsAnswered == false && M.CreatedTimeOfDaily >= seconds && forumid.Contains(t.Tag)
                             orderby M.CreatedTime descending
                             select new UnansweredThreads
                             {
                                 ThreadId = M.PostMessageId,
                                 title = M.Title,
                                 url = M.URL,
                                 Temptime = M.CreatedTime,
                                 Forum = t.Tag
                             };
            var list = unanswered.ToList();

            //foreach (var u in list)
            Parallel.ForEach(list, (u) =>
            {
                using (SHPEntities db2 = new SHPEntities())
                using (WSWebContext WSdb2 = new WSWebContext())
                {
                    var Owner = (from M in db2.ForumMessages
                                 where M.PostMessageId == u.ThreadId
                                 orderby M.CreatedTime
                                 select M.OwnerId).ToList();
                    u.IsLastOp = (Owner[0] == Owner[Owner.Count - 1]);                    
                    u.Product = forum.Where(x => x.Id.ToLower().Trim() == u.Forum.ToLower().Trim()).FirstOrDefault().Product.ProductName;
                    u.CreateTime = ConvertFromUnixTimestamp(u.Temptime);
                    var Note = WSdb2.ThreadNotes.Find(u.ThreadId);
                    if (Note != null)
                    {
                        u.Note = Note.Note;
                    }
                    else
                    {
                        u.Note = "";
                    }
                }                 
            });

            list = list.Where(x=>x.IsLastOp).OrderByDescending(x => x.CreateTime).ToList();
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


        public ActionResult AddNote(string Note, string Id)
        {
            try
            {
                WSWebContext db = new WSWebContext();
                ThreadNote note = db.ThreadNotes.Find(Id);
                if (note != null)
                {
                    note.Note = Note;
                    db.SaveChanges();
                }
                else
                {
                    note = new ThreadNote();
                    note.ThreadID = Id;
                    note.Note = Note;
                    db.ThreadNotes.Add(note);
                    db.SaveChanges();
                }


                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


        }
    }
}