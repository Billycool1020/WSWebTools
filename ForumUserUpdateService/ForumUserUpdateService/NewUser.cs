using ForumUserUpdateService.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumUserUpdateService
{
    class NewUser
    {
        static public void AddNewUser()
        {
            WSWebToolEntities WS = new WSWebToolEntities();
            SHPEntities SHP = new SHPEntities();
            List<ForumMember> MemberList = new List<ForumMember>();

            string Url1 = "https://social.msdn.microsoft.com/Profile/";
            string Url2 = "/activity";


            var ShpUser = SHP.ForumUsers.ToList();
            var WSUser = WS.ForumMembers.Select(x => x.Id).ToList();
            var i = 0;


            //4.65
            Parallel.ForEach(ShpUser, (u) =>
            {
                if (!WSUser.Contains(u.Id))
                {
                    try
                    {
                        HtmlWeb web = new HtmlWeb();
                        HtmlDocument document = web.Load(Url1 + u.Name + Url2);
                        var node = document.DocumentNode.SelectNodes("//p[@class='avatar-member-date']").FirstOrDefault();

                        ForumMember Member = new ForumMember();
                        Member.Name = u.Name;
                        Member.Id = u.Id;
                        Member.MemberSince = Convert.ToDateTime(node.InnerText);
                        MemberList.Add(Member);
                        Console.WriteLine(u.Name + "  " + i);
                        i++;
                       
                    }
                    catch
                    {
                        Console.WriteLine("Exception!!!:" + u.Name + "  " + i);
                        i++;
                    }
                }
            });
            WS.ForumMembers.AddRange(MemberList);
            WS.SaveChanges();

        }
    }
}
