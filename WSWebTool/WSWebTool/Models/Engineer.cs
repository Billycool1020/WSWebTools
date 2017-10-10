using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSWebTool.Models
{
    public class Engineer
    { 
        [Key]
        [Required]
        [StringLength(50)]
        public string MSAlias { get; set; }
        [Required]
        [StringLength(50)]
        public string Chinese { get; set; }

        public DateTime OnBoardDate { get; set; }
        public DateTime? GoLiveDate { get; set; }
        public string ReadinessPool { get; set; }
        public int EID { get; set; }


        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public string Project { get; set; }

        [StringLength(50)]
        public string WSAlias { get; set; }

        [ForeignKey("Team")]
        public int SubTeamId { get; set; }

        public virtual Team Team { get; set; }

    }
}
