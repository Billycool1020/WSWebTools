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
        [Key]
        public int Id { get; set; }

        [ForeignKey("Engineer")]
        public string OwnerId { get; set; }
        [Required]
        public string ProductName { get; set; }

        [ForeignKey("Team")]
        public int SubTeamId { get; set; }

        public virtual Team Team { get; set; }
        public virtual Engineer Engineer { get; set; }
    }
}
