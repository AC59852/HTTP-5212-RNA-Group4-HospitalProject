using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    public class Donar
    {
        [Key]
        public int DonarID { get; set; }
        public string DonarName { get; set; }
        public string DonarBio { get; set; }
        public int Donation { get; set; }

        [ForeignKey("Research")]
        public int ResearchID { get; set; }
        public string ResearchName { get; set; }
        public virtual Research Research { get; set; }

    }
    public class DonarDto
    {
        public int DonarID { get; set; }
        public string DonarName { get; set; }
        public string DonarBio { get; set; }
        public int Donation { get; set; }
        public int ResearchID { get; set; }
        public string ResearchName { get; set; }
    }
}