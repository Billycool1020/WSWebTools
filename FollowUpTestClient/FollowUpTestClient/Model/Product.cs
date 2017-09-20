using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient.Model
{
    public class Product
    {
        public int Id { get; set; }

        [ForeignKey("Engineer")]
        public int OwnerId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }
        public string URL { get; set; }
        [ForeignKey("Team")]
        public int SubTeamId { get; set; }

        public virtual Team Team { get; set; }
        public virtual Engineer Engineer { get; set; }
    }
}
