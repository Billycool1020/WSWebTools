using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EPS.Models
{
    public class Experience
    {
        public int Id { get; set; }
        [ForeignKey("Employee")]
        public string Owner { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string OM { get; set; }
        public string Group { get; set; }
        public string Lob { get; set; }
        public string Product { get; set; }


        public virtual Employee Employee { get; set; }
    }
}