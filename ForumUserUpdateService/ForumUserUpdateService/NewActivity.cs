using ForumUserUpdateService.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumUserUpdateService
{
    class NewActivity
    {
        static public void AddNewActivity()
        {
            WSWebToolEntities WS = new WSWebToolEntities();
            var AllUser = WS.ForumMembers.ToList().ToList();
            Console.WriteLine(AllUser.Count);
            string Url1 = "https://social.msdn.microsoft.com/Profile/";
            string Url2 = "/activity";
            int i = 0;
            int vol = 5000;
            int times = AllUser.Count() / vol;
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            List<ForumMemberActivity> ActivityList = new List<ForumMemberActivity>();
            List<ForumMember> MemberList = new List<ForumMember>();


            for (var j = 0; j <= times + 1; j++)
            {
                var User = AllUser.Skip(j * vol).Take(vol).ToList();
                Parallel.ForEach(User, (u) =>
                {
                    try
                    {
                        HtmlWeb web = new HtmlWeb();
                        HtmlDocument document = web.Load(Url1 + u.Name + Url2);
                        HtmlNode[] nodes = document.DocumentNode.SelectNodes("//div[@data-application-name='Forums']").ToArray();

                        Parallel.ForEach(nodes, (item) =>
                        {
                            try
                            {
                                HtmlNode[] divs = item.SelectNodes("div").ToArray();
                                var time = (origin.AddSeconds((Convert.ToInt64(divs[1].OuterHtml.Split('(')[1].Split(')')[0]) / 1000)));
                                if (time >= DateTime.Today.AddDays(-1))
                                {
                                    ForumMemberActivity actuvity = new ForumMemberActivity();
                                    actuvity.ForumMemberId = u.Id;

                                    if (divs[2].InnerHtml.Contains(@"https://social.msdn.microsoft.com/"))
                                    {
                                        actuvity.Product = "MSDN";
                                    }
                                    else if (divs[2].InnerHtml.Contains(@"https://social.technet.microsoft.com"))
                                    {
                                        actuvity.Product = "TN";
                                    }
                                    else if (divs[2].InnerHtml.Contains(@"http://social.expression.microsoft.com/"))
                                    {
                                        actuvity.Product = "Expression";
                                    }
                                    else if (divs[2].InnerHtml.Contains(@"https://social.microsoft.com/"))
                                    {
                                        actuvity.Product = "Other";
                                    }
                                    actuvity.Forum = divs[2].InnerHtml.Split('>')[divs[2].InnerHtml.Split('>').Count() - 2];
                                    actuvity.Forum = actuvity.Forum.Split('<')[0];
                                    actuvity.Activity = divs[2].InnerHtml.Split('<')[0];
                                    actuvity.Detail = divs[2].InnerHtml;
                                    actuvity.Time = (origin.AddSeconds((Convert.ToInt64(divs[1].OuterHtml.Split('(')[1].Split(')')[0]) / 1000)));
                                    ActivityList.Add(actuvity);
                                }

                            }
                            catch
                            {
                            }
                        });
                    }
                    catch
                    {
                        Console.WriteLine("Exception!!!");

                    }
                    i++;
                    Console.WriteLine(i);
                });
            }




            var yesterday = DateTime.Today.AddDays(-1);
            var ForumMemberActivitiesList = WS.ForumMemberActivities.Where(x => x.Time > yesterday).ToList();
            List<ForumMemberActivity> ActivityList2 = new List<ForumMemberActivity>();
            Parallel.ForEach(ActivityList, (l) =>
            {
                if (ForumMemberActivitiesList.Where(x => x.ForumMemberId == l.ForumMemberId && x.Time == l.Time).Count() == 0)
                {
                    ActivityList2.Add(l);
                }
            });
            WS.ForumMemberActivities.AddRange(ActivityList2);
            WS.SaveChanges();
        }
    }
}
