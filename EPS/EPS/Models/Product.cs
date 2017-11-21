using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPS.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int GroupID { get; set; }
        public virtual Group Group { get; set; }

    }
}