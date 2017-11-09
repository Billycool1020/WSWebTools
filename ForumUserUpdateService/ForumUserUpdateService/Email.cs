using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ForumUserUpdateService
{
    class Email
    {
        public static void SendErrorMail(Exception e, string Name)
        {
            try
            {
                string bodyMessage = "Here are Error in "+ Name + ": <br><br>";
                bodyMessage += "Data : " + e.Data + " <br /><br />";
                bodyMessage += "InnerException : " + e.InnerException + " <br /><br />";
                bodyMessage += "Message : " + e.Message + " <br /><br />";
                bodyMessage += "StackTrace : " + e.StackTrace + " <br /><br />";

                Send("v-haowli", bodyMessage);
            }
            catch
            {

            }
        }



        public  static void Send(string target, string content)
        {

            MailMessage mailMessage = new MailMessage();
            //mailMessage.To.Add(new MailAddress(target + "@Microsoft.com"));
            mailMessage.To.Add(new MailAddress("billyliu.cool@hotmail.com"));
            mailMessage.Subject = "Error In U/A update Service";
            mailMessage.Body = content;
            mailMessage.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
