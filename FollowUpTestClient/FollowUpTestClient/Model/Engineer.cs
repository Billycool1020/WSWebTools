using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient.Model
{
    public class Engineer
    {
        
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MSAlias { get; set; }
        [Required]
        [StringLength(50)]
        public string Chinese { get; set; }

        public DateTime OnBoardDate { get; set; }

        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public string ForumEmail { get; set; }

        [StringLength(50)]
        public string WSAlias { get; set; }

        [ForeignKey("Team")]
        public int SubTeamId { get; set; }

        public virtual Team Team { get; set; }

    }
}
