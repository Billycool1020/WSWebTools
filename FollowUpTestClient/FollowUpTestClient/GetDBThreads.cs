using FollowUpTestClient.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FollowUpTestClient
{
    class Failedlist
    {
       public DataRow r { get; set; }
       public int Forumid { get; set; }
    }
    class GetDBThreads
    {
        static List<FollowUpThread> list = new List<FollowUpThread>();
        static List<Failedlist> Faillist = new List<Failedlist>();
        static double seconds;
        public static List<FollowUpThread> GetSHPList()
        {
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);
            seconds = ConvertToUnixTimestamp(first);
            FollowContext db = new FollowContext();
            var forums = db.Forums.ToList();

            //var forums = db.Forums.Where(x=>x.ProductId==54).ToList();
            //var forums = db.Forums.Where(x => x.Id == "805bdc3f-bf13-4784-9e81-548b5030302b").ToList();


            Parallel.ForEach(forums, forum => GetData(forum));
            Parallel.ForEach(Faillist, f => Check(f.r, f.Forumid));
            return list;
        }

        public static void GetData(Forum forum)
        {
            string command = "select distinct PostMessageId, CreatedTime,OwnerId, Title, URL from [dbo].[ForumMessages] M join dbo.ForumMessageTags T on M.Id = T.ForumMessageId  where [CreatedTime]>=@Start_Date and IsAnswered=0 and Tag=@tag";

            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["SHP"].ConnectionString;


            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(command))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Start_Date", seconds);
                        cmd.Parameters.AddWithValue("@tag", forum.Id);
                        //cmd.Parameters.AddWithValue("@tag", "02db4821-3b55-4f15-8844-8cedc8e51558");

                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
            }
            //foreach (DataRow r in dt.Rows)
            //{
            //    Check(r, forum.ProductId);
            //}

            Parallel.ForEach(dt.AsEnumerable(), r => Check(r, forum.ProductId));
            

        }

        public static void Check(DataRow r, int Forumid)
        {
            string constr = ConfigurationManager.ConnectionStrings["SHP"].ConnectionString;
            var PostID = r["PostMessageId"].ToString();
            string command2 = "select OwnerId,IsAnswered, CreatedTime,url,title from [dbo].[ForumMessages] M  where [CreatedTime]>=@Start_Date and PostMessageId=@PostId order by createdTime";
            bool IsAnswered = false;
            DataTable dt2 = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(command2))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Start_Date", seconds);
                            cmd.Parameters.AddWithValue("@PostId", PostID);
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dt2);
                            con.Close();
                            con.Dispose();
                        }
                    }
                }

                foreach(DataRow dr in dt2.Rows)
                {
                    if (Convert.ToBoolean(dr["IsAnswered"]))
                    {
                        IsAnswered = true;
                    }
                        
                }
                if (!IsAnswered)
                {
                    DataRow lastRow = dt2.Rows[dt2.Rows.Count - 1];
                    if (lastRow["OwnerId"].ToString() == r["OwnerId"].ToString())
                    {
                        var c = lastRow["OwnerId"].ToString();
                        var d = r["OwnerId"].ToString();
                        var a = r;
                        var b = dt2;
                        FollowUpThread thread = new FollowUpThread();

                        thread.ThreadId = PostID;
                        thread.cat_URL = r["URL"].ToString();
                        thread.LastOP = ConvertFromUnixTimestamp(Convert.ToDouble(lastRow["CreatedTime"].ToString()));
                        thread.ThreadName = r["Title"].ToString();
                        thread.ProductId = Forumid;
                        list.Add(thread);
                    }
                }
                
            }
            catch
            {
                Failedlist failedlist = new Failedlist();
                failedlist.r = r;
                failedlist.Forumid = Forumid;
                Faillist.Add(failedlist);
            }
           

           
        }


        public static List<FollowUpThread> GetOwner(List<FollowUpThread> list)
        {
            List<FollowUpThread> Cosmos = GetCosmosDBList();
            foreach (var f in list)
            {
                var t = Cosmos.Where(x => x.ThreadId == f.ThreadId).FirstOrDefault();
                if (t != null)
                {
                    f.cat_msalias = t.cat_msalias;
                }
            }
            return list;
        }



        public static List<FollowUpThread> GetCosmosDBList()
        {
            string command =
                "select distinct ForumId,ThreadId,ThreadName,CreatedDateTime,CreatedByCustomerid,HasAnswer,HasReply,tl.cat_msalias,tl.cat_ismanagedthread,tl.cat_EntitlementStatus,tl.cat_URL" +
                " into #threads from Thread t " +
                "join cosmosdb..catTotalThread tl on t.ThreadId = tl.cat_externalid where datasource = 'STO' " +
                "and IsDeleted = 0 and localecode in ('en-US', 'zh-CN', 'zh-TW') " +
                "and CreatedDateTime>= @Start_Date " +
                "and CreatedDateTime<Dateadd(day,1, @End_Date) " +
                " and ThreadType = 'Question' " +
                "select distinct t.ThreadId,t.CreatedDateTime as ThreadCreateTime,t.CreatedByCustomerid as AuthorId,p.CreatedByCustomerid as PosterID,p.isMarked,p.isAnswer,p.CreatedDateTime " +
                "into #post from #threads t " +
                "join Post p on t.ThreadId = p.ThreadId " +
                "where p.CreatedDateTime >= @Start_Date " +
                " and p.IsDeleted = 0 " +
                "order by t.ThreadId,p.CreatedDateTime " +
                "select ThreadId,max(CreatedDateTime) as LastOP " +
                " into #a " +
                " from #post " +
                " where ThreadId not in (select distinct ThreadId from #threads where hasanswer=1) " +
                " group by ThreadId " +
                "select distinct a.ThreadId,a.LastOP into #LastOP from #a a" +
                " join #post p on p.ThreadId=a.ThreadId and a.lastop=p.CreatedDateTime and authorid=PosterID " +
                "union " +
                "select distinct threadid, CreatedDateTime from #threads" +
                " where hasanswer=0 and threadid not in (select threadid from #a)" +
                "select distinct lo.ThreadId,1 as CSSReplied into #CSSReplied from #LastOP lo " +
                "join #post p on lo.ThreadId=p.ThreadId " +
                "join ForumSupportNotificationSystem..userforumnamemapping uf on p.PosterID = uf.forumname and len(uf.forumname)>= 36 " +
                "join ForumSupportNotificationSystem..engineer e on e.userid = uf.userid and e.productid not in (100, 1000, 1001) and isnull(e.quitdate,'2099-12-31')>= @Start_Date " +
                "select distinct t.ForumId,t.ThreadId,t.cat_msalias,t.ThreadName,case when cr.CSSReplied = 1 then 'Yes' else 'No' end as HasReply,t.cat_URL,t.HasAnswer,t.cat_ismanagedthread,t.cat_EntitlementStatus,lo.LastOP " +
                "from #threads t " +
                "join #LastOP lo on t.ThreadId=lo.ThreadId " +
                "left join #CSSReplied cr on cr.ThreadId=lo.ThreadId " +
                "order by t.ForumId,t.cat_msalias " +
                "drop table #threads,#post,#a,#lastop,#CSSReplied ";
            List<FollowUpThread> list = new List<FollowUpThread>();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["cosmosdb"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(command))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Start_Date", DateTime.Now.AddDays(-60).ToShortDateString());
                        cmd.Parameters.AddWithValue("@End_Date", DateTime.Now.ToShortDateString());
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
            }
            FollowUpThread followUpThreads;
            foreach (DataRow row in dt.Rows)
            {
                if (DateTime.Now.Month - Convert.ToDateTime(row["LastOP"].ToString()).Month > 1)
                {
                    continue;
                }
                followUpThreads = new FollowUpThread();
                followUpThreads.ThreadName = row["ThreadName"].ToString();
                followUpThreads.cat_URL = row["cat_URL"].ToString();
                followUpThreads.cat_msalias = row["cat_msalias"].ToString();
                followUpThreads.ThreadId = row["ThreadId"].ToString();
                followUpThreads.LastOP = Convert.ToDateTime(row["LastOP"].ToString());
                list.Add(followUpThreads);
            }
            return list;
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }        
    }
}
