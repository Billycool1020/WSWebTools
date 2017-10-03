using FollowUpTestClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient
{
    class Email
    {
        public static async Task SendMailAsync(List<FollowUpThread> list)
        {
            FollowContext db = new FollowContext();
            var ProductList = list.GroupBy(x => x.ProductId).ToList();
            foreach (var p in ProductList)
            {               
                var Product = db.Products.Where(x => x.Id == p.Key).FirstOrDefault();
                var ProductOwner = Product.Engineer;
                string bodyMessage = "Hi " + ProductOwner.DisplayName + ",<br /><br />";
                bodyMessage += "No answer and no follow threads for " + Product.ProductName + ":<br /><br />";
                if (p.Where(x => x.LastOP > DateTime.Now.ToUniversalTime().AddDays(-2)).Count() > 0)
                {
                    bodyMessage += "Last Post in 48 hours:<br /><table border='1' style='border-collapse: collapse;'><tr><td width='800'>Thread</td><td width='200' >Owner</td></tr>";
                    foreach (var Tlist in p.Where(x => x.LastOP > DateTime.Now.ToUniversalTime().AddDays(-2)))
                    {
                        bodyMessage += "<tr><td style='word-wrap: break-word'><a href=" + Tlist.cat_URL + ">" + Tlist.ThreadName + "</a></td>";
                        if (Tlist.cat_msalias != null)
                        {
                            bodyMessage +="<td>"+Tlist.cat_msalias+ "</td></tr>";
                        }
                        else
                        {
                            bodyMessage += "<td></td></tr>";
                        }                        
                    }
                    bodyMessage += "</table>";

                }
                if (p.Where(x => x.LastOP < DateTime.Now.ToUniversalTime().AddDays(-2)).Count() > 0)
                {
                    bodyMessage += "Other:<br /><table border='1' style='border-collapse: collapse;'><tr><td width='800'>Thread</td><td width='200'>Owner</td></tr>";
                    foreach (var Tlist in p.Where(x => x.LastOP < DateTime.Now.ToUniversalTime().AddDays(-2)))
                    {
                        bodyMessage += "<tr><td style='word - wrap: break-word'><a href=" + Tlist.cat_URL + ">" + Tlist.ThreadName + "</a></td>";
                        if (Tlist.cat_msalias != null)
                        {
                            bodyMessage += "<td>" + Tlist.cat_msalias + "</td></tr>";
                        }
                        else
                        {
                            bodyMessage += "<td></td></tr>";
                        }
                    }
                    bodyMessage += "</table>";
                }

                    
                bodyMessage += "<br><br>Have A Good Day<br><br>";

                await Send(ProductOwner.MSAlias, bodyMessage);
            }


        }

        //public static async Task SendMailAsync(List<FollowUpThread> list)
        //{
            //try
            //{
            //    List<string> ownerList = list.Select(p => p.cat_msalias).Distinct().ToList();// thread owner list             
            //    if (ownerList[0] != null)
            //    {
            //        foreach (var owner in ownerList)
            //        {
            //            if (owner != "")
            //            {
            //                var ownerThreadList = list.Where(p => p.cat_msalias == owner).GroupBy(p => (p.LastOP.Month)).OrderByDescending(p => (p.Key)).ToList();

            //                string bodyMessage = "Hi " + owner + ",<br /><br />";
            //                bodyMessage += "No answer and no follow threads: <br /><br />";
            //                bodyMessage += "This month:<br />";
            //                foreach (var Tlist in ownerThreadList[0])
            //                {
            //                    bodyMessage += "<a href=" + Tlist.cat_URL + ">" + Tlist.ThreadName + "<br /></a>";
            //                }
            //                bodyMessage += "<br />Last month:<br />";
            //                foreach (var Tlist in ownerThreadList[1])
            //                {
            //                    bodyMessage += "<a href=" + Tlist.cat_URL + ">" + Tlist.ThreadName + "<br /></a>";
            //                }

            //                bodyMessage += "<br><br>Have A Good Day<br><br>";

            //                await Send(owner, bodyMessage);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        FollowContext db = new FollowContext();
            //        var ProductList = list.GroupBy(x => x.Product).ToList();
            //        foreach (var p in ProductList)
            //        {
            //            var ProductOwner = db.Products.Where(x => x.ProductName == p.Key).FirstOrDefault().Engineer;
            //            string bodyMessage = "Hi " + ProductOwner.DisplayName + ",<br /><br />";
            //            bodyMessage += "No answer and no follow threads for " + p.Key + ":<br /><br />";
            //            foreach (var Tlist in p)
            //            {
            //                bodyMessage += "<a href=" + Tlist.cat_URL + ">" + Tlist.ThreadName + "<br /></a>";
            //            }
            //            bodyMessage += "<br><br>Have A Good Day<br><br>";

            //            await Send(ProductOwner.MSAlias, bodyMessage);
            //        }

            //    }

            //}
            //catch
            //{

            //}
        //}


        public static async Task SendErrorMailAsync(List<FollowUpThread> list)
        {
            try
            {
                string bodyMessage = "Here are Error Threads: <br><br>";
                foreach (var Tlist in list)
                {
                    bodyMessage += "<a href=" + Tlist.cat_URL + ">" + Tlist.ThreadName + "</a><br />";
                }
                await Send("v-haowli", bodyMessage);
            }
            catch
            {

            }
        }


        public async static Task Send(string target, string content)
        {

            MailMessage mailMessage = new MailMessage();
            //mailMessage.CC.Add(new MailAddress("msdnfspatls@microsoft.com"));
            //mailMessage.CC.Add(new MailAddress("v-waxia5@microsoft.com"));
            //mailMessage.To.Add(new MailAddress(target + "@Microsoft.com"));
            mailMessage.To.Add(new MailAddress("billyliu.cool@hotmail.com"));
            mailMessage.Subject = "No Follow Threads";
            mailMessage.Body = content;
            mailMessage.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }

    }
}
