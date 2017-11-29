using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPS.Models
{
    public class EmployeeSkillDetail
    {
        public int Id { get; set; }
        public string Skill { get; set; }
        public string Employee { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }
}