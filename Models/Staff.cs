using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

        //TO-DO add department foreign key when the model is available 
    }

    public class StaffDto
    {
        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Department { get; set; }
    }
}