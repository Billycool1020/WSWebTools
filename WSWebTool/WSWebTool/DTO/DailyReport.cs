using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSWebTool.DTO
{
    public class DailyReport
    {
        public DateTime date { get; set; }
        public int MAsk { get; set; }
        public int MMark { get; set; }
        public int MVote { get; set; }
        public int MReply { get; set; }
        public int MTotal { get; set; }

        public int TAsk { get; set; }
        public int TMark { get; set; }
        public int TVote { get; set; }
        public int TReply { get; set; }
        public int TTotal { get; set; }
        public int Total { get; set; }
    }
}