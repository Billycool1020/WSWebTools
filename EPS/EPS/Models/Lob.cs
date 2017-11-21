using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPS.Models
{
    public class Lob
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}