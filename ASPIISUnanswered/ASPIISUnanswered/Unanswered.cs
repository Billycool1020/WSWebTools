using ASPIISUnanswered.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ASPIISUnanswered
{
    class Unanswered
    {
        static List<ASPIISThread> list = new List<ASPIISThread>();
        public static List<ASPIISThread> UnansweredThreads()
        {

            list.Clear();

            WSWebToolEntities db = new WSWebToolEntities();
            var Asp = db.ASPIISForums.Where(x => x.Product.ProductName == "ASP.NET").ToList();
            Parallel.ForEach(Asp, Thread => GetAsp(Thread));

            var IIS = db.ASPIISForums.Where(x => x.Product.ProductName == "IIS").ToList();
            Parallel.ForEach(IIS, Thread => GetIIS(Thread));
            return list;
        }

        public static List<ASPIISThread> GetIIS(ASPIISForum forum)
        {
            #region 网页源码示例
            /*
             <td class="col1 icon-unanswered ">
            
                <h2>
                <a href="/t/2128146.aspx?Dynamically+create+date+range+and+find+records+with+matching+dates" title="[Unanswered]-Crystal Reports XI on Oracle 11 I need to create a range of dates b...">
                Dynamically create date range and find records with match...</a></h2>
                <p>            
                Created in 
                <a href="/76.aspx/1?Crystal+Reports" title="Questions and discussions about Crystal Reports.">Crystal Reports</a>.
            
                Latest Post by 
                <a href="/members/ninjaef.aspx" title="View ninjaef's public profile">
                ninjaef</a>, 09-06-2017 06:29 AM.
                <div class="clear"></div>
                
               </p>
             </td>

             
             <td class="col1 icon-unanswered ">            
                <h2>
                <a href="/t/2130156.aspx?How+to+detect+file+size+of+a+file+inside+App_Data" title="[Unanswered]-I am planning to have code to detect the file size of a db1 file I...">
                How to detect file size of a file inside App_Data</a></h2>
                <p>            
                Created by
                <a href="/members/hkbeer.aspx" title="View hkbeer's public profile">hkbeer</a>.  
                
                Latest Post by 
                <a href="/members/hkbeer.aspx" title="View hkbeer's public profile">
                hkbeer</a>, 1 hour, 18 minutes ago.
                <div class="clear"></div>     
                </p>
             </td>
             <td class="col2">3</td>
             <td class="col3 last-cell">0</td>
             


             */
            #endregion

            var f = true;
            var index = 0;

          
            PostedTime pt = new PostedTime();
            while (f)
            {
                var p = ++index;
                var url = string.Format("https://forums.iis.net/f/unresolved/" + forum.ForumId.ToString() + "/{0}/20", p);

                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(url);

                var html = new HtmlDocument();
                html.LoadHtml(new WebClient().DownloadString(url));
                var root = html.DocumentNode;

                var nodes = root.Descendants()
                        .Where(n => n.GetAttributeValue("class", "").Contains("col1 icon-unanswered")).ToList();
                if (nodes.Count == 0)
                {
                    f = false;
                }
                //foreach (HtmlNode item in nodes)
                Parallel.ForEach(nodes, (item) =>
                {
                    var anchor = item.Descendants("a").ToList()[0];
                    var CreateBy = item.Descendants("a").ToList()[1].InnerText;
                    var LastReply = item.Descendants("a").ToList()[2].InnerText.Remove(0, 18);
                    var islastop = (LastReply == CreateBy);
                    if (anchor.Attributes["title"].Value.Contains("[Unanswered]") || anchor.Attributes["title"].Value.Contains("[Answered]"))
                    {
                        ASPIISThread thread = new ASPIISThread();

                        string LastPostTime = anchor.ParentNode.NextSibling.NextSibling.ChildNodes[4].InnerText;
                        LastPostTime = LastPostTime.Remove(0, 1);
                        if (pt.LessThanTwoMonth(LastPostTime))
                        {
                            Regex r = new Regex(@"/t/\d{7}");
                            var ThreadId = r.Match(anchor.Attributes["href"].Value).Value;
                            if (ThreadId.Length == 0)
                            {
                                r = new Regex(@"/t/\d{6}");
                                ThreadId = r.Match(anchor.Attributes["href"].Value).Value.Substring(3);
                            }
                            else
                            {
                                ThreadId = ThreadId.Substring(3);
                            }

                            XmlReader reader = XmlReader.Create("https://forums.iis.net/t/rss/" + ThreadId);
                            SyndicationFeed feed = SyndicationFeed.Load(reader);
                            reader.Close();
                            SyndicationItem i = feed.Items.FirstOrDefault();
                           
                            //string PostDate = i.LastUpdatedTime.DateTime.ToString();

                            //if (pt.LessThanOneMonth(PostDate))
                            //{
                            string Title = i.Title.Text;
                            string Link = "https://forums.iis.net";
                            Link = Link + anchor.Attributes["href"].Value;
                            thread.Link = Link;
                            thread.ThreadId = ThreadId;
                            thread.PostDate = i.LastUpdatedTime.DateTime;
                            thread.Title = Title;
                            thread.forumId = forum.Id;
                            thread.IsLastOp = islastop;

                            //thread.Product = "Asp.Net " + product;
                            //thread.Id = 0;
                            list.Add(thread);

                            Console.WriteLine(ThreadId);
                            Console.WriteLine(Title);
                            //Console.WriteLine(product);
                            Console.WriteLine(LastPostTime);
                            Console.WriteLine(Link);
                            Console.WriteLine();
                            //}
                        }
                        else
                        {
                            f = false;                            
                        }

                    }
                });
            }
            Console.WriteLine(forum.ForumName + " Finish");
            return list;
        }

        public static List<ASPIISThread> GetAsp(ASPIISForum forum)
        {
            #region 网页源码示例
            /*
             <td class="col1 icon-unanswered ">
            
                <h2>
                <a href="/t/2128146.aspx?Dynamically+create+date+range+and+find+records+with+matching+dates" title="[Unanswered]-Crystal Reports XI on Oracle 11 I need to create a range of dates b...">
                Dynamically create date range and find records with match...</a></h2>
                <p>            
                Created in 
                <a href="/76.aspx/1?Crystal+Reports" title="Questions and discussions about Crystal Reports.">Crystal Reports</a>.
            
                Latest Post by 
                <a href="/members/ninjaef.aspx" title="View ninjaef's public profile">
                ninjaef</a>, 09-06-2017 06:29 AM.
                <div class="clear"></div>
                
               </p>
             </td>

             
             <td class="col1 icon-unanswered ">            
                <h2>
                <a href="/t/2130156.aspx?How+to+detect+file+size+of+a+file+inside+App_Data" title="[Unanswered]-I am planning to have code to detect the file size of a db1 file I...">
                How to detect file size of a file inside App_Data</a></h2>
                <p>            
                Created by
                <a href="/members/hkbeer.aspx" title="View hkbeer's public profile">hkbeer</a>.  
                
                Latest Post by 
                <a href="/members/hkbeer.aspx" title="View hkbeer's public profile">
                hkbeer</a>, 1 hour, 18 minutes ago.
                <div class="clear"></div>     
                </p>
             </td>
             <td class="col2">3</td>
             <td class="col3 last-cell">0</td>
             


             */
            #endregion

            var f = true;
            var index = 0;

            //List<ASPIISThread> list = new List<ASPIISThread>();
            PostedTime pt = new PostedTime();
            while (f)
            {
                var p = ++index;
                var url = string.Format("https://forums.asp.net/f/unresolved/" + forum.ForumId.ToString() + "/{0}/20", p);

                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(url);

                var html = new HtmlDocument();
                html.LoadHtml(new WebClient().DownloadString(url));
                var root = html.DocumentNode;

                var nodes = root.Descendants()
                        .Where(n => n.GetAttributeValue("class", "").Contains("col1 icon-unanswered")).ToList();

                //foreach (HtmlNode item in nodes)
                Parallel.ForEach(nodes, (item) =>                
                {
                    var anchor = item.Descendants("a").ToList()[0];
                    var CreateBy = item.Descendants("a").ToList()[1].InnerText;
                    var LastReply = item.Descendants("a").ToList()[2].InnerText.Remove(0, 18);
                    var islastop = (LastReply == CreateBy);
                    if (anchor.Attributes["title"].Value.Contains("[Unanswered]") || anchor.Attributes["title"].Value.Contains("[Answered]"))
                    {
                        ASPIISThread thread = new ASPIISThread();

                        string LastPostTime = anchor.ParentNode.NextSibling.NextSibling.ChildNodes[4].InnerText;
                        LastPostTime = LastPostTime.Remove(0, 1);
                        if (pt.LessThanTwoMonth(LastPostTime))
                        {
                            Regex r = new Regex(@"/t/\d{7}");
                            var ThreadId = r.Match(anchor.Attributes["href"].Value).Value;
                            if (ThreadId.Length == 0)
                            {
                                r = new Regex(@"/t/\d{6}");
                                ThreadId = r.Match(anchor.Attributes["href"].Value).Value.Substring(3);
                            }
                            else
                            {
                                ThreadId = ThreadId.Substring(3);
                            }

                            XmlReader reader = XmlReader.Create("https://forums.asp.net/t/rss/" + ThreadId);
                            SyndicationFeed feed = SyndicationFeed.Load(reader);
                            reader.Close();
                            SyndicationItem i = feed.Items.FirstOrDefault();

                            //string PostDate = i.LastUpdatedTime.DateTime.ToString();

                            //if (pt.LessThanOneMonth(PostDate))
                            //{
                            string Title = i.Title.Text;
                            string Link = "https://forums.asp.net";
                            Link = Link + anchor.Attributes["href"].Value;
                            thread.Link = Link;
                            thread.ThreadId = ThreadId;
                            thread.PostDate = i.LastUpdatedTime.DateTime;
                            thread.Title = Title;
                            thread.forumId = forum.Id;
                            thread.IsLastOp = islastop;
                
                            // thread.Product = "Asp.Net " + product;
                            //thread.Id = 0;
                            list.Add(thread);

                            Console.WriteLine(ThreadId);
                            Console.WriteLine(Title);
                            //Console.WriteLine(product);
                            Console.WriteLine(LastPostTime);
                            Console.WriteLine(Link);
                            Console.WriteLine();
                            //}
                        }
                        else
                        {
                            f = false;
                            //break;
                        }

                    }
                });
            }
            Console.WriteLine(forum.ForumName + " Finish");
            return list;
        }
    }
}
