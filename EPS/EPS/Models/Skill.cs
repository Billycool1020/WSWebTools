using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPS.Models
{
    public class Skill
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }
        public int ParentID { get; set; }
    }
}