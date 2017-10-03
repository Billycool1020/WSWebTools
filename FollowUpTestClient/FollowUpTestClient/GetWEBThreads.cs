using FollowUpTestClient.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient
{
    class GetWEBThreads
    {

        public static List<FollowUpThread> GetMSDNThreads(string Url, string Name)
        {
            var f = true;
            var index = 0;

            List<FollowUpThread> list = new List<FollowUpThread>();
            PostedTime pt = new PostedTime();

            while (f)
            {
                var p = ++index;
                string time;
                var fliterstring = "&brandIgnore=true&page=";
                var url = Url + fliterstring + p.ToString();
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(url);
                HtmlNode[] nodes = document.DocumentNode.SelectNodes("//a[@data-block='main']").ToArray();
                foreach (HtmlNode item in nodes)
                {
                    HtmlNode timenode = item.ParentNode.ParentNode.SelectSingleNode("div[3]/span[7]/span[2]");
                    if (timenode.InnerText.Split(',').Length > 1)
                    {
                        time = timenode.InnerText.Split(',')[1] + timenode.InnerText.Split(',')[2];
                    }
                    else
                    {
                        time = timenode.InnerText;
                    }
                    if (pt.LessThanOneMonth(time))
                    {
                        FollowUpThread thread = new FollowUpThread();
                        thread.cat_URL = item.Attributes["href"].Value;
                        thread.LastOP = pt.Caltime(time);
                       // thread.Product = Name;
                        thread.ThreadId = item.Attributes["data-threadId"].Value;
                        thread.ThreadName = item.InnerText;
                        list.Add(thread);
                        Console.WriteLine(item.InnerText);
                        Console.WriteLine(pt.Caltime(time));
                        Console.WriteLine();
                    }
                    else
                    {
                        f = false;
                        break;
                    }
                }
            }
            return list;
        }
    }
}
