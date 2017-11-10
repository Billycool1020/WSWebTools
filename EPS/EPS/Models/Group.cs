using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPS.Models
{
    public class Group
    {
        public int ID { get; set; }
        public string Name { get; set; }

        ICollection<Product> Products { get; set; }
    }
}