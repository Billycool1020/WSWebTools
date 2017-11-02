using ForumUsers.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumUsers
{
    class Program
    {
        static void Main(string[] args)
        {



            
            WSWebToolEntities WS = new WSWebToolEntities();
            var User = WS.ForumMembers.ToList().Skip(10000).Take(5000).ToList();
            Console.WriteLine(User.Count);
            string Url1 = "https://social.msdn.microsoft.com/Profile/";
            string Url2 = "/activity";
            int i = 0;
            List<ForumMemberActuvity> ActuvityList = new List<ForumMemberActuvity>();
            List<ForumMember> MemberList = new List<ForumMember>();
            Parallel.ForEach(User, (u) =>
            {
                try
                {
                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument document = web.Load(Url1 + u.Name + Url2);
                    HtmlNode[] nodes = document.DocumentNode.SelectNodes("//div[@data-application-name='Forums']").ToArray();

                    foreach (HtmlNode item in nodes)
                    {
                        try
                        {
                            ForumMemberActuvity actuvity = new ForumMemberActuvity();
                            HtmlNode[] divs = item.SelectNodes("div").ToArray();
                            actuvity.ForumMemberId = u.Id;
                            actuvity.Activity = divs[2].InnerHtml.Split('<')[0];
                            actuvity.Detail = divs[2].InnerHtml;

                            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                            actuvity.Time = (origin.AddSeconds((Convert.ToInt64(divs[1].OuterHtml.Split('(')[1].Split(')')[0]) / 1000)));
                            ActuvityList.Add(actuvity);
                        }
                        catch
                        {

                        }
                       


                    }


                }
                catch
                {
                    Console.WriteLine("Exception!!!:");
                    i++;
                }

            });
            WS.ForumMemberActuvities.AddRange(ActuvityList);
            WS.SaveChanges();



            //SHPEntities SHP = new SHPEntities();
            //WSWebToolEntities WS = new WSWebToolEntities();
            //var User = SHP.ForumUsers.ToList().Skip(30000).ToList();
            //Console.WriteLine(User.Count);
            //string Url1 = "https://social.msdn.microsoft.com/Profile/";
            //string Url2 = "/activity";
            //int i = 0;
            //List<ForumMember> MemberList = new List<ForumMember>();
            //Parallel.ForEach(User, (u) =>
            //{
            //    try
            //    {
            //        HtmlWeb web = new HtmlWeb();
            //        HtmlDocument document = web.Load(Url1 + u.Name + Url2);
            //        var node = document.DocumentNode.SelectNodes("//p[@class='avatar-member-date']").FirstOrDefault();

            //        ForumMember Member = new ForumMember();
            //        Member.Name = u.Name;
            //        Member.Id = u.Id;
            //        Member.MemberSince = Convert.ToDateTime(node.InnerText);
            //        MemberList.Add(Member);
            //        Console.WriteLine(u.Name + "  " + i);
            //        i++;

            //    }
            //    catch
            //    {
            //        Console.WriteLine("Exception!!!:"+ u.Name + "  " + i);
            //        i++;
            //    }

            //});
            //var list = WS.ForumMembers.Select(x => x.Id).ToList();
            //List<ForumMember> MemberList2 = new List<ForumMember>();

            //foreach(var u in MemberList)
            //{
            //    if (!list.Contains(u.Id))
            //    {
            //        MemberList2.Add(u);
            //    }
            //}
            ////Parallel.ForEach(MemberList, (u) =>
            ////{

            ////});
            //WS.ForumMembers.AddRange(MemberList2);
            //WS.SaveChanges();


        }
    }
}
