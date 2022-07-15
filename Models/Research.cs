using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    public class Research
    {
        [Key]
        public int ResearchID { get; set; }
        public string ResearchName { get; set; }
        public string ResearchHead { get; set; }
        public string ResearchDesc { get; set; }
        public int NoOfCohorts { get; set; }

        public ICollection<Donar> Donars { get; set; }

    }

    public class ResearchDto
    {
        public int ResearchID { get; set; }
        public string ResearchName { get; set; }
        public string ResearchHead { get; set; }
        public string ResearchDesc { get; set; }
        public int NoOfCohorts { get; set; }

    }
}