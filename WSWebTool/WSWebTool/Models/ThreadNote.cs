using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WSWebTool.Models
{
    public class ThreadNote
    {
        [Key]
        public string ThreadID { get; set; }
        [AllowHtml]
        public string Note { get; set; }

       
    }
}