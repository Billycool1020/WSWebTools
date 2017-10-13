using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WSWebTool.Models
{
    public class ASPIISThread
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ThreadId { get; set; }
        public DateTime PostDate { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }

        [ForeignKey("ASPIISForum")]
        public int forumId { get; set; }        
        public bool IsLastOp { get; set; }


        public virtual ASPIISForum ASPIISForum { get; set; }
    }
}