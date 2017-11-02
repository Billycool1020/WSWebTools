using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WSWebTool.Data;
using WSWebTool.Models;

namespace WSWebTool.Logic
{
    public class IIS
    {
        public static List<UnansweredThreads> GetIIS()
        {
            WSWebContext WSdb = new WSWebContext();            

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
            

          
            return list;
        }


        public static List<UnansweredThreads> GetIISAll()
        {
            WSWebContext WSdb = new WSWebContext();

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


            var list = threads.ToList();



            //foreach (var u in list)
            ////Parallel.ForEach(list, (u) =>
            //{
            //    var Note = WSdb.ThreadNotes.Find(u.ThreadId);
            //    if (Note != null)
            //    {
            //        u.Note = Note.Note;
            //    }
            //    else
            //    {
            //        u.Note = "";
            //    }
            //};


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
            list = list.Where(x => x.CreateTime >= month).ToList();
            return list;
        }
    }
}