using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSWebTool.Models
{
    public class FollowUpThread
    {
        [Key]
        public string ThreadId { get; set; }
        public string cat_msalias { get; set; }
        public string ThreadName { get; set; }
        public string cat_URL { get; set; }
        public DateTime LastOP { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
