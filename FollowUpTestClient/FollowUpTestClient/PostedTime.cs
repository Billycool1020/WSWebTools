using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient
{
    class PostedTime
    {

        public bool LessThanOneMonth(string LastPostTime)
        {
            DateTime Time = Caltime(LastPostTime);

            if (DateTime.UtcNow < Time.AddMonths(1))
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
