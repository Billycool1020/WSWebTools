using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WSWebTool.Data;
using WSWebTool.Models;

namespace WSWebTool.Logic
{
    public class ASP
    {
        public static List<UnansweredThreads> GetASP()
        {
            WSWebContext WSdb = new WSWebContext();


            var threads = from t in WSdb.ASPIISThreads
                          where t.ASPIISForum.Product.ProductName == "ASP.NET"
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

            var list = threads.ToList();

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
            //var today = DateTime.Today;
            //var month = new DateTime(today.Year, today.Month, 1);
            //list = list.Where(x => x.IsLastOp && x.CreateTime >= month).OrderByDescending(l => l.CreateTime).ToList();
            return list;
        }

        public static List<UnansweredThreads> GetASPAll()
        {
            WSWebContext WSdb = new WSWebContext();


            var threads = from t in WSdb.ASPIISThreads
                          where t.ASPIISForum.Product.ProductName == "ASP.NET"
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

            var list = threads.ToList();

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
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            list = list.Where(x=>x.CreateTime >= month).OrderByDescending(l => l.CreateTime).ToList();
            return list;
        }
    }
}