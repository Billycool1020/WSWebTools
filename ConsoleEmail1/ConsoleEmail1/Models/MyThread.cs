using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEmail1.Models
{
    class MyThread
    {
        [Key]
        public string ThreadId { get; set; }
        public string cat_msalias { get; set; }
        public string ThreadName { get; set; }
        public string cat_URL { get; set; }
        public DateTime LastOP { get; set; }
    }
}
