using FollowUpTestClient.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FollowUpTestClient
{
    class Filter
    {
        static List<FollowUpThread> followList = new List<FollowUpThread>();
        static List<FollowUpThread> noneedFollowList = new List<FollowUpThread>();
        static List<FollowUpThread> errorFollowList = new List<FollowUpThread>();
        

        public static List<FollowUpThread> FliterDeleted(List<FollowUpThread> list)
        {
            followList = new List<FollowUpThread>();
            Parallel.ForEach(list, Thread => CheckDel(Thread));
    
           
            return followList;
        }


        public static void CheckDel(FollowUpThread oldThread)
        {
             
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(oldThread.cat_URL);
                // Sends the HttpWebRequest and waits for a response.
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                myHttpWebResponse.Close();
                followList.Add(oldThread);

            }
            catch
            {
                
            }           
           
        }


        public static List<FollowUpThread> FollowThreads(List<FollowUpThread> threads)
        {
            followList.Clear();
            noneedFollowList.Clear();
            errorFollowList.Clear();

            List<string> ownerList = threads.Select(p => p.cat_msalias).Distinct().ToList();
            if (ownerList[0]!=null)
            {
                FollowContext db = new FollowContext();
                var engineers = db.Engineers.Select(x => x.MSAlias).ToList();
                threads = threads.Where(x => engineers.Contains(x.cat_msalias)).ToList();
            }            

            #region 遍历threads集合 比较OP name 和 last poster name

            Parallel.ForEach(threads, oldThread => Check(oldThread));            

            #endregion
          
            if (errorFollowList.Count > 0)
            {
               // var result = Email.SendErrorMailAsync(errorFollowList);
               // result.Wait();
            }

            return followList;
        }
        public static void Check(FollowUpThread oldThread)
        {
            try
            {
                var html = new HtmlDocument();
                html.LoadHtml(new WebClient().DownloadString(oldThread.cat_URL));
                var root = html.DocumentNode;

                //判断是否mark

                #region 判断mark网页源码示例
                /*
                 <div id="threadPageSidebarContainer" class="sideBarContainer">
                   <div id="sidebar">
                     <section>
                      <h3>Asked by:</h3>                              
                 */
                #endregion

                //判断mark规则
                /*
                 1. 获取class="sideBarContainer"对应div 
                 2. 再获取h3 InnerText == "Asked by" ? "继续判断":"不做处理"
                 */
                var markContent = root.Descendants()
                    .Where(n => n.GetAttributeValue("class", "").Equals("sideBarContainer")).Single().Descendants("h3").Single();
                var h3Content = markContent.InnerText;

                #region 是否mark

                if (h3Content.Equals("Asked by:"))
                {
                    #region 获取OP name 和 last poster name网页源码示例
                    /*
                     <div class="unified-baseball-card-mini" data-profile-userid="31316825-49fc-404c-8bec-ba3d7947c2cd" data-profile-usercard-customlink='{"href":"https://social.msdn.microsoft.com/Forums/vstudio/en-US/user/threads?user=programmervb.net", "text":"programmervb.net&#39;s threads"}'>
                         <div class="profile-mini-content">
                          </div>
                     </div>
                   */
                    #endregion

                    //获取OP name 和 last poster name
                    /*
                     1. 获取class="unified-baseball-card-mini"对应div
                     2. 获取artibute="data-profile-usercard-customlink"对应value
                     3. 正则匹配获取"=user"后面的值,由于存在%20等空格字符 需decode
                     */

                    var content = root.Descendants()
                        .Where(n => n.GetAttributeValue("class", "").Equals("unified-baseball-card-mini")).ToList();

                    if (content != null && content.Count > 0)
                    {
                        //获取OP name
                        var first = content.First().GetAttributeValue("data-profile-usercard-customlink", "");
                        var firstName = GetItemValueFromString(first, "user");

                        //获取last poster name
                        var last = content.Last().GetAttributeValue("data-profile-usercard-customlink", "");
                        var lastName = GetItemValueFromString(last, "user");

                        #region 如果Ture 添加到need follow list

                        if (firstName.Equals(lastName))
                        {
                            #region 获取last poster time网页源码示例
                            /*
                             <div class="messageFooter">
                                <div class="actions">
                                  <div class="date">Tuesday, September 05, 2017 3:55 PM</div>
                                  <div class="menu message">
                             */
                            #endregion

                            //获取last poster time
                            /*
                             1. 获取class="messageFooter"对应div集合中的最后一个
                             2. 在该div中获取class="date" InnerText
                             3. 字符串转Datetime
                             4. 添加到follow list
                             */

                            var lastMessageTime = root.Descendants().Where(n => n.GetAttributeValue("class", "")
                            .Equals("messageFooter")).Last().Descendants("div").Where(x => x.GetAttributeValue("class", "")
                            .Equals("date")).Single();

                            //Tuesday, September 05, 2017 3:40 PM 
                            // a few minutes ago
                            var lastOP = lastMessageTime.InnerText;

                            var post = new PostedTime();
                            var parLastOp = post.Caltime(lastOP);
                            oldThread.LastOP = parLastOp;
                            followList.Add(oldThread);

                        }
                        else
                        {
                            noneedFollowList.Add(oldThread);
                        }  
                        #endregion
                    }
                }
                else
                {
                    noneedFollowList.Add(oldThread);
                }
                #endregion
            }
            catch
            {
                errorFollowList.Add(oldThread);
            }
        }
        public static string GetItemValueFromString(string connectionString, string itemName)
        {
            if (!connectionString.EndsWith(";"))
                connectionString += ";";

            // \s* 匹配0个或多个空白字符
            // .*? 匹配0个或多个除 "\n" 之外的任何字符(?指尽可能少重复)
            string regexStr = itemName + "\\s*=\\s*(?<key>.*?)\"";

            Regex r = new Regex(regexStr, RegexOptions.IgnoreCase);
            Match mc = r.Match(connectionString);
            var matchName = mc.Groups["key"].Value;
            //UrlDecode %20 空格等
            return HttpUtility.UrlDecode(matchName);
        }
        public static List<FollowUpThread> RemoveEscalateThread(List<FollowUpThread> threads)
        {
            List<FollowUpThread> Temp = new List<FollowUpThread>();
            FollowContext db = new FollowContext();
            DateTime Today = DateTime.Today.AddDays(-15);
            List<string> EscalatedThreads = db.EscalatedThreads.Where(x => x.LastOP >=Today).Select(x=>x.ThreadId).ToList();
            foreach(var t in threads)
            {
                if (EscalatedThreads.Contains(t.ThreadId))
                {
                    Temp.Add(t); //remove the same thread
                }
            }
            foreach(var t in Temp)
            {
                threads.Remove(t);
            }
           
            return threads;
        }
    }
}
