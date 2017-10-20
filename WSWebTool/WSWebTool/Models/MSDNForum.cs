using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSWebTool.Models
{
    [Table("MSDNForums")]
    public class MSDNForum
    {
        public string Id { get; set; }
        public string ForumName { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
