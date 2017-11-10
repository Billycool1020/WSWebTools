using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPS.Models
{
    public class Employee
    {
        [Key]
        public string MSAlias { get; set; }
        public string WSAlias { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        
        public DateTime DateofBirth { get; set; }
        public string Major { get; set; }
        public DateTime OnBoardDate { get; set; }



        public string OM { get; set; }
        public string Group { get; set; }
        public string Lob { get; set; }
        public string Product { get; set; }

        public virtual ICollection<Experience> Experiences { get; set; }



    }
}