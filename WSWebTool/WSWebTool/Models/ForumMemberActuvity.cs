using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WSWebTool.Models
{
    public class ForumMemberActuvity
    {
        public int Id { get; set; }
        [ForeignKey("ForumMember")]
        public string ForumMemberId { get; set; }
        public string Activity { get; set; }
        public string Detail { get; set; }
        public DateTime Time { get; set; }

        public virtual ForumMember ForumMember { get; set; }
    }
}