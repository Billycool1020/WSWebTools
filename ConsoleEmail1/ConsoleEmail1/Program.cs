using ConsoleEmail1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace ConsoleEmail1
{
    class Program
    {
        
        public static void Main(string[] args)
        {
           
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            try
            {
                MyList myList = new MyList();
                List<MyThread> threadList = myList.GetThreadList(); // get thread model list from MyList class
                List<string> ownerList = threadList.Select(p => p.cat_msalias).Distinct().ToList();// thread owner list
                List<string> aa = threadList.Select(p => p.cat_URL).ToList();

                foreach (var owner in ownerList)
                {
                    List<string> ownerThreadList = threadList.Where(p => p.cat_msalias == owner).Select(p => p.cat_URL).ToList();
                    string ownerThread = string.Join("<br>", ownerThreadList);
                    string threadUrl = string.Join("<br>", ownerThreadList);
                    string bodyMessage = "Hi " + owner + ",<br><br>"
                                            + "No answer and no follow threads of Yours: <br><br>"
                                            + ownerThread + " <br><br>"
                                            + "Best regards,<br><br>"
                                            + "Janley";
                    //send Email
                    m.From = new MailAddress("janleysoft@gmail.com", "Janley Zhang");
                    if (m.To.Count > 0)
                    {
                        m.To.Clear();
                    }
                    MailAddress to = new MailAddress("v-lezha1@microsoft.com", owner.LastOrDefault().ToString()); //test owner
                    //auto owner
                    //  MailAddress to = new MailAddress(owner.LastOrDefault().ToString() + "@microsoft.com", owner.LastOrDefault().ToString());
                    m.To.Add(to);
                    // m.CC.Add(new MailAddress("janleysoft@outlook.com", "Display name CC"));
                    m.Subject = "No answer and no follow threads"; m.IsBodyHtml = true; m.Body = bodyMessage;
                    sc.Host = "smtp.gmail.com";
                    sc.Port = 587;
                    sc.Credentials = new System.Net.NetworkCredential("janleysoft@gmail.com", "031351203636TF");
                    sc.EnableSsl = true;
                    sc.Send(m);
                }
                Console.WriteLine("Send Email Success!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
