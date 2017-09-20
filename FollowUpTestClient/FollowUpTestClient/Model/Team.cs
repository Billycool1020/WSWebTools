using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient.Model
{
    public class Team
    {     
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TeamName { get; set; }

        public int? OwnedTeamId { get; set; }
     
        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Engineer> Engineers { get; set; }
    }
}
