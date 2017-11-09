using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WSWebTool.Data;
using WSWebTool.Models;
using System.Threading.Tasks;
using WSWebTool.Logic;

namespace WSWebTool.Controllers
{
    public class UnansweredController : Controller
    {
        public ActionResult Detail(UnansweredThreads thread)
        {

            return View();
        }

        public ActionResult LastMouthFollow(string product, int? page)
        {

            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var Lastmonth = new DateTime(today.Year, today.Month-1, 1);
            List<UnansweredThreads> list = new List<UnansweredThreads>();
            WSWebContext WSdb = new WSWebContext();
            var StatusList = WSdb.ThreadStatus.ToList();
            ViewBag.Status = StatusList;
            List<string> productlist = WSdb.Products.Select(x => x.ProductName).OrderBy(x => x).ToList();
            ViewBag.productlist = productlist;
            ViewBag.product = product;
            if (product == "IIS")
            {
                ViewBag.Target = "N/A";
                list = IIS.GetIIS();
            }
            else if (product == "ASP.NET")
            {
                ViewBag.Target = "N/A";
                list = ASP.GetASP();
            }
            else
            {
                if (!String.IsNullOrEmpty(product))
                {
                    var Threads = (from a in WSdb.MSDNThreads
                                   where a.Product == product && a.PostDate< month && a.PostDate > Lastmonth
                                   select a).Count();
                    ViewBag.Threads = Threads;

                    var Marked = (from a in WSdb.MSDNThreads
                                  where a.Product == product && a.IsAnswered && a.PostDate < month && a.PostDate > Lastmonth
                                  select a).Count();
                    ViewBag.Marked = Marked;



                    var OPVAR = WSdb.Products.Where(x => x.ProductName == product).FirstOrDefault().OPVAR;
                    ViewBag.CProduct = product + "  OPVAR Target: " + OPVAR * 100 + "%";
                    ViewBag.Target = OPVAR;
                    ViewBag.Need = "Need:" + (int)(Threads * OPVAR - Marked + 1);



                    var unanswered = from a in WSdb.MSDNThreads
                                     where a.Product == product && a.IsLastOp && a.IsAnswered == false 
                                     select new UnansweredThreads
                                     {
                                         CreateTime = a.PostDate,
                                         Product = a.Product,
                                         ThreadId = a.ThreadId,
                                         url = a.Link,
                                         title = a.Title,
                                         IsLastOp = a.IsLastOp
                                     };

                    list = unanswered.ToList();

                    Parallel.ForEach(list, (u) =>
                    {
                        var date = (DateTime.Now - u.CreateTime);

                        u.Idle = (date.Days == 0 ? "" : date.Days + "d ") + date.Hours + "H";

                        using (WSWebContext WSdb2 = new WSWebContext())
                        {
                            var Note = WSdb2.ThreadNotes.Find(u.ThreadId);
                            if (Note != null)
                            {
                                u.Note = Note.Note;
                            }
                            else
                            {
                                u.Note = "0";
                            }
                        }
                    });
                   
                }
            }
            list = list.Where(x => x.IsLastOp && x.CreateTime> Lastmonth && x.CreateTime < month).OrderByDescending(x => x.CreateTime).ToList();

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
            
        }

        public ActionResult LastMouthUnanswered(string product, int? page)
        {

            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var Lastmonth = new DateTime(today.Year, today.Month - 1, 1);
            List<UnansweredThreads> list = new List<UnansweredThreads>();
            WSWebContext WSdb = new WSWebContext();
            var StatusList = WSdb.ThreadStatus.ToList();
            ViewBag.Status = StatusList;
            List<string> productlist = WSdb.Products.Select(x => x.ProductName).OrderBy(x => x).ToList();
            ViewBag.productlist = productlist;
            ViewBag.product = product;
            if (product == "IIS")
            {
                ViewBag.Target = "N/A";
                list = IIS.GetIIS();
            }
            else if (product == "ASP.NET")
            {
                ViewBag.Target = "N/A";
                list = ASP.GetASP();
            }
            else
            {
                if (!String.IsNullOrEmpty(product))
                {
                    var Threads = (from a in WSdb.MSDNThreads
                                   where a.Product == product && a.PostDate < month && a.PostDate > Lastmonth
                                   select a).Count();
                    ViewBag.Threads = Threads;

                    var Marked = (from a in WSdb.MSDNThreads
                                  where a.Product == product && a.IsAnswered && a.PostDate < month && a.PostDate > Lastmonth
                                  select a).Count();
                    ViewBag.Marked = Marked;



                    var OPVAR = WSdb.Products.Where(x => x.ProductName == product).FirstOrDefault().OPVAR;
                    ViewBag.CProduct = product + "  OPVAR Target: " + OPVAR * 100 + "%";
                    ViewBag.Target = OPVAR;
                    ViewBag.Need = "Need:" + (int)(Threads * OPVAR - Marked + 1);



                    var unanswered = from a in WSdb.MSDNThreads
                                     where a.Product == product && a.IsLastOp && a.IsAnswered == false
                                     select new UnansweredThreads
                                     {
                                         CreateTime = a.PostDate,
                                         Product = a.Product,
                                         ThreadId = a.ThreadId,
                                         url = a.Link,
                                         title = a.Title,
                                         IsLastOp = a.IsLastOp
                                     };

                    list = unanswered.ToList();

                    Parallel.ForEach(list, (u) =>
                    {
                        var date = (DateTime.Now - u.CreateTime);

                        u.Idle = (date.Days == 0 ? "" : date.Days + "d ") + date.Hours + "H";

                        using (WSWebContext WSdb2 = new WSWebContext())
                        {
                            var Note = WSdb2.ThreadNotes.Find(u.ThreadId);
                            if (Note != null)
                            {
                                u.Note = Note.Note;
                            }
                            else
                            {
                                u.Note = "0";
                            }
                        }
                    });

                }
            }
            list = list.Where(x => x.CreateTime > Lastmonth && x.CreateTime < month).OrderByDescending(x => x.CreateTime).ToList();

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Follow(string product, int? page)
        {
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            List<UnansweredThreads> list = new List<UnansweredThreads>();
            WSWebContext WSdb = new WSWebContext();
            var StatusList = WSdb.ThreadStatus.ToList();
            ViewBag.Status = StatusList;
            List<string> productlist = WSdb.Products.Select(x => x.ProductName).OrderBy(x => x).ToList();
            ViewBag.productlist = productlist;
            ViewBag.product = product;
            if (product == "IIS")
            {
                ViewBag.Target = "N/A";
                list = IIS.GetIIS();
            }
            else if (product == "ASP.NET")
            {
                ViewBag.Target = "N/A";
                list = ASP.GetASP();
            }
            else
            {
                if (!String.IsNullOrEmpty(product))
                {
                    var Threads = (from a in WSdb.MSDNThreads
                                   where a.Product == product && a.PostDate> month
                                   select a).Count();
                    ViewBag.Threads = Threads;

                    var Marked = (from a in WSdb.MSDNThreads
                                  where a.Product == product && a.IsAnswered && a.PostDate > month
                                  select a).Count();
                    ViewBag.Marked = Marked;



                    var OPVAR = WSdb.Products.Where(x => x.ProductName == product).FirstOrDefault().OPVAR;
                    ViewBag.CProduct = product + "  OPVAR Target: " + OPVAR * 100 + "%";
                    ViewBag.Target = OPVAR;
                    ViewBag.Need = "Need:" + (int)(Threads * OPVAR - Marked + 1);



                    var unanswered = from a in WSdb.MSDNThreads
                                     where a.Product == product && a.IsLastOp && a.IsAnswered == false
                                     select new UnansweredThreads
                                     {
                                         CreateTime = a.PostDate,
                                         Product = a.Product,
                                         ThreadId = a.ThreadId,
                                         url = a.Link,
                                         title = a.Title,
                                         IsLastOp = a.IsLastOp
                                     };

                    list = unanswered.ToList();

                    Parallel.ForEach(list, (u) =>
                    {
                        var date = (DateTime.Now - u.CreateTime);

                        u.Idle = (date.Days == 0 ? "" : date.Days + "d ") + date.Hours + "H";

                        using (WSWebContext WSdb2 = new WSWebContext())
                        {
                            var Note = WSdb2.ThreadNotes.Find(u.ThreadId);
                            if (Note != null)
                            {
                                u.Note = Note.Note;
                            }
                            else
                            {
                                u.Note = "0";
                            }
                        }
                    });
                    //list = list.Where(x => x.IsLastOp).OrderByDescending(x => x.CreateTime).ToList();
                }
            }
           
            list = list.Where(x => x.IsLastOp && x.CreateTime >= month).OrderByDescending(l => l.CreateTime).ToList();

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index(string product, int? page)
        {
            List<UnansweredThreads> list = new List<UnansweredThreads>();
            WSWebContext WSdb = new WSWebContext();
            var StatusList = WSdb.ThreadStatus.ToList();
            ViewBag.Status = StatusList;
            List<string> productlist = WSdb.Products.Select(x => x.ProductName).OrderBy(x => x).ToList();
            ViewBag.productlist = productlist;
            ViewBag.product = product;
            if (product == "IIS")
            {
                ViewBag.Target = "N/A";
                list = IIS.GetIIS();
            }
            else if (product == "ASP.NET")
            {
                ViewBag.Target = "N/A";
                list = ASP.GetASP();
            }
            else
            {
                if (!String.IsNullOrEmpty(product))
                {
                    var Threads = (from a in WSdb.MSDNThreads
                                   where a.Product == product
                                   select a).Count();
                    ViewBag.Threads = Threads;

                    var Marked = (from a in WSdb.MSDNThreads
                                  where a.Product == product && a.IsAnswered
                                  select a).Count();
                    ViewBag.Marked = Marked;



                    var OPVAR = WSdb.Products.Where(x => x.ProductName == product).FirstOrDefault().OPVAR;
                    ViewBag.CProduct = product + "  OPVAR Target: " + OPVAR * 100 + "%";
                    ViewBag.Target = OPVAR;
                    ViewBag.Need = "Need:" + (int)(Threads * OPVAR - Marked + 1);



                    var unanswered = from a in WSdb.MSDNThreads
                                     where a.Product == product && a.IsAnswered == false
                                     select new UnansweredThreads
                                     {
                                         CreateTime = a.PostDate,
                                         Product = a.Product,
                                         ThreadId = a.ThreadId,
                                         url = a.Link,
                                         title = a.Title,
                                         IsLastOp = a.IsLastOp
                                     };

                    list = unanswered.ToList();

                    Parallel.ForEach(list, (u) =>
                    {
                        var date = (DateTime.Now - u.CreateTime);

                        u.Idle = (date.Days == 0 ? "" : date.Days + "d ") + date.Hours + "H";

                        using (WSWebContext WSdb2 = new WSWebContext())
                        {
                            var Note = WSdb2.ThreadNotes.Find(u.ThreadId);
                            if (Note != null)
                            {
                                u.Note = Note.Note;
                            }
                            else
                            {
                                u.Note = "0";
                            }
                        }
                    });
                    //list = list.OrderByDescending(x => x.CreateTime).ToList();
                }
            }
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            list = list.Where(x => x.CreateTime >= month).OrderByDescending(x => x.CreateTime).ToList();

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


        public ActionResult AddNote(string Note, string Id,string product)
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
                    note.Product = product;
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