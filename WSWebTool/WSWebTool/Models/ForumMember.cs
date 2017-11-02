using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSWebTool.Models
{
    public class ForumMember
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime MemberSince { get; set; }
    }
}