using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPIISUnanswered
{
    class PostedTime
    {
        public static DateTime StartTime;
        public bool RecentPostM(string LastPostTime)
        {
            DateTime Time = CaltimeM(LastPostTime);
            TimeSpan ts = StartTime - Time;
            if (ts.TotalMinutes > 10)
            {
                return false;
            }
            return true;
        }

        public DateTime CaltimeM(string Time)
        {

            DateTime PostDate;
            Time = Time.Trim();
            Time = Time.Replace(".\r\n", "");
            if (Time.Contains("年"))
            {
                PostDate = Convert.ToDateTime(Time);
            }
            else
            {
                Time = Time.Replace(" ", "");
                string Minute;
                string Hour;
                if (Time == "数秒钟以前")
                {
                    Minute = "0";
                    Hour = "0";
                }
                else if (Time.Contains("小时") && Time.Contains("分钟"))
                {
                    Hour = Time.Split('小')[0];
                    Minute = Time.Split('分')[0].Split('时')[1];
                }
                else if (Time.Contains("小时") && !Time.Contains("分钟"))
                {
                    Hour = Time.Split('小')[0];
                    Minute = "0";
                }
                else
                {
                    Hour = "0";
                    Minute = Time.Split('分')[0];
                }
                PostDate = DateTime.UtcNow.AddHours(-1 * Convert.ToInt32(Hour.Trim())).AddMinutes(-1 * Convert.ToInt32(Minute.Trim()));

            }
            return PostDate;
        }

        public bool RecentPost(string LastPostTime)
        {
            DateTime Time = Caltime(LastPostTime);
            TimeSpan ts = StartTime - Time;
            if (ts.TotalMinutes > 10)
            {
                return false;
            }
            return true;
        }

        public bool LessThanOneMonth(string LastPostTime)
        {
            DateTime Time = Caltime(LastPostTime);
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            if (Time>=month)
            {
                return true;
            }
            return false;
        }


        public DateTime Caltime(string Time)
        {
            DateTime PostDate;
            Time = Time.Trim();
            Time = Time.Replace(".\r\n", "");
            if (Time.Contains("AM") || Time.Contains("PM"))
            {
                PostDate = Convert.ToDateTime(Time);
            }
            else
            {
                Time = Time.Replace(" ", "");
                string Minute;
                string Hour;
                if (Time == "afewsecondsago")
                {
                    Minute = "0";
                    Hour = "0";
                }
                else
                {
                    Minute = (Time.Split('m')[0]);
                    var L = Minute.Split(',').Length;
                    if (L > 1)
                    {
                        Minute = Minute.Split(',')[1];
                    }
                    else
                    {
                        if (Minute.Length > 2)
                        {
                            Minute = "0";
                        }
                        else
                        {
                            Minute = Minute.Split(',')[0];
                        }

                    }
                    Hour = Time.Split('h')[0];
                    if (Hour.Length > 2)
                    {
                        Hour = "0";
                    }
                }
                PostDate = DateTime.UtcNow.AddHours(-1 * Convert.ToInt32(Hour.Trim())).AddMinutes(-1 * Convert.ToInt32(Minute.Trim()));

            }
            return PostDate;
        }
    }
}
