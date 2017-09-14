
using FollowUp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FollowUp
{
    class Email
    {
        public static async Task SendMailAsync(List<FollowUpThread> list)
        {
            try
            {
                List<string> ownerList = list.Select(p => p.cat_msalias).Distinct().ToList();// thread owner list             

                foreach (var owner in ownerList)
                {
                    List<FollowUpThread> ownerThreadList = list.Where(p => p.cat_msalias == owner).ToList();

                    string bodyMessage = "Hi " + owner + ",<br><br>";
                    bodyMessage += "No answer and no follow threads: <br><br>";
                    foreach (var Tlist in ownerThreadList)
                    {
                        bodyMessage += "<a href=" + Tlist.cat_URL + ">" + Tlist.ThreadName + "<br /></a>";
                    }
                    bodyMessage += "<br><br>Have A Good Day<br><br>";
                    if (owner == "")
                    {
                        await Send("v-waxia5", bodyMessage);
                    }
                    else
                    {
                        await Send(owner, bodyMessage);
                    }
                }
            }
            catch
            {

            }
        }


        public static async Task SendErrorMailAsync(List<FollowUpThread> list)
        {
            try
            {
                string bodyMessage = "Here are Error Threads: <br><br>";
                foreach (var Tlist in list)
                {
                    bodyMessage += "<a href=" + Tlist.cat_URL +">" + Tlist.ThreadName + "</a><br />";
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
               // mailMessage.CC.Add(new MailAddress(t));
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
