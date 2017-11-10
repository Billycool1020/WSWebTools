using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSWebTool.DTO
{
    public class ActivityReport
    {
        public string Month { get; set; }
        public int MemberCount { get; set; }
        public int MSDN { get; set; }
        public int TN { get; set; }
    }
}