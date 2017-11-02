using ASPIISUnanswered.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ASPIISUnanswered
{
    public class temp
    {
        public string ThreadId { get; set; }
        public long PostDate { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public bool IsLastOp { get; set; }
        public bool IsAnswered { get; set; }
        public string Product { get; set; }
    }
    class MSDNUnanswered
    {
        public static List<MSDNThread> MSDNUnansweredlist()
        {
            List<MSDNThread> list = new List<MSDNThread>();
            List<MSDNThread> list2 = new List<MSDNThread>();
            List<MSDNThread> errorlist = new List<MSDNThread>();
            List<MSDNThread> deletelist = new List<MSDNThread>();
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month-1, 1);

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = month.ToUniversalTime() - origin;
            var seconds = Math.Floor(diff.TotalSeconds);



            WSWebToolEntities WSdb = new WSWebToolEntities();

            List<string> forumid;
            forumid = WSdb.MSDNForums.Select(x => x.Id).ToList();

            var forum = WSdb.MSDNForums.Include("Product").ToList();

            MSDNEntities db = new MSDNEntities();


            var Threads = (from M in db.ForumMessages
                           join t in db.ForumMessageTags
                           on M.Id equals t.ForumMessageId
                           where M.CreatedTimeOfDaily >= seconds && forumid.Contains(t.Tag)
                           select new temp
                           {
                               ThreadId = M.PostMessageId,
                               IsAnswered = M.IsAnswered,
                               Link = M.URL,
                               PostDate = M.CreatedTime,
                               Title = M.Title,
                               Product = t.Tag,
                           });
            Console.WriteLine("Threads : "+Threads.Count());
            Parallel.ForEach(Threads, (t) =>
            {
                MSDNThread msdn = new MSDNThread();
                msdn.ThreadId = t.ThreadId;
                msdn.IsAnswered = t.IsAnswered;
                msdn.Link = t.Link;
                msdn.PostDate = ConvertFromUnixTimestamp(t.PostDate);
                msdn.Title = t.Title;
                msdn.Product = t.Product;
                list.Add(msdn);
            });

            Console.WriteLine("List : "+list.Count);

            Console.WriteLine("Check answered Start");
            Parallel.ForEach(list, (l) =>
            {
                l.Product = forum.Where(x => x.Id.ToLower().Trim() == l.Product.ToLower().Trim()).FirstOrDefault().Product.ProductName;
                if (!l.IsAnswered)
                {
                    using (MSDNEntities db2 = new MSDNEntities())
                    {
                        l.IsAnswered = db2.ForumMessages.Where(x => x.PostMessageId == l.ThreadId).Any(c => c.IsAnswered == true);                       
                    }
                }
            });
            Console.WriteLine("Check answered Finsih");
            Console.WriteLine("Check Deleted Start");
            
            Parallel.ForEach(list, (t) =>
             {
                 if (t.IsAnswered)
                 {
                     list2.Add(t);
                 }
                 else
                 {
                     try
                     {
                         HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(t.Link);
                         // Sends the HttpWebRequest and waits for a response.
                         HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                         myHttpWebResponse.Close();
                         
                         list2.Add(t);
                         Console.WriteLine(t.ThreadId);
                        
                     }
                     catch(Exception e)
                     {
                         if (!e.Message.Contains("410"))
                         {
                             errorlist.Add(t);
                         }
                         else
                         {
                             deletelist.Add(t);
                         }
                         
                     }
                 }                 
             });

            Console.WriteLine("Check Deleted Finish");


            Console.WriteLine("Check Deleted 2 Start");
            Parallel.ForEach(errorlist, (t) => {
                try
                {
                    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(t.Link);
                    // Sends the HttpWebRequest and waits for a response.
                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    myHttpWebResponse.Close();
                    list2.Add(t);
                    Console.WriteLine(t.ThreadId);

                }
                catch
                {
                    deletelist.Add(t);
                }

            });

            Console.WriteLine("Check Deleted 2 Finish");

            Console.WriteLine("Check LastOp Start");
            Parallel.ForEach(list2, (t) =>
            {
                using (MSDNEntities db2 = new MSDNEntities())
                {
                    var owner = db2.ForumMessages.Where(x => x.PostMessageId == t.ThreadId).OrderBy(x => x.CreatedTime).Select(x => x.OwnerId).ToList();
                    var islastop = owner[0] == owner[owner.Count()-1];
                    t.IsLastOp = islastop;
                }

            });

            Console.WriteLine("Check LastOp Finish");
            return list2;

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
