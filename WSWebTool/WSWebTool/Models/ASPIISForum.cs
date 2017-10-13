using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WSWebTool.Models
{
    [Table("ASPIISForums")]
    public class ASPIISForum
    {
        [Key]
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string ForumName { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}