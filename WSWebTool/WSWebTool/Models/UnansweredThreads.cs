using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSWebTool.Models
{
    public class UnansweredThreads
    {
        public string ThreadId { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public DateTime CreateTime { get; set; }
        public string Product { get; set; }
        public string Forum { get; set; }
        public long Temptime { get; set; }
        public string Note { get; set; }
        public bool IsLastOp { get; set; }
    }
}