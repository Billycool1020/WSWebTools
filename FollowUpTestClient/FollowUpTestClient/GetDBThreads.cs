using FollowUpTestClient.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient
{
    class GetDBThreads
    {
        public static  List<FollowUpThread> GetDBList()
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
    }
}
